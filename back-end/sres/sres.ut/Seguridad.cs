using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sres.ut
{
    public static class Seguridad
    {
        public static string hashSal(string pass)
        {
            byte[] salt;
            byte[] buffer;

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(pass, 16, 1000))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }

        public static bool CompararHashSal(string pass, string passBD)
        {
            byte[] buffer;
            byte[] src = Convert.FromBase64String(passBD);
            if ((src.Length != 49) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[16];
            Buffer.BlockCopy(src, 1, dst, 0, 16);
            byte[] buffer2 = new byte[32];
            Buffer.BlockCopy(src, 17, buffer2, 0, 32);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(pass, dst, 1000))
            {
                buffer = bytes.GetBytes(32);
            }
            return buffer2.SequenceEqual(buffer);
        }
    }
}
