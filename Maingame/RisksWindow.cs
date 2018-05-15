using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class RisksWindow : Window
    {
        public RisksWindow() : base("Known Existential Risks")
        {
        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            int y = rectMid.Y;
            foreach (var risk in session.Risks)
            {
                string text = "{b}" + risk.Name + "{/b}\n" + risk.Description; /*\nRisk assessments:\n";
                foreach(var kvp in risk.Assessments)
                {
                    text += " " + kvp.Key.Name + ": {b}" + kvp.Value + "{/b}\n";
                }              */
                var bounds = Primitives.GetMultiLineTextBounds(text, rectMid, FontFamily.Small);
                Primitives.DrawMultiLineText(text, new Rectangle(rectMid.X, y, rectMid.Width, rectMid.Height), Colors.Front);
                y += bounds.Height + 15;
            }
        }
    }
}