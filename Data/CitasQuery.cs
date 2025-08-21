using System.Data;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Proyecto_Final.Modelo;

namespace Proyecto_Final.Data
{
    public class CitasQuery
    {
        List<Cita> citas;
        ConexionBD conexionBD;

        public CitasQuery()
        {
            citas = new List<Cita>();
            conexionBD = new ConexionBD();
        }

        public List<Cita> GetCitas()
        {
            List<Cita> citas = new List<Cita>();
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("obtenerTodasLasCitas", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            MySqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                int id = Convert.ToInt32(lector["ID"]);
                int id_paciente = Convert.ToInt32(lector["ID_Paciente"]);
                int id_medico = Convert.ToInt32(lector["ID_Medico"]);
                DateTime fecha = Convert.ToDateTime(lector["Fecha"]);
                string hora = lector["Hora"].ToString();
                string especialidad = lector["Especialidad"].ToString();
                int estado = Convert.ToInt32(lector["Estado"]);

                Cita cita = new Cita(id, id_paciente, id_medico, fecha, hora, especialidad, estado);
                citas.Add(cita);
            }

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return citas;
        }

        public void AgregarCita(Cita cita)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("insertCita", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("p_ID_Paciente", cita.ID_Paciente);
            comando.Parameters.AddWithValue("p_ID_Medico", cita.ID_Medico);
            comando.Parameters.AddWithValue("p_Fecha", cita.Fecha);
            comando.Parameters.AddWithValue("p_Hora", cita.Hora);
            comando.Parameters.AddWithValue("p_Especialidad", cita.Especialidad);
            comando.Parameters.AddWithValue("p_Estado", cita.Estado);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }

        public Cita BuscarCita(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("consultarCitaPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            MySqlDataReader lector = comando.ExecuteReader();

            lector.Read();

            Cita cita = new Cita(
                Convert.ToInt32(lector["ID"]),
                Convert.ToInt32(lector["ID_Paciente"]),
                Convert.ToInt32(lector["ID_Medico"]),
                Convert.ToDateTime(lector["Fecha"]),
                lector["Hora"].ToString(),
                lector["Especialidad"].ToString(),
                Convert.ToInt32(lector["Estado"])
            );

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return cita;
        }

        public void ActualizarCita(int id, Cita cita)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            try
            {
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = conexion;

                List<string> setClauses = new List<string>();

                if (cita.ID_Paciente.HasValue) setClauses.Add("ID_Paciente = @ID_Paciente");
                if (cita.ID_Medico.HasValue) setClauses.Add("ID_Medico = @ID_Medico");
                if (cita.Fecha.HasValue) setClauses.Add("Fecha = @Fecha");
                if (!string.IsNullOrEmpty(cita.Hora)) setClauses.Add("Hora = @Hora"); // hora como string
                if (!string.IsNullOrEmpty(cita.Especialidad)) setClauses.Add("Especialidad = @Especialidad");
                if (cita.Estado.HasValue) setClauses.Add("Estado = @Estado");

                if (setClauses.Count == 0)
                {
                    conexionBD.CerrarConexion(conexion);
                    return; // No hay nada que actualizar
                }

                string query = $"UPDATE Cita SET {string.Join(", ", setClauses)} WHERE ID = @ID";
                comando.CommandText = query;

                comando.Parameters.AddWithValue("@ID", id);
                if (cita.ID_Paciente.HasValue) comando.Parameters.AddWithValue("@ID_Paciente", cita.ID_Paciente.Value);
                if (cita.ID_Medico.HasValue) comando.Parameters.AddWithValue("@ID_Medico", cita.ID_Medico.Value);
                if (cita.Fecha.HasValue) comando.Parameters.AddWithValue("@Fecha", cita.Fecha.Value);
                if (!string.IsNullOrEmpty(cita.Hora)) comando.Parameters.AddWithValue("@Hora", cita.Hora); // hora como string
                if (!string.IsNullOrEmpty(cita.Especialidad)) comando.Parameters.AddWithValue("@Especialidad", cita.Especialidad);
                if (cita.Estado.HasValue) comando.Parameters.AddWithValue("@Estado", cita.Estado.Value);

                comando.ExecuteNonQuery();
            }
            finally
            {
                conexionBD.CerrarConexion(conexion);
            }
        }



        public void InactivarCita(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("eliminarCitaPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }
    }
}
