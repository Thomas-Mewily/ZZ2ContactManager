using System.Security.Cryptography;
using System.Text;

namespace ContactManager;

public class Encoder
{
    public static byte[] StringToKey(string keyString)
    {
        string salt = "AddingSomeSalt" + keyString;
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(salt));
    }

    public static void EncryptTextToFile(string plainText, string filePath, string key)
    {
        byte[] keyBytes = StringToKey(key);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.GenerateIV(); // Generate a new IV for each encryption
        byte[] IV = aesAlg.IV; // Get the IV for potential future use during decryption

        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, IV);

        using FileStream fsEncrypt = new(filePath, FileMode.Create);
        fsEncrypt.Write(IV, 0, IV.Length); // Write IV to the beginning of the file

        using CryptoStream csEncrypt = new(fsEncrypt, encryptor, CryptoStreamMode.Write);
        using StreamWriter swEncrypt = new(csEncrypt);
        swEncrypt.Write(plainText);
    }

    public static string DecryptTextFromFile(string filePath, string key)
    {
        byte[] keyBytes = StringToKey(key);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;

        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        byte[] IV = new byte[aesAlg.IV.Length];

        using FileStream fsDecrypt = new(filePath, FileMode.Open);
        fsDecrypt.Read(IV, 0, IV.Length); // Read IV from the beginning of the file

        aesAlg.IV = IV;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using CryptoStream csDecrypt = new(fsDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);
        return srDecrypt.ReadToEnd();
    }

}
