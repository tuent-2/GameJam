using Sfs2X.Entities.Data;
using UnityEngine;

public class GameConnection : BaseConnection
{
    protected override void HandleExtensionData(string cmd, SFSObject data)
    {
        switch (cmd)
        {
            case Cmd.PlayScoreDataCmd:
                GameControllerModel.Instance.UpdateScoreJamResponse(data);
                break;
            case Cmd.QuesCmd:
                GameControllerModel.Instance.QuesJamResponse(data);
                break;
            
        }
    }
}
