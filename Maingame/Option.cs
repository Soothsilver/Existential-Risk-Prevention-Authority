using System;
using System.Linq;

namespace MainGameSpace
{
    [Serializable]
    public class Option
    {
        public string Title;

        public Action ImmediateAction;
        public Action<AgendaItem, Session> CompletionAction;

        public int TeamsRequired;
        public int TurnsRequired;

        public Option(string title, Action<AgendaItem, Session> action)
        {
            Title = title;
            CompletionAction = action;
        }

        internal static Option CloseAgendaItem(string caption = "Understood")
        {
            return new Option(caption, (item, session) =>
            {
            });
        }

        internal bool CanWeTakeIt(Session session)
        {
            return session.Teams.Count(tm => tm.Working == false) >= this.TeamsRequired;
        }

        internal static Option AnalyzeIssue(int teams, int work, string analysisResults)
        {
            Option o = new Option("Analyze the issue (" + teams + " teams, " + work + " turns)", (item, s) =>
            {
                item.Description = "{b}Analysis result: {/b}" + analysisResults + "\n\n" + item.Description;
                item.Options.RemoveAll(opt => opt.Title.StartsWith("Analyze the issue"));
                s.Items.Add(item);
            })
            {
                TeamsRequired = teams,
                TurnsRequired = work
            };
            return o;
        }

        internal static Option TimedWork(string name, int teams, int turns, Action<AgendaItem, Session> completion, bool hideTime = false)
        {
            string nm = name;
            if (teams >= 1 || (turns >= 1 && !hideTime))
            {
                nm += " (";
                if (teams >= 1)
                {
                    nm += teams + " teams";
                    if (turns >= 1)
                    {
                        nm += ", ";
                    }
                }
                if (turns >= 1)
                {
                    nm += turns + " turns";
                }
                nm += ")";
            }

            Option o = new Option(nm, (item, s) =>
            {
                completion(item, s);
            })
            {
                TeamsRequired = teams,
                TurnsRequired = turns
            };
            return o;
        }

        internal static Option Delay()
        {
            return Option.TimedWork("Delay for now", 0, 3, (ai, s) =>
              {
                  ai.Delayed = true;
                  s.Items.Add(ai);
              });
        }

        internal static Option ImmediateThing(string title, Action immediateAction)
        {
            return new Option(title, (ai, ss) => { })
            {
                ImmediateAction = immediateAction
            };
        }
        
    }
}
