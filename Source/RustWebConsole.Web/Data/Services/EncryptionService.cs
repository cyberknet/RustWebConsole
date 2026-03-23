using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace RustWebConsole.Web.Data.Services
{

    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtector _dataProtector;

        public EncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("RustWebConsole.EncryptionService");
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            return _dataProtector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                return _dataProtector.Unprotect(cipherText);
            }
            catch
            {
                throw new InvalidOperationException("Failed to decrypt the provided text.");
            }
        }
    }
}