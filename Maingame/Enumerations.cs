using System;

namespace MainGameSpace
{
    [Serializable]
    public enum Attitude
    {
        Hostile = 0,
        Negative,
        Neutral,
        Friendship,
        CloseFriendship,
        Adoration,
        Love
    }
    [Serializable]
    public enum RiskId
    {
        Generic,
        Supervolcano,
        NuclearWeapons,
        NuclearTerrorism,
        NuclearWinter,
        Asteroid,
        RedHerring,
        Pandemic
    }
    [Serializable]
    public enum Assessment
    {
        DeniedRisk = 0,
        NoAssessment,
        RecognizedRisk,
        Priority,
        TopPriority
    }
}