using Sfs2X.Entities.Data;
using UnityEngine;

public class PlayJamResponseVO : IFromSFSObject
{
    public int answer { get; set; }
    public int questionId { get; set; }
    
    public void FromSfsObject(ISFSObject o)
    {
        answer = o.GetInt("a");
        questionId = o.GetInt("qid");
    }
}
