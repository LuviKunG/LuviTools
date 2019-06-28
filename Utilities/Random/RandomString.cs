using UnityEngine;

public static class RandomString
{
    public enum StringRandomType
    {
        Default,
        LowerCase,
        UpperCase,
        UpperAndLowerCase,
        LowerCaseWithNumber,
        UpperCaseWithNumber
    }

    public static string GetString(int length, StringRandomType type = StringRandomType.Default)
    {
        string valid;
        switch (type)
        {
            case StringRandomType.LowerCase:
                valid = "abcdefghijklmnopqrstuvwxyz";
                break;
            case StringRandomType.UpperCase:
                valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                break;
            case StringRandomType.UpperAndLowerCase:
                valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                break;
            case StringRandomType.LowerCaseWithNumber:
                valid = "abcdefghijklmnopqrstuvwxyz0123456789";
                break;
            case StringRandomType.UpperCaseWithNumber:
                valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                break;
            default:
                valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                break;
        }
        string res = "";
        while (0 < length--)
            res += valid[Random.Range(0, valid.Length)];
        return res;
    }
}
