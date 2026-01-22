using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class ScoreJamResponseVO : IFromSFSObject
{
    public List<PlayData> playDatas;

    public void FromSfsObject(ISFSObject o)
    {
        var data = o.GetSFSArray("s");
        playDatas = new List<PlayData>();
        foreach (SFSObject result in data)
        {
            playDatas.Add(new PlayData(result));
        }
    }
    
    [Serializable]
    public class PlayData
    {
        public readonly int id;
        public readonly int score;
        public readonly string name;
        public readonly int camelId;
        public readonly bool isDisconnet;
      

        public PlayData(SFSObject data)
        {
            id = data.GetInt("uid");
            score = data.GetInt("s");
            name = data.GetUtfString("n");
            camelId = data.GetInt("c");
            isDisconnet = data.GetBool("id");
           
        }
    }
}
