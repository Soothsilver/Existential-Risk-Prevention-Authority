using Auxiliary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGameSpace
{
    [Serializable]
    public class Session
    {
        public static Session Self;
        public int Year = 2018;
        public List<ErpaTeam> Teams = new List<ErpaTeam>();
        public List<Actor> Actors = new List<Actor>();
        public List<Risk> Risks = new List<Risk>();
        public ObservableCollection<AgendaItem> Items = new ObservableCollection<AgendaItem>();
        public List<DelayedAgendaItem> FutureAgenda = new List<DelayedAgendaItem>();
        public Actor US;
        public Actor UN;
        public Actor EU;
        public Actor JP;
        public bool Flag_YarkowskyComplete;
        public bool Flag_AsteroidDeflected;
        internal bool Flag_IonComplete;
        internal bool Flag_WeaponsDestroyed;
        internal bool Flag_NuclearComplete;
        internal bool Flag_BioReady;
        internal bool Flag_BanImplemented;
        public List<AttitudeChange> AttitudeChanges = new List<AttitudeChange>();

        public Session()
        {
            Self = this;
        }

        public bool Flag_AsteroidIdentified { get; internal set; }

        internal static Session Start()
        {
           
            var s = new Session();
            s.Items.CollectionChanged += s.Items_CollectionChanged;
            s.Teams.Add(new ErpaTeam(RiskId.Generic));
            s.Teams.Add(new ErpaTeam(RiskId.Generic));
            s.US = new Actor("United States", "The United States of America are a nuclear world actor concerned with their power and protective of their interests.");
            s.EU = new Actor("European Union", "The European Union championed the creation of ERPA and believes itself to be good at scientific research.");
            s.UN = new Actor("United Nations", "The United Nations represent the entire world and are the source of your funding but it is difficult for them to enforce action.");
            s.JP = new Actor("Japan", "Japan is capable of advanced scientific and technological research, but not all of their scientists propose meaningful projects.");
            s.Actors.Add(s.US);
            s.Actors.Add(s.EU);
            s.Actors.Add(s.UN);
            s.Actors.Add(s.JP);

            Story.CreateStory(s);
            /*
            foreach (var itm in s.Items)
            {
                itm.OnProduce?.Invoke(s);
            }
            */
            return s;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as AgendaItem)?.OnProduce?.Invoke(this);
                }
            }
        }

        internal void EndTurn()
        {
            AttitudeChanges.Clear();
            Year++;
            for (int ai = FutureAgenda.Count -1; ai >= 0; ai--)
            {
                var dai = FutureAgenda[ai];
                dai.TurnsUntilCreation--;
                if(dai.TurnsUntilCreation <= 0)
                {
                    // Free teams
                    foreach(var team in Teams)
                    {
                        if (team.WorkingOn == dai)
                        {
                            team.WorkingOn = null;
                        }
                    }
                    // Execute
                    FutureAgenda.RemoveAt(ai);
                    dai.CompletionAction(dai.AgendaItem, this);
                }
            }
            
            // Check victory
            if (Year == 2040)
            {
                Root.PopFromPhase();
                Root.PushPhase(new VictoryPhase());
            }
        }

        internal void Trigger(AgendaItem dai)
        {
            this.Items.Add(dai);
        }

        internal void AsteroidCulminationCheck()
        {
            if (Flag_AsteroidIdentified)
            {
                if (Flag_YarkowskyComplete)
                {
                    Trigger(new AgendaItem("Yarkowsky Project Finale", RiskId.Asteroid,
                       "The asteroid, Nemesis, is approaching Earth but you have the technology to deflect it using the Yarkowsky Project. But you must hurry because even though all the technology is now available, it will still take time.",
                       Option.TimedWork("Launch the project.", 1, 5, (ai, ss) =>
                        {
                            ss.Flag_AsteroidDeflected = true;
                            ss.FutureAgenda.RemoveAll(dai => dai.AgendaItem?.Topic == RiskId.Asteroid);
                            ss.Trigger(new AgendaItem("Asteroid Deflected", RiskId.Asteroid, "Excellent! The project worked. The asteroid was successfully deflected onto a stable orbit around the Sun and will not impact our world.", Option.CloseAgendaItem("Bullet dodged.")));
                        }),
                       Option.Delay(),
                       Option.CloseAgendaItem("End the project.")));
                }
                if (Flag_IonComplete)
                {
                    Trigger(new AgendaItem("Ion Engine Launch", RiskId.Asteroid,
                    "The asteroid, Nemesis, is approaching Earth but you have the technology to deflect it using ion engines. But you must hurry because even though all the technology is now available, it will still take time. ",
                    Option.TimedWork("Launch the project.", 1, 5, (ai, ss) =>
                    {
                        ss.Flag_AsteroidDeflected = true;
                        ss.FutureAgenda.RemoveAll(dai => dai.AgendaItem?.Topic == RiskId.Asteroid);
                        ss.Trigger(new AgendaItem("Asteroid Deflected", RiskId.Asteroid, "Excellent! The project worked. The asteroid was successfully deflected onto a stable orbit around the Sun and will not impact our world.", Option.CloseAgendaItem("Bullet dodged.")));
                    }),
                    Option.Delay(),
                    Option.CloseAgendaItem("End the project.")));
                }
                if (Flag_NuclearComplete)
                {
                    Trigger(new AgendaItem("Nuclear Asteroid Deflection Launch", RiskId.Asteroid,
                  "The asteroid, Nemesis, is approaching Earth but you have the technology to deflect it using nuclear bombs. Will you launch them?. ",
                  Option.TimedWork("Launch the nukes.", 1, 5, (ai, ss) =>
                  {
                      ss.Flag_AsteroidDeflected = true;
                      ss.FutureAgenda.RemoveAll(dai => dai.AgendaItem?.Topic == RiskId.Asteroid);
                      ss.Trigger(new AgendaItem("Asteroid Deflected", RiskId.Asteroid, "Excellent! The project worked. The explosions could not be seen by the naked eye but astronomical pictures circulate on the internet. The asteroid was successfully deflected onto a stable orbit around the Sun and will not impact our world.", Option.CloseAgendaItem("Bullet dodged.")));
                  }),
                  Option.Delay(),
                  Option.CloseAgendaItem("End the project.")));
                }
            }
        }

        internal void GameOver(string text)
        {
            Root.PopFromPhase();
            Root.PushPhase(new VictoryPhase(Year, text));
        }

        internal void TriggerInfo(string text, string buttonCaption = "Understood")
        {
            this.Trigger(new AgendaItem("Information", RiskId.Generic, text, Option.CloseAgendaItem(buttonCaption)));
        }
    }
}
