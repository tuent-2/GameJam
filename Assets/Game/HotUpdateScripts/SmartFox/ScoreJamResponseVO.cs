using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class ScoreJamResponseVO : IFromSFSObject
{
    public List<Score> scores;

    public void FromSfsObject(ISFSObject o)
    {
        var data = o.GetSFSArray("s");
        scores = new List<Score>();
        foreach (SFSObject result in data)
        {
            scores.Add(new Score(result));
        }
    }
    
    [Serializable]
    public class Score
    {
        public readonly int id;
        public readonly int score;
      

        public Score(SFSObject data)
        {
            id = data.GetInt("uid");
            score = data.GetInt("s");
        }
    }
}
