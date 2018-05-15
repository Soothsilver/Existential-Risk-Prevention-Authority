using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class TeamsWindow : Window
    {
        public TeamsWindow() : base("ERPA Teams")
        {

        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            int y = rectMid.Y;
            int i = 1;
            foreach (var team in session.Teams)
            {
                string text = "{b}" + "ERPA Team " + i + "{/b}\n" +
                    (team.Working ? "Working on a project" : "{Green}Available.{/color}");

                var bounds = Primitives.GetMultiLineTextBounds(text, rectMid, FontFamily.Small);
                Primitives.DrawMultiLineText(text, new Rectangle(rectMid.X, y, rectMid.Width, rectMid.Height), Colors.Front);
                y += bounds.Height + 15;
                i++;
            }
        }
    }
}