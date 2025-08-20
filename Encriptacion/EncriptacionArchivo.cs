using System.Security.Cryptography;
using System.Text;

namespace Proyecto_Final.Encriptacion
{
    public class EncriptacionArchivo
    {

        // Clave segura de 32 bytes 
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

        public static void EncryptFileInit(string fileIn, string fileOut)
        {
            byte[] ivBytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(ivBytes); // Generamos un IV aleatorio
            }

            byte[] fileBytes = File.ReadAllBytes(fileIn); // Leer contenido del archivo

            using (Aes aes = Aes.Create()) //AES-256 (Advanced Encryption Standard con una clave de 256 bits) es un algoritmo de cifrado simétrico
            {
                aes.Key = Key;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC; // Cada bloque se cifra combinándolo con el bloque anterior. Requiere un IV (vector de inicialización) para evitar que los mismos datos generen la misma salida.
                aes.Padding = PaddingMode.PKCS7; // El cifrado AES trabaja con bloques de 16 bytes, por lo que si los datos no ocupan exactamente ese tamaño, se debe agregar un relleno para completar el bloque.

                using (MemoryStream ms = new MemoryStream()) // En lugar de escribir o leer desde un archivo en disco, MemoryStream almacena los datos en RAM y permite procesarlos como si fuera un archivo. Esto evita operaciones de disco, que pueden ser más lentas.
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        ms.Write(ivBytes, 0, ivBytes.Length); // Guarda el IV al inicio del archivo
                        cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    File.WriteAllBytes(fileOut, ms.ToArray()); // Sobrescribe el archivo con los datos encriptados en caso de que aplique
                }
            }
        }
        public static void EncryptFile(string file)
        {
            byte[] ivBytes = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(ivBytes); // Generamos un IV aleatorio
            }

            byte[] fileBytes = File.ReadAllBytes(file); // Leer contenido del archivo

            using (Aes aes = Aes.Create()) //AES-256 (Advanced Encryption Standard con una clave de 256 bits) es un algoritmo de cifrado simétrico
            {
                aes.Key = Key;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC; // Cada bloque se cifra combinándolo con el bloque anterior. Requiere un IV (vector de inicialización) para evitar que los mismos datos generen la misma salida.
                aes.Padding = PaddingMode.PKCS7; // El cifrado AES trabaja con bloques de 16 bytes, por lo que si los datos no ocupan exactamente ese tamaño, se debe agregar un relleno para completar el bloque.

                using (MemoryStream ms = new MemoryStream()) // En lugar de escribir o leer desde un archivo en disco, MemoryStream almacena los datos en RAM y permite procesarlos como si fuera un archivo. Esto evita operaciones de disco, que pueden ser más lentas.
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        ms.Write(ivBytes, 0, ivBytes.Length); // Guarda el IV al inicio del archivo
                        cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    File.WriteAllBytes(file, ms.ToArray()); // Sobrescribe el archivo con los datos encriptados en caso de que aplique
                }
            }
        }

        public static void DecryptFile(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath); // Leer contenido encriptado

            byte[] ivBytes = new byte[16];
            Array.Copy(fileBytes, ivBytes, ivBytes.Length); // Extraer IV del inicio del archivo
            byte[] encryptedData = new byte[fileBytes.Length - ivBytes.Length];
            Array.Copy(fileBytes, ivBytes.Length, encryptedData, 0, encryptedData.Length);

            using (Aes aes = Aes.Create()) //AES-256 (Advanced Encryption Standard con una clave de 256 bits) es un algoritmo de cifrado simétrico
            {
                aes.Key = Key;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC; // Cada bloque se cifra combinándolo con el bloque anterior. Requiere un IV (vector de inicialización) para evitar que los mismos datos generen la misma salida.
                aes.Padding = PaddingMode.PKCS7;// El cifrado AES trabaja con bloques de 16 bytes, por lo que si los datos no ocupan exactamente ese tamaño, se debe agregar un relleno para completar el bloque.

                using (MemoryStream ms = new MemoryStream()) // En lugar de escribir o leer desde un archivo en disco, MemoryStream almacena los datos en RAM y permite procesarlos como si fuera un archivo. Esto evita operaciones de disco, que pueden ser más lentas.
                using (CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)) //CryptoStream es el objeto encriptador
                {
                    cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, ms.ToArray()); // Sobrescribe el archivo con los datos desencriptados
                }
            }
        }





    }
}
