using System;

namespace MainGameSpace
{
    [Serializable]
    public class Actor
    {
        public string Name;
        public string Description;
        public Attitude Attitude;
        public Attitude MaxAttitude;

        public Actor(string name, string description)
        {
            Name = name;
            Description = description;
            Attitude = Attitude.Friendship;
            MaxAttitude = Attitude.Friendship;
        }

        internal void WorsenAttitude(int howMuch)
        {
            Attitude -= 2;
            if (Attitude < 0) Attitude = 0;
            Session.Self.AttitudeChanges.Add(new AttitudeChange(Name, false, Attitude));
        }

        internal void ImproveAttitude(int v)
        {
            Attitude += 1;
            if (Attitude > Attitude.Love) Attitude = Attitude.Love;
            Session.Self.AttitudeChanges.Add(new AttitudeChange(Name, true, Attitude));
            int diff = Attitude - MaxAttitude;
            if (diff > 0)
            {
                for (int i =0; i < diff; i++)
                {
                    Session.Self.Trigger(new AgendaItem(Name + "'s attitude improves", RiskId.Generic,
                        Name + " is now more friendly and accepting of our cause and they go beyond the requirements of the law to help us. They have provided funding for an additional team that we may use as we wish.", Option.CloseAgendaItem()));
                    Session.Self.Teams.Add(new ErpaTeam(RiskId.Generic));
                }
                MaxAttitude = Attitude;
            }
        }
    }
}