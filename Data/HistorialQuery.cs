using Proyecto_Final.Modelo;
using MySql.Data.MySqlClient;
using System.Data;

namespace Proyecto_Final.Data
{
    public class HistorialQuery
    {
        List<Historial> historiales;
        ConexionBD conexionBD;

        public HistorialQuery()
        {
            historiales = new List<Historial>();
            conexionBD = new ConexionBD();
        }

        public List<Historial> GetHistoriales()
        {
            List<Historial> historiales = new List<Historial>();
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("obtenerTodosLosHistoriales", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            MySqlDataReader lector = comando.ExecuteReader();

            while (lector.Read())
            {
                int id = Convert.ToInt32(lector["ID"]);
                int id_paciente = Convert.ToInt32(lector["ID_Paciente"]);
                int id_medico = Convert.ToInt32(lector["ID_Medico"]);
                string diagnostico = lector["Diagnostico"].ToString();
                string tratamiento = lector["Tratamiento"].ToString();
                DateTime fecha_consulta = Convert.ToDateTime(lector["Fecha_consulta"]);

                Historial historial = new Historial(id, id_paciente, id_medico, diagnostico, tratamiento, fecha_consulta);
                historiales.Add(historial);
            }

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return historiales;
        }

        public void AgregarHistorial(Historial historial)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("insertHistorial", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("p_ID_Paciente", historial.ID_Paciente);
            comando.Parameters.AddWithValue("p_ID_Medico", historial.ID_Medico);
            comando.Parameters.AddWithValue("p_Diagnostico", historial.Diagnostico);
            comando.Parameters.AddWithValue("p_Tratamiento", historial.Tratamiento);
            comando.Parameters.AddWithValue("p_Fecha_consulta", historial.Fecha_consulta);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }

        public Historial BuscarHistorial(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("consultarHistorialPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            MySqlDataReader lector = comando.ExecuteReader();
            lector.Read();

            Historial historial = new Historial(
                Convert.ToInt32(lector["ID"]),
                Convert.ToInt32(lector["ID_Paciente"]),
                Convert.ToInt32(lector["ID_Medico"]),
                lector["Diagnostico"].ToString(),
                lector["Tratamiento"].ToString(),
                Convert.ToDateTime(lector["Fecha_consulta"])
            );

            lector.Close();
            conexionBD.CerrarConexion(conexion);
            return historial;
        }

        public void ActualizarHistorial(int id, Historial historial)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            try
            {
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = conexion;

                List<string> setClauses = new List<string>();

                if (historial.ID_Paciente.HasValue) setClauses.Add("ID_Paciente = @ID_Paciente");
                if (historial.ID_Medico.HasValue) setClauses.Add("ID_Medico = @ID_Medico");
                if (!string.IsNullOrEmpty(historial.Diagnostico)) setClauses.Add("Diagnostico = @Diagnostico");
                if (!string.IsNullOrEmpty(historial.Tratamiento)) setClauses.Add("Tratamiento = @Tratamiento");
                if (historial.Fecha_consulta.HasValue) setClauses.Add("Fecha_consulta = @Fecha_consulta");

                if (setClauses.Count == 0)
                {
                    conexionBD.CerrarConexion(conexion);
                    return;
                }

                string query = $"UPDATE Historial SET {string.Join(", ", setClauses)} WHERE ID = @ID";
                comando.CommandText = query;

                comando.Parameters.AddWithValue("@ID", id);
                if (historial.ID_Paciente.HasValue) comando.Parameters.AddWithValue("@ID_Paciente", historial.ID_Paciente.Value);
                if (historial.ID_Medico.HasValue) comando.Parameters.AddWithValue("@ID_Medico", historial.ID_Medico.Value);
                if (!string.IsNullOrEmpty(historial.Diagnostico)) comando.Parameters.AddWithValue("@Diagnostico", historial.Diagnostico);
                if (!string.IsNullOrEmpty(historial.Tratamiento)) comando.Parameters.AddWithValue("@Tratamiento", historial.Tratamiento);
                if (historial.Fecha_consulta.HasValue) comando.Parameters.AddWithValue("@Fecha_consulta", historial.Fecha_consulta.Value);

                comando.ExecuteNonQuery();
            }
            finally
            {
                conexionBD.CerrarConexion(conexion);
            }
        }


        public void EliminarHistorial(int id)
        {
            MySqlConnection conexion = conexionBD.AbrirConexion();

            MySqlCommand comando = new MySqlCommand("eliminarHistorialPorID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("p_id", id);

            comando.ExecuteNonQuery();
            conexionBD.CerrarConexion(conexion);
        }
    }
}
