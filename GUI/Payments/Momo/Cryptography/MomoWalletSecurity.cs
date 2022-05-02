using GUI.Payments.Momo.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GUI.Payments.Momo.Cryptography
{
    public class MomoWalletSecurity
    {
        private readonly byte[] _keyBytes;

        public MomoWalletSecurity(string secretKey)
        {
            _keyBytes = Encoding.UTF8.GetBytes(secretKey);
        }

        public void SignRequest(MomoWalletCaptureRequest request)
        {
            var rawMessage = request.GetSecurityMessage();
            byte[] messageBytes = Encoding.UTF8.GetBytes(rawMessage);
            using var hmacsha256 = new HMACSHA256(_keyBytes);
            byte[] hashedBytes = hmacsha256.ComputeHash(messageBytes);
            request.Signature = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
