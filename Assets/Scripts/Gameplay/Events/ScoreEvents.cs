using System;


public static class ScoreEvents
{
    public static Action<int> OnLoadedScoreData;
    public static Action<int> OnAddedPlayerScore;
}
