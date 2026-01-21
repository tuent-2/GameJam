using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameExtension
{
    public static T GetRandom<T>(this List<T> items)
    {
        return items[Random.Range(0, items.Count)];
    }

    public static T GetRandom<T>(this List<T> items, Func<T, bool> predicate)
    {
        var filterItems = items.Where(predicate).ToList();
        return filterItems[Random.Range(0, filterItems.Count)];
    }

    public static string FormatCoin(this long value, int digit = 2)
    {
        return FormatCoin((double)value, digit);
    }

    public static string FormatMoney(this long value)
    {
        return FormatMoney((double)value);
    }

    public static string FormatMoneySprite(this long value)
    {
        var digits = value.ToString().Select(digit => digit).ToArray();
        var moneyString = new StringBuilder();
        moneyString.Append("<sprite=10>");
        for (var i = 0; i < digits.Length; i++)
        {
            moneyString.Append($"<sprite={digits[i]}>");
        }

        return moneyString.ToString();
    }

    public static string FormatMoney(this double value)
    {
        return value.ToString("N0", CultureInfo.InvariantCulture);
    }

    public static string FormatCoin(this double value, int digit = 2)
    {
        if (value >= 1000000000000)
        {
            value /= 1000000000000;
            return Math.Round(value, digit) + "T";
        }

        if (value >= 1000000000)
        {
            value /= 1000000000;
            return Math.Round(value, digit) + "B";
        }

        if (value >= 1000000)
        {
            value /= 1000000;
            return Math.Round(value, digit) + "M";
        }

        if (!(value >= 1000)) return value.ToString();
        value /= 1000;
        return Math.Round(value, digit) + "K";
    }

    public static byte[] ToBytes(this ISFSArray o)
    {
        if (o == null || o.Size() == 0)
        {
            return Array.Empty<byte>();
        }

        var temp = new byte[o.Size()];
        for (int i = 0; i < o.Size(); i++)
        {
            temp[i] = (byte)o.GetInt(i);
        }

        return temp;
    }

    public static int[] ToInts(this ISFSArray o)
    {
        if (o == null || o.Size() == 0)
        {
            return Array.Empty<int>();
        }

        var temp = new int[o.Size()];
        for (int i = 0; i < o.Size(); i++)
        {
            temp[i] = o.GetInt(i);
        }

        return temp;
    }

    public static string[] ToStrings(this ISFSArray o)
    {
        if (o == null || o.Size() == 0)
        {
            return Array.Empty<string>();
        }

        var temp = new string[o.Size()];
        for (int i = 0; i < o.Size(); i++)
        {
            temp[i] = o.GetUtfString(i);
        }

        return temp;
    }

    public static long[] ToLongs(this ISFSArray o)
    {
        if (o == null || o.Size() == 0)
        {
            return Array.Empty<long>();
        }

        var temp = new long[o.Size()];
        for (int i = 0; i < o.Size(); i++)
        {
            temp[i] = o.GetLong(i);
        }

        return temp;
    }

    public static long GetLongValue(this ISFSObject o, string key)
    {
        long x = 0;
        try
        {
            x = o.GetInt(key);
        }
        catch (InvalidCastException)
        {
            x = o.GetLong(key);
        }

        return x;
    }

    public static double GetDoubleValue(this ISFSObject o, string key)
    {
        double x = 0;
        try
        {
            x = o.GetDouble(key);
        }
        catch (InvalidCastException)
        {
            x = o.GetInt(key);
        }

        return x;
    }

    public static long GetLongValue(this BuddyVariable o)
    {
        long x = 0;
        try
        {
            x = o.GetIntValue();
        }
        catch (InvalidCastException)
        {
            x = (long)o.GetDoubleValue();
        }

        return x;
    }

    public static int GetByteOrDefault(this SFSObject o, string key)
    {
        var x = 0;
        try
        {
            x = o.GetByte(key);
        }
        catch (InvalidCastException)
        {
            x = o.GetInt(key);
        }

        return x;
    }

    public static int GetIntOrDefault(this SFSObject o, string key)
    {
        var x = 0;
        try
        {
            x = o.GetInt(key);
        }
        catch (InvalidCastException)
        {
            x = o.GetByte(key);
        }

        return x;
    }

    public static T Cast<T>(this MonoBehaviour mono) where T : class
    {
        var t = mono as T;
        return t;
    }

    public static List<string> SortByDescendingLength(this List<string> list)
    {
        return list.OrderByDescending(item => item.Length).ToList();
    }
}