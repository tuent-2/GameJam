using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class GameUtils
{
    private static readonly List<int> ALPHA_CHAR_CODES = new List<int>()
        { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70 };

    public static bool IsAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    public static bool IsIOS()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
        // return true;
    }

    public static bool IsWeb()
    {
        return Application.platform == RuntimePlatform.WebGLPlayer;
    }

    public static string GetPlatform()
    {
#if UNITY_IOS
        return "IOS";
#elif UNITY_ANDROID
        return "Android";
#endif
        return "Web";
    }

    public static string GetVersion()
    {
        return Application.version;
    }

    public static string GetVersionBundle()
    {
        //return Resources.Load<LoadConfig>("LoadConfig").version;
        return "1";
    }

    public static bool IsEditor()
    {
        return Application.platform == RuntimePlatform.OSXEditor ||
               Application.platform == RuntimePlatform.WindowsEditor;
    }

    public static void CheckAndCreateFolder(this string path)
    {
        var dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    public static void SaveAssets(Object target)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(target);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public static List<FileInfo> GetAllFiles(DirectoryInfo info)
    {
        if (!info.Exists)
        {
            info.Create();
        }

        var temps = info.GetDirectories();
        var ls = new List<FileInfo>();
        ls.AddRange(info.GetFiles());
        foreach (var temp in temps)
        {
            ls.AddRange(GetAllFiles(temp));
        }

        return ls;
    }

    public static double BytesToMb(double bytes)
    {
        return Math.Round(bytes / 1024 / 1024, 2);
    }

    public static double BytesToKb(double bytes)
    {
        return Math.Round(bytes / 1024, 2);
    }

    public static string ConvertToMbOrKb(double bytes)
    {
        if (bytes > 1024 * 1024)
            return $"{BytesToMb(bytes)}MB";

        if (bytes > 1024)
            return $"{BytesToKb(bytes)}KB";

        return $"{Math.Round(bytes, 2)}B";
    }

    public static string GetDeviceId()
    {
        if (!IsWeb() && !IsEditor()) return SystemInfo.deviceUniqueIdentifier;
        var s = LocalStorageUtils.DeviceId;
        if (string.IsNullOrEmpty(s))
        {
            s = IsWeb() || IsEditor() ? CreateID() : SystemInfo.deviceUniqueIdentifier;
            LocalStorageUtils.DeviceId = s;
        }

        return s;
    }

    public static string CreateID()
    {
        var uid = new int[36];
        var index = 0;
        var rand = new System.Random();
        for (var i = 0; i < 8; i++)
        {
            uid[index++] = ALPHA_CHAR_CODES[rand.Next(0, 15)];
        }

        for (var i = 0; i < 3; i++)
        {
            uid[index++] = 45; // charCode for "-"
            for (var j = 0; j < 4; j++)
            {
                uid[index++] = ALPHA_CHAR_CODES[rand.Next(0, 15)];
            }
        }

        uid[index++] = 45; // charCode for "-"
        //        var time:Number = new Date().getTime();
        var time = DateTime.Now.Millisecond;
        // Note: time is the number of milliseconds since 1970,
        // which is currently more than one trillion.
        // We use the low 8 hex digits of this number in the UID.
        // Just in case the system clock has been reset to
        // Jan 1-4, 1970 (in which case this number could have only
        // 1-7 hex digits), we pad on the left with 7 zeros
        // before taking the low digits.
        var timeString = ("0000000" + time.ToString("X16")).ToUpper().Substring(0, 8);
        for (var i = 0; i < 8; i++)
        {
            uid[index++] = timeString[i];
        }

        for (var i = 0; i < 4; i++)
        {
            uid[index++] = ALPHA_CHAR_CODES[rand.Next(0, 15)];
        }

        var res = uid.Aggregate("", (current, c) => current + Convert.ToChar(c));
        Debug.Log("res " + res);
        return res;
    }

    public static int GetMaxIndexHasValue(int[] arr, int targetValue)
    {
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i] >= targetValue)
            {
                return i;
            }
        }

        return -1;
    }

    public static bool ZipFile(string filePath, string zipPath)
    {
        try
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using var zipToOpen = new FileStream(zipPath, FileMode.Create);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
            archive.CreateEntryFromFile(filePath, "data.txt");
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}