using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class CreditsWindow : Window
    {
        public CreditsWindow() : base("Credits")
        {
        }
        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            Primitives.DrawMultiLineText("{b}Game design and programming:{/b} Petr Hudecek\n{b}Subject matter expertise and narrative design:{/b} Kopa Leo",
                 rectMid, Colors.Front);
        }
    }
}