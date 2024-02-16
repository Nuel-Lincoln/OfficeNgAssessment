using System.Security.Cryptography;
using System.Text;

namespace TechnicalAssesment.Services.Encryption
{
    public class EncryptionHelper
    {
        private readonly IConfiguration _config;
        public EncryptionHelper(IConfiguration config)
        {
            _config = config;
        }
        private const int KeySize = 256;
        private const int BlockSize = 128;
        

        public  string Encrypt(string plainText)
        {
            string Ignore = _config.GetSection("Ignore").Value;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Ignore);
                aesAlg.BlockSize = BlockSize;
                aesAlg.Mode = CipherMode.CFB;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                }
            }
        }

        public  string Decrypt(string cipherText)
        {
            string Ignore = _config.GetSection("Ignore").Value;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Ignore);
                aesAlg.BlockSize = BlockSize;
                aesAlg.Mode = CipherMode.CFB;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] iv = cipherBytes.Take(BlockSize / 8).ToArray();
                byte[] encryptedData = cipherBytes.Skip(BlockSize / 8).ToArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
