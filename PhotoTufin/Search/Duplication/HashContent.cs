using System;
using System.IO;
using System.Security.Cryptography;

namespace PhotoTufin.Search.Duplication;

public static class HashContent
{
    /// <summary>
    /// Creates a md5 hash of the file content. 
    /// </summary>
    /// <param name="pathToFile"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Converts bytes to a hash string.
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
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