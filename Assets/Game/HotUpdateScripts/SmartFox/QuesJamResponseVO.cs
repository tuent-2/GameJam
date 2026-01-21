using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class QuesJamResponseVO : IFromSFSObject
{
    public int id { get; set; }
    public string question { get; set; }
    public List<AnswersDetail> answersDetail { get; set; }
    public int answerCorrectly { get; set; }
    
    
    public void FromSfsObject(ISFSObject o)
    {
        id = o.GetInt("mi");
        question = o.GetUtfString("mt");
        
        var data = o.GetSFSArray("a");
        answersDetail = new List<AnswersDetail>();
        foreach (SFSObject result in data)
        {
            answersDetail.Add(new AnswersDetail(result));
        }
        answerCorrectly = o.GetInt("ac");
    }
    
    
    [Serializable]
    public class AnswersDetail
    {
        public readonly int id;
        public readonly string content;
        
        public AnswersDetail(SFSObject data)
        {
            id = data.GetInt("id");
            content = data.GetUtfString("c");
        }
    }
}
