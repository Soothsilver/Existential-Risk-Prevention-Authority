using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainGameSpace
{
    [Serializable]
    public class AgendaItem
    {
        public string Title;
        public RiskId Topic;
        public string Description;
        public string GetDescription()
        {
            return (Delayed ? "{i}{b}Already delayed.{/b} You don't need to process this agenda item before ending the turn.{/i}\n\n" : "") + Description;
        }
        public List<Option> Options = new List<Option>();
        public Action<Session> OnProduce;
        internal bool Delayed;

        public AgendaItem(string title, RiskId topic, string description, params Option[] options)
        {
            Title = title;
            Topic = topic;
            Description = description;
            Options.AddRange(options);
        }
    }
}
