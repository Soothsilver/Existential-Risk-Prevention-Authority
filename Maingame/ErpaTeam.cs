using System;

namespace MainGameSpace
{
    [Serializable]
    public class ErpaTeam
    {
        public RiskId SpecializedFor;
        public DelayedAgendaItem WorkingOn;
        public bool Working => WorkingOn != null;

        public ErpaTeam(RiskId specialization)
        {
            SpecializedFor = specialization;
        }
    }   
}