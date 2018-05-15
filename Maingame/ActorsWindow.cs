using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class ActorsWindow : Window
    {
        public ActorsWindow() : base("Actors")
        {

        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            int y = rectMid.Y;
            foreach (var actor in session.Actors) {
                string text = "{b}" + actor.Name + "{/b}\n" + actor.Description + "\n" + "Attitude towards ERPA: {b}" + actor.Attitude + "{/b}";

                var bounds =  Primitives.GetMultiLineTextBounds(text, rectMid, FontFamily.Small);
                Primitives.DrawMultiLineText(text, new Rectangle(rectMid.X, y, rectMid.Width, rectMid.Height), Colors.Front);
                y += bounds.Height + 15;
            }
        }
    }
}