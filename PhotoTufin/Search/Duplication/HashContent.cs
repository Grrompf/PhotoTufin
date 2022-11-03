using System;
using System.IO;
using System.Security.Cryptography;

namespace PhotoTufin.Search.Duplication;

public class HashContent
{
    public static byte[]? readHash(string pathToFile)
    {
        try
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(pathToFile);
            
            return md5.ComputeHash(stream);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
    
    public static string convertHash(byte[]? hash)
    {
        try
        {
            return hash == null ? "-" : BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return "-";
    }
}