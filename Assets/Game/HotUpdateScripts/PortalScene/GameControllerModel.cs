using Sfs2X.Entities.Data;
using UnityEngine;

public class GameControllerModel : Singleton<GameControllerModel>
{
    public readonly Observable<ScoreJamResponseVO> scoreJamResponse = new();
    public readonly Observable<QuesJamResponseVO> quesJamResponse = new();
    
    public void UpdateCorePlayer()
    {
        
    }

    public void PlayJamResponse(SFSObject sfsObject)
    {
        
    }
    
  
    public void QuesJamResponse(SFSObject sfsObject)
    {
        var data = new QuesJamResponseVO();
        data.FromSfsObject(sfsObject);
        quesJamResponse.SetValue(data);
    } 
    
    public void UpdateScoreJamResponse(SFSObject sfsObject )
    {
        var data = new ScoreJamResponseVO();
        data.FromSfsObject(sfsObject);
        scoreJamResponse.SetValue(data);
    }
    
    public void SendSetUpClass(int classId, int location)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("id", classId);
        paramsObj.PutInt("l", location);
        
        SmartFoxConnection.Instance.SendExt(Cmd.UpdateClassJam, paramsObj);
    }

    public void SendStartPlay(int subjectId, int language)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("s", subjectId);
        paramsObj.PutInt("l", language);
        SmartFoxConnection.Instance.SendExt(Cmd.UpdateClassJam, paramsObj);
    }

    public void SendPlayRequest(int answer, int questionId)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("a", answer);
        paramsObj.PutInt("qid", questionId);
        SmartFoxConnection.Instance.SendExt(Cmd.PlayReq, paramsObj);
    }
    
}
