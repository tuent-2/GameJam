using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class Response : IFromSFSObject
{
    public List<OnlineQuest> OnlineQuests;
    public List<GameQuest> GameQuests;

    public void FromSfsObject(ISFSObject o)
    {
        var data = o.GetSFSArray("otq");
        OnlineQuests = new List<OnlineQuest>();
        foreach (SFSObject result in data)
        {
            OnlineQuests.Add(new OnlineQuest(result));
        }

        var data1 = o.GetSFSArray("gq");
        GameQuests = new List<GameQuest>();
        foreach (SFSObject result in data1)
        {
            GameQuests.Add(new GameQuest(result));
        }
    }


    [Serializable]
    public class OnlineQuest
    {
        public readonly int id;
        public readonly int time;
        public readonly int reward;
        public readonly int progress;
        public readonly bool isClaimed;

        public OnlineQuest(SFSObject data)
        {
            id = data.GetInt("i");
            time = data.GetInt("trq");
            reward = data.GetInt("rw");
            progress = data.GetInt("pg");
            isClaimed = data.GetBool("ic");
        }
    }

    [Serializable]
    public class GameQuest
    {
        public readonly int id;
        public readonly int game;
        public readonly int reward;
        public readonly int progress;
        public readonly bool isClaimed;

        public GameQuest(SFSObject data)
        {
            id = data.GetInt("i");
            game = data.GetInt("gr");
            reward = data.GetInt("rw");
            progress = data.GetInt("pg");
            isClaimed = data.GetBool("ic");
        }
    }
}