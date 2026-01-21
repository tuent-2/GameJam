using System;

public class StringUtils
{
    public static bool CheckLoginForm(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            Toast.Show(GameMsg.USERNAME_BLANK, isClearOther: true);
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            Toast.Show(GameMsg.PASSWORD_BLANK, isClearOther: true);
            return false;
        }

        return true;
    }

    public static bool CheckRegisterForm(string userName, string passWord, string confirmPassWord)
    {
        if (string.IsNullOrEmpty(userName))
        {
            Toast.Show(GameMsg.USERNAME_BLANK, isClearOther: true);
            return false;
        }

        if (string.IsNullOrEmpty(passWord))
        {
            Toast.Show(GameMsg.PASSWORD_BLANK, isClearOther: true);
            return false;
        }

        if (string.IsNullOrEmpty(passWord))
        {
            Toast.Show(GameMsg.CONFIRM_PASSWORD_BLANK, isClearOther: true);
            return false;
        }

        if (string.CompareOrdinal(passWord, confirmPassWord) != 0)
        {
            Toast.Show(GameMsg.CONFIRM_NOT_EQUAL, isClearOther: true);
            return false;
        }
        //
        // if (passWord.Contains("111111") ||
        //     passWord.Contains("123456") ||
        //     passWord.Contains("123123") ||
        //     passWord.Equals(userName))
        // {
        //     Toast.Show(GameMsg.VERY_SIMPLE_PASSWORD, isClearOther: true);
        //     return false;
        // }

        return true;
    }

    public static string FormatMoney(long v, long max = 1000000000)
    {
        if (v < max)
        {
            return v.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
        }
        else
        {
            return FormatMoneyK(v);
        }
    }

    public static string FormatMoneyK(double value, int digit = 2)
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
}