using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace GameLogEscritorio.Utilidades
{
    public static class SesionToken
    {

        private static readonly string rutaToken = "token.dat";

        public static void GuardarToken(string token)
        {
            byte[] data = Encoding.UTF8.GetBytes(token);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(rutaToken, encrypted);
        }

        public static string LeerToken()
        {
            if (!File.Exists(rutaToken)) return null!;
            byte[] encrypted = File.ReadAllBytes(rutaToken);
            byte[] decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }

        public static void CerrarSesion()
        {
            if (File.Exists(rutaToken))
            {
                File.Delete(rutaToken);
            }
        }

    }
}
