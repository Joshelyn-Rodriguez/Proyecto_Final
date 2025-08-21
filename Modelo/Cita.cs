using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Modelo
{
    public class Cita
    {
        public int ID { get; set; }
        public int? ID_Paciente { get; set; }
        public int? ID_Medico { get; set; }
        public DateTime? Fecha { get; set; }
        public string Hora { get; set; }
        public string Especialidad { get; set; }
        public int? Estado { get; set; }

        public Cita(int id, int? id_paciente, int? id_medico, DateTime? fecha, string hora, string especialidad, int? estado)
        {
            ID = id;
            ID_Paciente = id_paciente;
            ID_Medico = id_medico;
            Fecha = fecha;
            Hora = hora;
            Especialidad = especialidad;
            Estado = estado;
        }
    }
}
