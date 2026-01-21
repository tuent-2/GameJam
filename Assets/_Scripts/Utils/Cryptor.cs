using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public static class Cryptor
{
    /// <summary>
    /// Encrypt string by aes to bytes
    /// </summary>
    public static byte[] EncryptToBytes(string input, string key)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        keyBytes = RestoreKey(keyBytes);
        byte[] encryptedBytes = null;
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        }
        return encryptedBytes;
    }

    /// <summary>
    /// Encrypt string by aes
    /// </summary>
    public static string Encrypt(string input, string key)
    {
        var encryptedBytes = EncryptToBytes(input, key);
        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    /// Decrypt from bytes
    /// </summary>
    public static string DecryptFromBytes(byte[] input, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        keyBytes = RestoreKey(keyBytes);
        byte[] decryptedBytes = null;
        using (SymmetricAlgorithm aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            decryptedBytes = decryptor.TransformFinalBlock(input, 0, input.Length);
        }
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public static string Decrypt(string input, string key)
    {
        byte[] inputBytes = Convert.FromBase64String(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        keyBytes = RestoreKey(keyBytes);

        byte[] decryptedBytes = null;
        using (SymmetricAlgorithm aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            decryptedBytes = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
        }
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private static byte[] RestoreKey(byte[] key)
    {
        // thêm các byte 0 vào cuối cho đến khi keyBytes đủ size cho Aes
        if (key.Length < 16)
        {
            byte[] newKey = new byte[16];
            for (int i = 0; i < key.Length; i++)
            {
                newKey[i] = key[i];
            }
            for (int i = key.Length; i < 16; i++)
            {
                newKey[i] = 0;
            }
            return newKey;
        }
        return key;

    }

    public static byte[] XOR(string szPlainText, int szEncryptionKey) => Encoding.UTF8.GetBytes(XORToString(szPlainText, szEncryptionKey));

    public static string XORToString(string szPlainText, int szEncryptionKey)
    {
        StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
        StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
        char Textch;
        for (int iCount = 0; iCount < szPlainText.Length; iCount++)
        {
            Textch = szInputStringBuild[iCount];
            Textch = (char)(Textch ^ szEncryptionKey);
            szOutStringBuild.Append(Textch);
        }
        return szOutStringBuild.ToString();
    }

    public static byte[] XOR(byte[] szPlainBytes, int szEncryptionKey)
    {

        byte[] szOutBytes = new byte[szPlainBytes.Length];
        for (int iCount = 0; iCount < szPlainBytes.Length; iCount++)
        {
            szOutBytes[iCount] = (byte)(szPlainBytes[iCount] ^ szEncryptionKey);
        }
        return szOutBytes;

    }

    public static void Test()
    {

    }
}
