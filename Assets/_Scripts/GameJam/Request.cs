using Sfs2X.Entities.Data;
using UnityEngine;

public class Request : IToSFSObject
{
    public readonly int actionIndex;

    // public FoldRequestVO(int actionIndex)
    // {
    //     this.actionIndex = actionIndex;
    // }

    public ISFSObject ToSFSObject()
    {
        var sfsObject = new SFSObject();
        sfsObject.PutInt("i", actionIndex);
        sfsObject.PutInt("t", 1);
        return sfsObject;
    }
}
