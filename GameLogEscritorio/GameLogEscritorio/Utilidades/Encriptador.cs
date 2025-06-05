using System.Security.Cryptography;
using System.Text;

namespace GameLogEscritorio.Utilidades
{
    public static class Encriptador
    {
        public static string hasheoA256(string textoAHashear)
        {
            StringBuilder constructorStringHash = new StringBuilder();
            byte[] inputBytes = Encoding.UTF8.GetBytes(textoAHashear);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                string hexaDecimalFormat = "x2";

                for (int indexBytes = 0; indexBytes < hashBytes.Length; indexBytes++)
                {
                    constructorStringHash.Append(hashBytes[indexBytes].ToString(hexaDecimalFormat));
                }

            }
            return constructorStringHash.ToString();
        }
    }
}
