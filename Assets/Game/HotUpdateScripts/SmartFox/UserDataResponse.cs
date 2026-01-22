using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class UserDataResponse : IFromSFSObject
{
    public int score;
    public int camelId;
    public int sumGame;
    public int sumGameWin;
    public int age;
    public string name;
    public List<ThanhTuu> ThanhTuus;
    
    public void FromSfsObject(ISFSObject o)
    {
        name = o.GetUtfString("n");
        score = o.GetInt("s");
        camelId = o.GetInt("ci");
        sumGame = o.GetInt("tp");
        sumGame = o.GetInt("tw");
        sumGameWin = o.GetInt("a");
        age = o.GetInt("ci");
       
        
        var data = o.GetSFSArray("tt");
        ThanhTuus = new List<ThanhTuu>();
        foreach (SFSObject result in data)
        {
            ThanhTuus.Add(new ThanhTuu(result));
        }
    }
    
    
    [Serializable]
    public class ThanhTuu
    {
        public readonly int subjectId;
        public readonly int totalMatch;
        public readonly int totalWin;
      

        public ThanhTuu(SFSObject data)
        {
            subjectId = data.GetInt("sId");
            totalMatch = data.GetInt("tm");
            totalWin = data.GetInt("tw");
        }
    }
}
