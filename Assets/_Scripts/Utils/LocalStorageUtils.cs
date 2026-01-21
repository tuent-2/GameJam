using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization.Settings;

public enum LocalStorageEnum
{
    //Portal
    Username,
    Password,
    CompleteRateApp,
    RateAppDay,
    DeviceID,
    UserToken,
    LoginKey,
    RecentPlayGames,
    FcmToken,

    //TeangLen
    TeanglenCurrencyType,

    //Slot3x3
    Slot3X3Bet,
    Slot3X3CurrencyType,

    //Slot3x5
    Slot3X5Bet,
    Slot3X5CurrencyType,

    //Ads  
    AdWatchCount,
    AdWatchDate,

    //Localization
    Localization,
}

public static class LocalStorageUtils
{
    public static int Localization
    {
        get => PlayerPrefs.GetInt(LocalStorageEnum.Localization.ToString(), LocalizeText.VN_POSITION);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Localization.ToString(), value);
    }

    public static void SetAdsWatchCount(int value)
    {
        PlayerPrefs.SetInt(LocalStorageEnum.AdWatchCount.ToString(), value);
    }

    public static int GetAdsWatchCount()
    {
        return PlayerPrefs.GetInt(LocalStorageEnum.AdWatchCount.ToString(), 0);
    }

    public static void SetAdsWatchDate(string date)
    {
        SetString(LocalStorageEnum.AdWatchDate.ToString(), date);
    }

    public static string GetAdsWatchDate()
    {
        return GetString(LocalStorageEnum.AdWatchDate.ToString());
    }

    public static void SetFCMToken(string fcmToken)
    {
        SetString(LocalStorageEnum.FcmToken.ToString(), fcmToken);
    }

    public static string GetFCMToken()
    {
        return GetString(LocalStorageEnum.FcmToken.ToString());
    }

    public static bool GetSceneSound(SceneId sceneId)
    {
        return GetBoolean(sceneId + "Sound", true);
    }

    public static void SetSceneSound(SceneId sceneId, bool value)
    {
        SetBoolean(sceneId + "Sound", value);
    }

    public static bool GetSceneMusic(SceneId sceneId)
    {
        return GetBoolean(sceneId + "Music", true);
    }

    public static void SetSceneMusic(SceneId sceneId, bool value)
    {
        SetBoolean(sceneId + "Music", value);
    }


    public static List<int> RecentPlayGames
    {
        get => GetList<int>(LocalStorageEnum.RecentPlayGames.ToString());
        set => SetList(LocalStorageEnum.RecentPlayGames.ToString(), value);
    }

    public static int CurrencyType3X5
    {
        get => PlayerPrefs.GetInt(LocalStorageEnum.Slot3X5CurrencyType.ToString(), 1);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Slot3X5CurrencyType.ToString(), value);
    }

    public static int CurrencyType3X3
    {
        get => PlayerPrefs.GetInt(LocalStorageEnum.Slot3X3CurrencyType.ToString(), 1);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Slot3X3CurrencyType.ToString(), value);
    }

    public static CurrencyType CurrencyTypeTeangLen
    {
        get => (CurrencyType)PlayerPrefs.GetInt(LocalStorageEnum.Slot3X3CurrencyType.ToString(), 1);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Slot3X3CurrencyType.ToString(), (int)value);
    }

    public static int Current3x3Bet
    {
        get => PlayerPrefs.GetInt(LocalStorageEnum.Slot3X3Bet.ToString(), 1);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Slot3X3Bet.ToString(), value);
    }

    public static int Current3x5Bet
    {
        get => PlayerPrefs.GetInt(LocalStorageEnum.Slot3X5Bet.ToString(), 1);
        set => PlayerPrefs.SetInt(LocalStorageEnum.Slot3X5Bet.ToString(), value);
    }

    public static string LoginUsername
    {
        get => PlayerPrefs.GetString(LocalStorageEnum.Username.ToString(), "");
        set => PlayerPrefs.SetString(LocalStorageEnum.Username.ToString(), value);
    }

    public static string LoginPassword
    {
        get => PlayerPrefs.GetString(LocalStorageEnum.Password.ToString(), "");
        set => PlayerPrefs.SetString(LocalStorageEnum.Password.ToString(), value);
    }


    public static bool IsRateApp
    {
        get => GetBoolean(LocalStorageEnum.CompleteRateApp.ToString());
        set => SetBoolean(LocalStorageEnum.CompleteRateApp.ToString(), value);
    }

    public static long RateAppDay
    {
        get => GetLong(LocalStorageEnum.RateAppDay.ToString(), -1);
        set => SetLong(LocalStorageEnum.RateAppDay.ToString(), value);
    }

    public static string UserToken
    {
        get => GetString(LocalStorageEnum.UserToken.ToString());
        set => SetString(LocalStorageEnum.UserToken.ToString(), value);
    }

    public static string DeviceId
    {
        get => GetString(LocalStorageEnum.DeviceID.ToString());
        set => SetString(LocalStorageEnum.DeviceID.ToString(), value);
    }

    public static string LoginKey
    {
        get => GetString(LocalStorageEnum.LoginKey.ToString());
        set => SetString(LocalStorageEnum.LoginKey.ToString(), value);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, EncryptUtils.Encrypt(value));
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static string GetString(string key)
    {
        var data = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(data)) return "";
        return EncryptUtils.Decrypt(data);
    }

    public static bool GetBoolean(string key, bool defaultValue = false)
    {
        var value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
        if (value == 1) return true;
        return false;
    }

    public static void SetBoolean(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static void SetLong(string key, long value)
    {
        PlayerPrefs.SetString(key, EncryptUtils.Encrypt(value.ToString()));
    }

    public static long GetLong(string key, long d = 0)
    {
        var value = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(value))
        {
            return long.Parse(EncryptUtils.Decrypt(value));
        }

        return d;
    }

    public static List<T> GetList<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var d = JsonConvert.DeserializeObject(PlayerPrefs.GetString(LocalStorageEnum.RecentPlayGames.ToString()),
                typeof(List<T>));
            return d as List<T>;
        }

        return new List<T>();
    }

    public static void SetList<T>(string key, List<T> values)
    {
        var jsonValues = JsonConvert.SerializeObject(values);
        PlayerPrefs.SetString(LocalStorageEnum.RecentPlayGames.ToString(), jsonValues);
    }
}