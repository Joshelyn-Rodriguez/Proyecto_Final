using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Modelo
{
    public class Medico : Persona
    {
        public string Especialidad { get; set; }
        public DateTime? Horario_consulta { get; set; }

        public Medico(int id, string nombre, string cedula, string especialidad, int? telefono, string correo, DateTime? horario_consulta, int? estado)
            : base (id, nombre, cedula, estado, telefono, correo)
        {
            Especialidad = especialidad;
            Horario_consulta = horario_consulta;
        }

    }
}
