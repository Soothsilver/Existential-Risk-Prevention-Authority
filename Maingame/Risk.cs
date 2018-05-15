using System;
using System.Collections.Generic;

namespace MainGameSpace
{
    [Serializable]
    public class Risk
    {
        public string Name;
        public RiskId Id;
        public string Description;
        public Dictionary<Actor, Assessment> Assessments = new Dictionary<Actor, Assessment>();

        public Risk(RiskId id, string name, string description, Assessment us, Assessment eu, Assessment un, Assessment jp)
        {
            Id = id;
            Name = name;
            Description = description;
            Assessments.Add(Session.Self.US, us);
            Assessments.Add(Session.Self.EU, eu);
            Assessments.Add(Session.Self.UN, un);
            Assessments.Add(Session.Self.JP, jp);
        }
        public Risk(RiskId id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }


        internal AgendaItem IntroductionAgendaItem()
        {
            return new AgendaItem(Name + " Risk Introduction", Id, Description, Option.CloseAgendaItem())
            {
                OnProduce = (session) => session.Risks.Add(this)
            };
        }
    }
}