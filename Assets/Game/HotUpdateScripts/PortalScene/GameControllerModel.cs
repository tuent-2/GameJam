using Sfs2X.Entities.Data;
using UnityEngine;

public class GameControllerModel : Singleton<GameControllerModel>
{
    public readonly Observable<ScoreJamResponseVO> scoreJamResponse = new();
    public readonly Observable<QuesJamResponseVO> quesJamResponse = new();
    
    public readonly Observable<UserDataResponse>  userDataResponse = new();
    public readonly Observable<LeaderBoardSO>  leaderBoardResponse = new();
    
  
  
    public void QuesJamResponse(SFSObject sfsObject)
    {
        Debug.Log("QuesJamResponse");
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

    public void UpdateLeaderBoardJamResponse(SFSObject sfsObject)
    {
        var data = new LeaderBoardSO();
        data.FromSfsObject(sfsObject);
        leaderBoardResponse.SetValue(data);
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
        SmartFoxConnection.Instance.SendExt(Cmd.StartJam, paramsObj);
    }

    public void SendCancelMatching()
    {
        SmartFoxConnection.Instance.SendExt(Cmd.CancelReq);
    }
    
    public void SendGetLeaderBoard()
    {
        Debug.Log("SendGetLeaderBoard");
        SmartFoxConnection.Instance.SendExt(Cmd.LeaderBoardJam);
    }

    
    public void SendPlayRequest(int answer, int questionId)
    {
        var paramsObj = new SFSObject();
        paramsObj.PutInt("a", answer);
        paramsObj.PutInt("qid", questionId);
        SmartFoxConnection.Instance.SendExt(Cmd.PlayReq, paramsObj);
    }
    
}
