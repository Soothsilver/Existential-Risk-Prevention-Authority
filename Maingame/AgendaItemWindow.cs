using Auxiliary;
using Auxiliary.GUI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MainGameSpace
{
    internal class AgendaItemWindow : Window
    {
        public AgendaItem item;
        public List<Button> Buttons = new List<Button>();

        public AgendaItemWindow(AgendaItem item) : base(item.Title)
        {
            this.item = item;
        }
        public override void InitializeMiddle(Rectangle rectMid)
        {
            base.InitializeMiddle(rectMid);
            string midtext = item.GetDescription();
            var bounds = Primitives.GetMultiLineTextBounds(midtext, rectMid, FontFamily.Small);
            int y = rectMid.Bottom - 10 - item.Options.Count * 30;
            foreach(var opt in item.Options)
            {
                var buttonoption = new Button(opt.Title, new Rectangle(rectMid.X, y, rectMid.Width - 40, 30));
                buttonoption.Tag = opt;
                buttonoption.Click += Buttonoption_Click;
                Buttons.Add(buttonoption);
                y += 30;
            }
        }

        public void Buttonoption_Click(Button obj)
        {
            Option opt = (Option)obj.Tag;
            if (!opt.CanWeTakeIt(MainPhase.Session))
            {
                Root.SendToast("Not enough teams are available for that option.", icon: GuiIcon.Warning);
                return;
            }
            Session session = MainPhase.Session;
            this.MainPhase.Session.Items.Remove(item);
            this.MainPhase.CloseWindow();
            opt.ImmediateAction?.Invoke();
            var dai = new DelayedAgendaItem(item, opt.CompletionAction, opt.TurnsRequired);
            for (int i = 0; i < opt.TeamsRequired; i++)
            {
                var tm = session.Teams.First(team => !team.Working);
                tm.WorkingOn = dai;
            }
            session.FutureAgenda.Add(dai);
            this.MainPhase.UpdateRightBar();
        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            string midtext = item.GetDescription();

            //  Primitives.DrawSingleLineText(midtext, new Vector2(rectMid.X, rectMid.Y), Color.Black, Assets.FontLittle);

            Primitives.DrawMultiLineText(midtext, rectMid, Colors.Front);
            foreach(var b in Buttons)
            {
                b.Draw();
            }

        }

        public override void Update(MainPhase mainPhase, Session session, float elapsedSeconds)
        {
            base.Update(mainPhase, session, elapsedSeconds);
            foreach(var b in Buttons)
            {
                b.Update();
            }
            if (Root.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter) &&
                !Root.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter, ModifierKey.Alt))
            {
                if (Buttons.Count > 0)
                {
                    Buttonoption_Click(Buttons[0]);
                }
                else
                {
                    MainPhase.CloseWindow();
                }
            }
        }

    }
}