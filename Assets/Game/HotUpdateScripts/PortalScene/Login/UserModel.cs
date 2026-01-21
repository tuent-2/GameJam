using System;
using System.Collections.Generic;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using UnityEngine;

public class UserModel : Singleton<UserModel>
{
    public string localizationVersion { get; set; }
    public bool IsUseDiamond { get; set; }
    public bool _isFirstLoaded = false;

    public event Action OnUserInfoFirstLoaded;

    public int Uid
    {
        get => UserInfoData.uid.Value;
        set => UserInfoData.uid.SetValue(value);
    }

    public UserInfoData UserInfoData { get; } = new();

    public void UpdateUserInfo(User u, List<string> varChanges)
    {
        if (varChanges.Contains(UserVarKey.GOLD))
        {
            UserInfoData.gold.SetValue((long)u.GetVariable(UserVarKey.GOLD).GetDoubleValue());
        }

        if (varChanges.Contains(UserVarKey.DIAMOND))
        {
            UserInfoData.diamond.SetValue((long)u.GetVariable(UserVarKey.DIAMOND).GetDoubleValue());
        }

        if (varChanges.Contains(UserVarKey.NAME))
        {
            UserInfoData.userName.SetValue(u.GetVariable(UserVarKey.NAME).GetStringValue());
        }

        if (varChanges.Contains(UserVarKey.UID))
        {
            Uid = u.GetVariable(UserVarKey.UID).GetIntValue();
        }

        if (varChanges.Contains(UserVarKey.AVATAR))
        {
            UserInfoData.avatar.SetValue(u.GetVariable(UserVarKey.AVATAR).GetIntValue());
        }

        if (varChanges.Contains(UserVarKey.NLD))
        {
            UserInfoData.nld.SetValue(u.GetVariable(UserVarKey.NLD).GetIntValue());
        }

        if (varChanges.Contains(UserVarKey.BONUS_MISSION))
        {
            UserInfoData.bonus.SetValue(u.GetVariable(UserVarKey.BONUS_MISSION).GetBoolValue());
        }

        if (varChanges.Contains(UserVarKey.FIRST_LOGIN))
        {
            UserInfoData.firstLogin.SetValue(u.GetVariable(UserVarKey.FIRST_LOGIN).GetBoolValue());
        }

        if (varChanges.Contains(UserVarKey.VALUE_PIGGY_BANK))
        {
            UserInfoData.PiggyBank.SetValue((long)u.GetVariable(UserVarKey.VALUE_PIGGY_BANK).GetDoubleValue());
        }

        if (varChanges.Contains(UserVarKey.FLASH_SALE_TIME))
        {
            var flashSaleVar = u.GetVariable(UserVarKey.FLASH_SALE_TIME);
            if (flashSaleVar != null && flashSaleVar.GetSFSObjectValue() != null)
            {
                var sfsObj = flashSaleVar.GetSFSObjectValue();
                var flashSale = new FlashSale(sfsObj);
                UserInfoData.FlashSaleTime.SetValue(flashSale);
            }
        }

        if (!_isFirstLoaded)
        {
            _isFirstLoaded = true;
            OnUserInfoFirstLoaded?.Invoke();
        }
    }
}

public class UserInfoData
{
    public readonly Observable<int> uid = new();
    public readonly Observable<string> userName = new("");
    public readonly Observable<int> avatar = new();
    public readonly Observable<int> nld = new();
    public readonly Observable<bool> bonus = new();
    public readonly Observable<bool> firstLogin = new();
    public readonly Observable<long> gold = new();
    public readonly Observable<long> diamond = new();
    public readonly Observable<long> PiggyBank = new(-1);
    public readonly Observable<FlashSale> FlashSaleTime = new();
}

public class FlashSale
{
    public int id;
    public double amount;
    public double discount;
    public long diamondRes;
    public long diamondOrigin;
    public long time;

    public FlashSale(ISFSObject data)
    {
        id = data.GetInt("i");
        amount = data.GetDouble("a");
        discount = data.GetDouble("d");
        diamondRes = data.GetLong("dr");
        diamondOrigin = data.GetLong("do");

        //long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        time = data.GetLong("ex");
    }
}

public static class UserVarKey
{
    public const string GOLD = "c";
    public const string DIAMOND = "d";
    public const string NAME = "n";
    public const string UID = "u";
    public const string PLAYER_POS = "pos";
    public const string BET = "b";
    public const string DEPOSIT_COIN = "dp";
    public const string AVATAR = "av";
    public const string NLD = "nld";
    public const string BONUS_MISSION = "bn";
    public const string FIRST_LOGIN = "flg";
    public const string VALUE_PIGGY_BANK = "vpb";
    public const string FLASH_SALE_TIME = "vf";
}