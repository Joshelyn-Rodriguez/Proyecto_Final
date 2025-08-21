using Proyecto_Final.Modelo;
using MySql.Data.MySqlClient;
using System.Data;

namespace Proyecto_Final.Data
{
    public class PacienteQuery
    {
        List<Paciente> pacientes;
        ConexionBD conexionBD;

        public PacienteQuery()
        {
            pacientes = new List<Paciente>();
            conexionBD = new ConexionBD();
        }

        // Helper para valores nulos
        private T GetValueOrDefault<T>(object value, T defaultValue)
        {
            return value != DBNull.Value ? (T)Convert.ChangeType(value, typeof(T)) : defaultValue;
        }

        public List<Paciente> GetPacientes()
        {
            List<Paciente> pacientes = new List<Paciente>();
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("obtenerTodosLosPacientes", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            MySqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                int id = GetValueOrDefault<int>(lector["ID"], 0);
                string nombre = GetValueOrDefault<string>(lector["Nombre"], "");
                string cedula = GetValueOrDefault<string>(lector["Cedula"], "");
                DateTime fecha_nacimiento = GetValueOrDefault<DateTime>(lector["Fecha_nacimiento"], DateTime.MinValue);
                string genero = GetValueOrDefault<string>(lector["Genero"], "");
                string direccion = GetValueOrDefault<string>(lector["Direccion"], "");
                int telefono = GetValueOrDefault<int>(lector["Telefono"], 0);
                string correo = GetValueOrDefault<string>(lector["Correo"], "");
                int estado = GetValueOrDefault<int>(lector["Estado"], 0);
                DateTime fecha_registro = GetValueOrDefault<DateTime>(lector["Fecha_registro"], DateTime.MinValue);

                Paciente paciente = new Paciente(id, nombre, cedula, fecha_nacimiento, genero, direccion, telefono, correo, estado, fecha_registro);
                pacientes.Add(paciente);
            }

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return pacientes;
        }

        public void AgregarPaciente(Paciente paciente)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("insertPaciente", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("p_Nombre", paciente.Nombre);
            comando.Parameters.AddWithValue("p_Cedula", paciente.Cedula);
            comando.Parameters.AddWithValue("p_Fecha_nacimiento", paciente.Fecha_nacimiento);
            comando.Parameters.AddWithValue("p_Genero", paciente.Genero);
            comando.Parameters.AddWithValue("p_Direccion", paciente.Direccion);
            comando.Parameters.AddWithValue("p_Telefono", paciente.Telefono);
            comando.Parameters.AddWithValue("p_Correo", paciente.Correo);
            comando.Parameters.AddWithValue("p_Estado", paciente.Estado);
            comando.Parameters.AddWithValue("p_Fecha_registro", paciente.Fecha_registro);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }

        public Paciente BuscarPaciente(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("consultarPacientePorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            MySqlDataReader lector = comando.ExecuteReader();

            if (!lector.Read()) // Manejar si no hay resultados
            {
                lector.Close();
                conexionBD.CerrarConexion(conexion);
                return null;
            }

            Paciente paciente = new Paciente(
                GetValueOrDefault<int>(lector["ID"], 0),
                GetValueOrDefault<string>(lector["Nombre"], ""),
                GetValueOrDefault<string>(lector["Cedula"], ""),
                GetValueOrDefault<DateTime>(lector["Fecha_nacimiento"], DateTime.MinValue),
                GetValueOrDefault<string>(lector["Genero"], ""),
                GetValueOrDefault<string>(lector["Direccion"], ""),
                GetValueOrDefault<int>(lector["Telefono"], 0),
                GetValueOrDefault<string>(lector["Correo"], ""),
                GetValueOrDefault<int>(lector["Estado"], 0),
                GetValueOrDefault<DateTime>(lector["Fecha_registro"], DateTime.MinValue)
            );

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return paciente;
        }

        public void ActualizarPaciente(int id, Paciente paciente)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            try
            {
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = conexion;

                List<string> setClauses = new List<string>();

                if (!string.IsNullOrEmpty(paciente.Nombre)) setClauses.Add("Nombre = @Nombre");
                if (!string.IsNullOrEmpty(paciente.Cedula)) setClauses.Add("Cedula = @Cedula");
                if (paciente.Fecha_nacimiento.HasValue) setClauses.Add("Fecha_nacimiento = @Fecha_nacimiento");
                if (!string.IsNullOrEmpty(paciente.Genero)) setClauses.Add("Genero = @Genero");
                if (!string.IsNullOrEmpty(paciente.Direccion)) setClauses.Add("Direccion = @Direccion");
                if (paciente.Telefono != 0) setClauses.Add("Telefono = @Telefono");
                if (!string.IsNullOrEmpty(paciente.Correo)) setClauses.Add("Correo = @Correo");
                if (paciente.Estado != 0) setClauses.Add("Estado = @Estado");
                if (paciente.Fecha_registro.HasValue) setClauses.Add("Fecha_registro = @Fecha_registro");

                if (setClauses.Count == 0)
                {
                    conexionBD.CerrarConexion(conexion);
                    return; // No hay nada que actualizar
                }

                string query = $"UPDATE Paciente SET {string.Join(", ", setClauses)} WHERE ID = @ID";
                comando.CommandText = query;

                comando.Parameters.AddWithValue("@ID", id);
                if (!string.IsNullOrEmpty(paciente.Nombre)) comando.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                if (!string.IsNullOrEmpty(paciente.Cedula)) comando.Parameters.AddWithValue("@Cedula", paciente.Cedula);
                if (paciente.Fecha_nacimiento.HasValue) comando.Parameters.AddWithValue("@Fecha_nacimiento", paciente.Fecha_nacimiento.Value);
                if (!string.IsNullOrEmpty(paciente.Genero)) comando.Parameters.AddWithValue("@Genero", paciente.Genero);
                if (!string.IsNullOrEmpty(paciente.Direccion)) comando.Parameters.AddWithValue("@Direccion", paciente.Direccion);
                if (paciente.Telefono != 0) comando.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                if (!string.IsNullOrEmpty(paciente.Correo)) comando.Parameters.AddWithValue("@Correo", paciente.Correo);
                if (paciente.Estado != 0) comando.Parameters.AddWithValue("@Estado", paciente.Estado);
                if (paciente.Fecha_registro.HasValue) comando.Parameters.AddWithValue("@Fecha_registro", paciente.Fecha_registro.Value);

                comando.ExecuteNonQuery();
            }
            finally
            {
                conexionBD.CerrarConexion(conexion);
            }
        }



        public void InactivarPaciente(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("eliminarPacientePorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }
    }
}
