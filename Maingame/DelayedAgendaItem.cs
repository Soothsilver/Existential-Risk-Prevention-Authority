using System;

namespace MainGameSpace
{
    [Serializable]
    public class DelayedAgendaItem
    {
        public Action<AgendaItem, Session> CompletionAction;
        /// <summary>
        /// If this is 1, then it will be created during the next night.
        /// </summary>
        public int TurnsUntilCreation;
        public AgendaItem AgendaItem;

        public DelayedAgendaItem(AgendaItem ai, Action<AgendaItem, Session> completionAction, int turnsUntilCreation)
        {
            AgendaItem = ai;
            TurnsUntilCreation = turnsUntilCreation;
            CompletionAction = completionAction;
        }
    }
}