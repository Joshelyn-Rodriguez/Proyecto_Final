using Proyecto_Final.Encriptacion;
using Microsoft.AspNetCore.Hosting.Server;
using MySql.Data.MySqlClient;
using System.Data;

namespace Proyecto_Final.Data
{
    public class ConexionBD
    {
        private const string cadenaConexion = "Server = localhost; Database=vitali;User Id = root; Password=root; Port=3306;";


        public ConexionBD()
        { 
           
        }

        public MySqlConnection AbrirConexion()
        {

            MySqlConnection conexion = new MySqlConnection(cadenaConexion);
            try
            {
                conexion.Open();
                Console.WriteLine("✅ Conexión MySQL abierta correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al abrir la conexión MySQL: " + ex.Message);
            }
            return conexion;
        }

        public void CerrarConexion(MySqlConnection conexion)
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();
                Console.WriteLine("🔒 Conexión MySQL cerrada.");
            }
        }
    }
}
