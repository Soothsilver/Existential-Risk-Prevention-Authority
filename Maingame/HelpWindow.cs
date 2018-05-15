using Auxiliary;
using Microsoft.Xna.Framework;

namespace MainGameSpace
{
    internal class HelpWindow : Window
    {
        public HelpWindow() : base("Help")
        {
        }

        public override void DrawMiddle(Session session, Rectangle rectMid)
        {
            Primitives.DrawMultiLineText("{b}Objective:{/b} You lead the Existential Risk Prevention Authority (ERPA). Your goal is to see humankind survive until the end of year 2040.\n\n{b}Gameplay:{/b}Each turn (year), you make decision on the agenda items listed in the right bar. Each decision you make has consequences, sometimes next year, sometimes many years later. You must make a decision for each agenda item before ending the turn.\n\n{b}Teams:{/b} ERPA teams are your most important resource. Most decisions will require you to allocate one or more teams to execute them. Use them wisely.\n\n{b}Keyboard shortcuts:{/b} Press Esc to close the active window. Press Enter to select the first option in a window.\n\n{b}Actors:{/b} You are not alone in the world. Major players on the world stage will come to your help, providing extra teams, for example, if you treat them well.\n\n{b}Risks:{/b} If ever a risk becomes reality and destroys humankind, you lose the game and must start over. Make sure that doesn't happen. However, some risks are not really extinction-level risks or may be blown out of proportion. Concentrate on what is necessary right now, but also plan for the future.",
                rectMid, Colors.Front);
        }
    }
}