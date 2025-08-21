using System.Data;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Proyecto_Final.Modelo;

namespace Proyecto_Final.Data
{
    public class MedicoQuery
    {
        List<Medico> medicos;
        ConexionBD conexionBD;

        public MedicoQuery()
        {
            medicos = new List<Medico>();
            conexionBD = new ConexionBD();
        }

        public List<Medico> GetMedicos()
        {
            List<Medico> medicos = new List<Medico>();
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("obtenerTodosLosMedicos", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            MySqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                int id = Convert.ToInt32(lector["ID"]);
                string nombre = lector["Nombre"].ToString();
                string cedula = lector["Cedula"].ToString();
                string especialidad = lector["Especialidad"].ToString();
                int telefono = Convert.ToInt32(lector["Telefono"]);
                string correo = lector["Correo"].ToString();
                DateTime horario_consulta = Convert.ToDateTime(lector["Horario_consulta"]);
                int estado = Convert.ToInt32(lector["Estado"]);

                Medico medico = new Medico(id, nombre, cedula, especialidad, telefono, correo, horario_consulta, estado);
                medicos.Add(medico);
            }

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return medicos;
        }

        public void AgregarMedico(Medico medico)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("insertMedico", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("p_Nombre", medico.Nombre);
            comando.Parameters.AddWithValue("p_Cedula", medico.Cedula);
            comando.Parameters.AddWithValue("p_Especialidad", medico.Especialidad);
            comando.Parameters.AddWithValue("p_Telefono", medico.Telefono);
            comando.Parameters.AddWithValue("p_Correo", medico.Correo);
            comando.Parameters.AddWithValue("p_Horario_consulta", medico.Horario_consulta);
            comando.Parameters.AddWithValue("p_Estado", medico.Estado);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }

        public Medico BuscarMedico(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("consultarMedicoPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            MySqlDataReader lector = comando.ExecuteReader();
            lector.Read();

            Medico medico = new Medico(
                Convert.ToInt32(lector["ID"]),
                lector["Nombre"].ToString(),
                lector["Cedula"].ToString(),
                lector["Especialidad"].ToString(),
                Convert.ToInt32(lector["Telefono"]),
                lector["Correo"].ToString(),
                Convert.ToDateTime(lector["Horario_consulta"]),
                Convert.ToInt32(lector["Estado"])
            );

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return medico;
        }

        public void ActualizarMedico(int id, Medico medico)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            try
            {
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = conexion;

                List<string> setClauses = new List<string>();

                if (!string.IsNullOrEmpty(medico.Nombre)) setClauses.Add("Nombre = @Nombre");
                if (!string.IsNullOrEmpty(medico.Cedula)) setClauses.Add("Cedula = @Cedula");
                if (!string.IsNullOrEmpty(medico.Especialidad)) setClauses.Add("Especialidad = @Especialidad");
                if (medico.Telefono.HasValue) setClauses.Add("Telefono = @Telefono");
                if (!string.IsNullOrEmpty(medico.Correo)) setClauses.Add("Correo = @Correo");
                if (medico.Horario_consulta.HasValue) setClauses.Add("Horario_consulta = @Horario_consulta");
                if (medico.Estado.HasValue) setClauses.Add("Estado = @Estado");

                if (setClauses.Count == 0)
                {
                    conexionBD.CerrarConexion(conexion);
                    return; // No hay nada que actualizar
                }

                string query = $"UPDATE Medico SET {string.Join(", ", setClauses)} WHERE ID = @ID";
                comando.CommandText = query;

                comando.Parameters.AddWithValue("@ID", id);
                if (!string.IsNullOrEmpty(medico.Nombre)) comando.Parameters.AddWithValue("@Nombre", medico.Nombre);
                if (!string.IsNullOrEmpty(medico.Cedula)) comando.Parameters.AddWithValue("@Cedula", medico.Cedula);
                if (!string.IsNullOrEmpty(medico.Especialidad)) comando.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                if (medico.Telefono.HasValue) comando.Parameters.AddWithValue("@Telefono", medico.Telefono.Value);
                if (!string.IsNullOrEmpty(medico.Correo)) comando.Parameters.AddWithValue("@Correo", medico.Correo);
                if (medico.Horario_consulta.HasValue) comando.Parameters.AddWithValue("@Horario_consulta", medico.Horario_consulta.Value);
                if (medico.Estado.HasValue) comando.Parameters.AddWithValue("@Estado", medico.Estado.Value);

                comando.ExecuteNonQuery();
            }
            finally
            {
                conexionBD.CerrarConexion(conexion);
            }
        }



        public void InactivarMedico(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("eliminarMedicoPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }
    }
}
