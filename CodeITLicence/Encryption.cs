using System;
using System.IO;
using System.Security.Cryptography;


namespace CodeITLicence
{
    public static class Encryption
    {
        const string Key = "XMIqZxMiHmktYc0OHQjQgA==";
        const string GlobalIv = "QmnrC4Th2idgNqfFXmbhiA==";

        public static string Encrypt(this string text)
        {
            string res = "";
            try
            {
                // Check arguments.
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                byte[] encrypted;
                // Create an AesManaged object
                // with the specified key and IV.
                using (var aesAlg = new AesManaged())
                {

                    aesAlg.KeySize = 128;
                    aesAlg.Key = Convert.FromBase64String(Key);

                    aesAlg.IV = Convert.FromBase64String(GlobalIv);


                    // Create a decrytor to perform the stream transform.
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (var msEncrypt = new MemoryStream())
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                res = Convert.ToBase64String(encrypted);

            }
            catch (Exception ex)
            {
                return null;
            }

            // Return the encrypted bytes from the memory stream.
            return res;
        }

        public static string Decript(this string text)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var cipherText = Convert.FromBase64String(text);

            // Declare the string used to hold
            // the decrypted text.
            string result;

            // Create an AesManaged object
            // with the specified key and IV.
            using (var aesAlg = new AesManaged())
            {

                // Create a decrytor to perform the stream transform.
                aesAlg.Key = Convert.FromBase64String(Key);

                aesAlg.IV = Convert.FromBase64String(GlobalIv);


                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    result = srDecrypt.ReadToEnd();
                }
            }

            return result;
        }
    }
}