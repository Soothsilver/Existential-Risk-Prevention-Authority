namespace MainGameSpace
{
    public class AttitudeChange
    {
        public string Actor;
        public bool Up;
        public Attitude NewAttitude;
        public AttitudeChange(string actor, bool up, Attitude newAtt)
        {
            Actor = actor;
            Up = up;
            NewAttitude = newAtt;
        }
    }
}