namespace Proyecto_Final.Modelo
{
    public class Historial
    {
        public int ID { get; set; }
        public int? ID_Paciente { get; set; }
        public int? ID_Medico { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public DateTime? Fecha_consulta { get; set; }

        public Historial(int id, int? id_paciente, int? id_medico, string diagnostico, string tratamiento, DateTime? fecha_consulta)
        {
            ID = id;
            ID_Paciente = id_paciente;
            ID_Medico = id_medico;
            Diagnostico = diagnostico;
            Tratamiento = tratamiento;
            Fecha_consulta = fecha_consulta;
        }
    }
}
