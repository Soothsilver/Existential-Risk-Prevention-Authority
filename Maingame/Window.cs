using Auxiliary;
using Auxiliary.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGameSpace
{
    class Window
    {
        public string Title;
        public bool Initialized;
        public Button bClose;
        public Rectangle RectMid;
        public MainPhase MainPhase;

        public Window(string title)
        {
            Title = title;
        }

        public void Initialize(Rectangle rect, MainPhase mp)
        {
            Initialized = true;
            MainPhase = mp;
            bClose = new Button("Close", new Rectangle(rect.Right - 80, rect.Y, 80, 30));
            bClose.Click += BClose_Click;
            RectMid = new Rectangle(rect.X + 6, rect.Y + 45, rect.Width - 12, rect.Height - 55);
            InitializeMiddle(RectMid);
        }

        public virtual void InitializeMiddle(Rectangle rectMid)
        {

        }

        private void BClose_Click(Button obj)
        {
            MainPhase.CloseWindow();
        }

        public virtual void DrawMiddle(Session session, Rectangle rectMid)
        {

        }

        public virtual void Draw(SpriteBatch sb, Rectangle rect, float elapsedSeconds)
        {
            Primitives.FillRectangle(rect, Colors.Back);
            Rectangle rectTitle = new Rectangle(rect.X, rect.Y, rect.Width, 30);
            Primitives.FillRectangle(rectTitle, Colors.HighlightBack);
            Primitives.DrawRectangle(rectTitle, Colors.Front);
            Primitives.DrawMultiLineText("{b}" + Title + "{/b}", rectTitle.Extend(-5, -2), Colors.Front, FontFamily.Small, Primitives.TextAlignment.Left);
            DrawMiddle(MainPhase.Session, RectMid);
            Primitives.DrawRectangle(rect, Colors.Front);
            bClose.Draw();
        }
        public virtual void Update(MainPhase mainPhase, Session session, float elapsedSeconds)
        {
            bClose.Update();
        }
    }
}
