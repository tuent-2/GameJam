using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class Logger
{
    const int SIZE_2MB = 2097152;
    const int SIZE_2dot5MB = 2621440;
    const int SIZE_3MB = 3145728;

    static bool isInited = false;
    static string path;

    public static void Init()
    {
        path = Application.persistentDataPath + "/logs/";
        Directory.CreateDirectory(path);
        if (File.Exists(GetFilePath()))
        {
            TruncateFileLog();
        }

        isInited = true;
    }

    private static void TruncateFileLog()
    {
        if (new FileInfo(GetFilePath()).Length > SIZE_3MB)
        {
            // Set the size of file to 2.5MB when >3MB
            FileStream fs = new FileStream(GetFilePath(), FileMode.OpenOrCreate);
            Byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();

            FileStream fs2 = new FileStream(GetFilePath(), FileMode.Create);
            fs2.Write(bytes, bytes.Length - SIZE_2dot5MB, SIZE_2dot5MB);
            fs2.Flush();
            fs2.Close();
        }
    }

    /// <summary>
    /// Log a message
    /// </summary>
    /// <param name="message">pure message</param>
    public static void Log(string message)
    {
        Debug.Log(message);
        WriteToFile(message);
    }

    /// <summary>
    /// Log a message
    /// </summary>
    /// <param name="message">pure message</param>
    public static void LogError(string message)
    {
        Debug.LogError(message);
        WriteToFile($"log error: {message}");
    }

    private static void WriteToFile(string message)
    {
        if (!isInited)
        {
            Init();
        }

        var timeServer = DateTime.Now;
        long time = new DateTimeOffset(timeServer).ToUnixTimeMilliseconds();
        blockStream.Write(BitConverter.GetBytes(time), 0, 8);
        byte[] encodedMessage = Encoding.UTF8.GetBytes(message);
        blockStream.Write(BitConverter.GetBytes(encodedMessage.Length), 0, 4);

        blockStream.Write(encodedMessage, 0, encodedMessage.Length);
        //Debug.LogError(blockStream.Length);

        counter++;
        if (counter > _BLOCK_MAX_SIZE)
            WriteBlockToFile();
    }

    static int counter = 0;
    const int _BLOCK_MAX_SIZE = 5;
    static MemoryStream blockStream = new MemoryStream();

    public static void WriteBlockToFile()
    {
        if (counter <= 0) return;
        counter = 0;
        byte[] blockBytes;


        blockBytes = blockStream.ToArray(); // lưu bytes ra
        blockStream.SetLength(0); // clear


        int key = UnityEngine.Random.Range(2000, 5000); // lấy key ngẫu nhiên
        byte[] encryptedKey =
            Cryptor.XOR(BitConverter.GetBytes(key), blockBytes.Length); // encrypt key bằng size của block
        byte[] tempEncryptedBlock = Cryptor.XOR(blockBytes, blockBytes.Length); // encrypt block lần 1 bằng size của nó
        byte[] encryptedBlock = Cryptor.XOR(tempEncryptedBlock, key); // encrypt block lần 2 bằng key của

        try
        {
            using (FileStream fs = new FileStream(GetFilePath(), FileMode.Append))
            {
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    w.Write(blockBytes.Length); // 4 byte đầu cho block size (x)
                    w.Write(encryptedBlock); // x byte tiếp theo cho block
                    w.Write(encryptedKey); // 4 byte cuối cho key
                }
            }

            TruncateFileLog();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static void SystemLog(string logString, string stackTrace)
    {
        Log(logString + "\n" + stackTrace);
    }


    // method get filepath 
    public static string GetFilePath()
    {
        return path + "data.bin";
    }


    public static void UploadLogToServer()
    {
        var filePath = GetFilePath();
        var zipPath = path + "/data";
        if (GameUtils.ZipFile(filePath, zipPath))
        {
            // Api.UploadLogFile($"{UserModel.Instance.Uid}.zip", File.ReadAllBytes(zipPath),
            //     () => { Toast.ShowString("Send log success to server"); },
            //     message => { Toast.ShowString($"Send log error: {message}"); });
        }
        else
        {
            Debug.LogError("File to upload log file to server");
        }
    }
}