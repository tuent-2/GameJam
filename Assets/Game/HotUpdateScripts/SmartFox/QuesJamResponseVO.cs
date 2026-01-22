using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class QuesJamResponseVO : IFromSFSObject
{
    public int id { get; set; }
    public string question { get; set; }
    public List<Content> answersDetail { get; set; }
    public int answerCorrectly { get; set; }
    
    
    public void FromSfsObject(ISFSObject o)
    {
        id = o.GetInt("id");
        question = o.GetUtfString("q");
        
        var data = o.GetSFSArray("a");
        answersDetail = new List<Content>();
        foreach (SFSObject result in data)
        {
            answersDetail.Add(new Content(result));
        }
        answerCorrectly = o.GetInt("ac");
    }
    
    [Serializable]
    public class Content
    {
        
        public readonly string questionContent;
        
        public Content(SFSObject data)
        {
            questionContent = data.GetUtfString("d");

        }
    }
   
}
