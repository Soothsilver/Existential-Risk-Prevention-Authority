using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class MessageWindow : Window
    {
        public string Text;
        public MessageWindow(string title) : base("Information")
        {
            Text = title;
        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            Primitives.DrawMultiLineText(Text, rectMid, Colors.Front, FontFamily.Small, Primitives.TextAlignment.Middle);
        }

        public override void Update(MainPhase mainPhase, Session session, float elapsedSeconds)
        {
            base.Update(mainPhase, session, elapsedSeconds);
            if (Root.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {            
                    MainPhase.CloseWindow();
            }
        }
    }
}