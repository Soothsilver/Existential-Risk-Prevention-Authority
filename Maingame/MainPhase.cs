using Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Auxiliary.GUI;

namespace MainGameSpace
{
    class MainPhase : GamePhase
    {
        Window ActiveWindow = null;
        private Session _session;
        public MainPhase()
        {
            Session = Session.Start();
        }
        public MainPhase(Session session)
        {
            Session = session;
        }
        public Session Session
        {
            get { return _session; }
            set
            {

                _session = value;
                Session.Self = value;
            }
        }
        Rectangle rectLeftBar = new Rectangle(0, 0, 20, Root.ScreenHeight);
        Rectangle rectRightBar = new Rectangle(Root.ScreenWidth - 20, 0, 20, Root.ScreenHeight);
        Rectangle rectBottomBar = new Rectangle(0, Root.ScreenHeight - 50, Root.ScreenWidth, 50);
        List<Button> RightBarButtons = new List<Button>();
        Rectangle rectMiddleWindow = new Rectangle(210, 20, Root.ScreenWidth - 400, Root.ScreenHeight - 100);
        Button bTeams;
        protected override void Initialize(Game game)
        {
            // Bottom bar
            Button bQuit = new Button("Credits", new Rectangle(rectBottomBar.X, rectBottomBar.Y, 120, rectBottomBar.Height));
            Button bFS = new Button("Fullscreen [Alt+Enter]", new Rectangle(rectBottomBar.X+120, rectBottomBar.Y, 200, rectBottomBar.Height));
            Button bEndTurn = new Button("End Turn", new Rectangle(rectBottomBar.Right - 200, rectBottomBar.Y, 200, rectBottomBar.Height));
            AddUIElement(bQuit); AddUIElement(bFS);
            AddUIElement(bEndTurn);
          //  AddUIElement(bQuickLoad);

            // Left bar
            Button bHelp = new Button("Help", new Rectangle(rectLeftBar.X, rectLeftBar.Y, 200, 40));
            bTeams = new Button("ERPA Teams", new Rectangle(rectLeftBar.X, rectLeftBar.Y+40, 200, 40));
            Button bRisks = new Button("Known Risks", new Rectangle(rectLeftBar.X, rectLeftBar.Y+80, 200, 40));
            Button bActors = new Button("Actors", new Rectangle(rectLeftBar.X, rectLeftBar.Y+120, 200, 40));
            AddUIElement(bHelp);
            AddUIElement(bTeams);
            AddUIElement(bRisks);
            AddUIElement(bActors);



            // Actions
            bQuit.Click += BQuit_Click;
            bFS.Click += BFS_Click;
            bHelp.Click += BHelp_Click;
            bActors.Click += BActors_Click;
            bTeams.Click += BTeams_Click;
            bRisks.Click += BRisks_Click;
            bEndTurn.Click += BEndTurn_Click;

            UpdateRightBar();
            if (Session.Year == 2018 && Session.Items.Count > 0 && Session.Items[0].Title.Equals("Introduction"))
            {
                OpenWindow(new AgendaItemWindow(Session.Items[0]));
            }

            base.Initialize(game);
        }

        private void BFS_Click(Button obj)
        {
            this.ToggleFullscreen();
        }

        public void ToggleFullscreen()
        {
            if (Root.IsFullscreen)
            {
                Root.SetResolution(1280, 900);
                Root.IsFullscreen = false;
                Root.PopFromPhase();
                Root.PushPhase(new MainPhase(Session));
            }
            else
            {
                Root.SetResolution(Utilities.GetSupportedResolutions().FindLast(m => true));
                Root.IsFullscreen = true;
                Root.PopFromPhase();
                Root.PushPhase(new MainPhase(Session));
            }
        }

        private void BEndTurn_Click(Button obj)
        {
            if (Session.Items.Count(itm => !itm.Delayed) == 0)
            {
                Session.EndTurn();
                UpdateRightBar();
            }
            else
            {
                Root.SendToast("You must clear all agenda items in the right bar before you end the turn.");
                OpenWindow(new AgendaItemWindow(Session.Items.Where(itm=>!itm.Delayed).First()));
                //OpenWindow(new MessageWindow("You must clear all agenda items in the right bar before you end the turn."));
            }
        }

        private void BRisks_Click(Button obj)
        {
            OpenWindow(new RisksWindow());
        }

        private void BTeams_Click(Button obj)
        {
            OpenWindow(new TeamsWindow());
        }

        private void BActors_Click(Button obj)
        {
            OpenWindow(new ActorsWindow());
        }

        internal void CloseWindow()
        {
            ActiveWindow = null;
        }

        private void BHelp_Click(Button obj)
        {
            OpenWindow(new HelpWindow());
        }

        public void OpenWindow(Window window)
        {
            window.Initialize(rectMiddleWindow, this);
            ActiveWindow = window;
        }

        private void BQuit_Click(Button obj)
        {
            OpenWindow(new CreditsWindow());
        }
        public void UpdateRightBar()
        {
            bTeams.Caption = "Teams (" + Session.Teams.Count(tm => !tm.Working) + " free)";
            RightBarButtons.Clear();
            int y = rectRightBar.Bottom - 100;
            foreach(var ai in Session.Items)
            {
                Button b = new Button((ai.Title.Length < 40 ? ai.Title : ai.Title.Substring(0, 37) + "...")
                    + (ai.Delayed ? " (delayed)": ""),
                    new Rectangle(Root.ScreenWidth - 350, y, 350, 30));
                b.Tag = ai;
                b.AlignRight = true;
                b.Click += B_Click;
                b.RightClick += B_RightClick;
                RightBarButtons.Add(b);
                y -= 30;
            }
        }

        private void B_RightClick(Button obj)
        {
            AgendaItem item = (AgendaItem)obj.Tag;
            AgendaItemWindow thaWindow;
            thaWindow = new AgendaItemWindow(item);
            thaWindow.Initialize(Root.Screen, this);
            if (thaWindow.Buttons.Count == 1)
            {
                thaWindow.Buttonoption_Click(thaWindow.Buttons[0]);
            }
        }

        private void B_Click(Button obj)
        {
            AgendaItem item = (AgendaItem)obj.Tag;
            OpenWindow(new AgendaItemWindow(item));
        }

        protected override void Draw(SpriteBatch sb, Game game, float elapsedSeconds)
        {
            Primitives.FillRectangle(Root.Screen, Colors.TotalBackground);
            Primitives.DrawImage(Assets.Blue, Root.Screen);
            Primitives.DrawAndFillRectangle(rectLeftBar, Colors.HighlightBack, Colors.Front);
            Primitives.DrawAndFillRectangle(rectRightBar, Colors.HighlightBack, Colors.Front);
            Primitives.DrawAndFillRectangle(rectBottomBar, Colors.HighlightBack, Colors.Front);
            Primitives.DrawMultiLineText("Year " + Session.Year, new Rectangle(rectBottomBar.Right - 500, rectBottomBar.Y, 120, rectBottomBar.Height), Colors.Front, FontFamily.Small, Primitives.TextAlignment.Middle);
            // Right bar
            foreach(var rb in RightBarButtons)
            {
                rb.Draw();
            }
            // Attitude changes
            int y = 180;
            foreach(var att in Session.AttitudeChanges)
            {
                var r = new Rectangle(0, y, 200, 80);
                Primitives.DrawAndFillRectangle(r, att.Up ? Color.LightGreen : Color.Pink, Color.Black);
                Primitives.DrawMultiLineText(att.Up ? "Attitude improves" : "Attitude worsens", r.Extend(-2, -2), Color.Black);
                Primitives.DrawMultiLineText("{b}" + att.Actor + "{/b}", new Rectangle(r.X + 2, r.Y + 30, r.Width, r.Height), Color.Black);
                Primitives.DrawMultiLineText("Now {i}" + att.NewAttitude + "{/i}", new Rectangle(r.X + 2, r.Y + 50, r.Width, r.Height), Color.Black);
                y += 80;
            }
            ActiveWindow?.Draw(sb, rectMiddleWindow, elapsedSeconds);

            base.Draw(sb, game, elapsedSeconds);
        }
        protected override void Update(Game game, float elapsedSeconds)
        {
            ActiveWindow?.Update(this, Session, elapsedSeconds); 
            // Right bar
            foreach (var rb in RightBarButtons.ToList())
            {
                rb.Update();
            }
            if (Root.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                CloseWindow();
            }
            base.Update(game, elapsedSeconds);
        }
    }
}
