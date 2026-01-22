using Sfs2X.Entities.Data;
using UnityEngine;

public class SumUpVO : IFromSFSObject
{
    public bool isWin;
    
    public void FromSfsObject(ISFSObject o)
    {
        isWin = o.GetBool("w");
    }
}
