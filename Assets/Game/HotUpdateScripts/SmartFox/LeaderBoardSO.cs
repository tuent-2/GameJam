using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class LeaderBoardSO : IFromSFSObject
{
    public List<LeaderBoard> LeaderBoards { get; set; }
    public void FromSfsObject(ISFSObject o)
    {
        var data = o.GetSFSArray("d");
        LeaderBoards = new List<LeaderBoard>();
        foreach (SFSObject result in data)
        {
            LeaderBoards.Add(new LeaderBoard(result));
        }
    }
    
    [Serializable]
    public class LeaderBoard
    {
        public readonly int uId;
        public readonly string name;
        public readonly int camelId;
        public readonly int score;
        public readonly string description;
        
        public LeaderBoard(SFSObject data)
        {
            uId = data.GetInt("u");
            name = data.GetUtfString("n");
            camelId = data.GetInt("c");
            score = data.GetInt("s");
            if (!data.ContainsKey("d")) return;
            description = data.GetUtfString("d");
        }
    }
}
