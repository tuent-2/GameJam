using Sfs2X.Entities;

public static class SfsUserExtension
{
    public static int GetUid(this User user)
    {
        return 1;
        //return user.GetVariable(UserVarKey.UID).GetIntValue();
    }

    public static string GetName(this User user)
    {
        return "Tue";
        //return user.GetVariable(UserVarKey.NAME).GetStringValue();
    }
}