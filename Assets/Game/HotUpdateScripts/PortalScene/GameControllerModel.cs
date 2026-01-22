using Sfs2X.Entities.Data;
using UnityEngine;

public class GameControllerModel : Singleton<GameControllerModel>
{
    public readonly Observable<ScoreJamResponseVO> scoreJamResponse = new();
    public readonly Observable<QuesJamResponseVO> quesJamResponse = new();
    
    public readonly Observable<UserDataResponse>  userDataResponse = new();
    
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
    
    public void UpdateUserData(SFSObject sfsObject )
    {
        var data = new UserDataResponse();
        data.FromSfsObject(sfsObject);
        userDataResponse.SetValue(data);
    }
    /// <summary>
    /// /////////////
    /// </summary>
    /// <param name="classId"></param>
    /// <param name="location"></param>
    public void SendSetUpClass(int year)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("y", year);
        SmartFoxConnection.Instance.SendExt(Cmd.UpdateClassJam, paramsObj);
    }

    public void SendStartPlay(int subjectId)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("s", subjectId);
        SmartFoxConnection.Instance.SendExt(Cmd.UpdateClassJam, paramsObj);
    }

    public void SendCancelMatching()
    {
        SmartFoxConnection.Instance.SendExt(Cmd.CancelReq);
    }
    
    public void SendPlayRequest(int answer, int questionId)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("a", answer);
        paramsObj.PutInt("qid", questionId);
        SmartFoxConnection.Instance.SendExt(Cmd.PlayReq, paramsObj);
    }
    
}
