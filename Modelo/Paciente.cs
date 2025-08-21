using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Modelo
{
    public class Paciente : Persona
    {
        public string Genero { get; set; }
        public string Direccion { get; set; }
        public DateTime? Fecha_nacimiento { get; set; }
        public DateTime? Fecha_registro { get; set; }

        public Paciente (int id, string nombre, string cedula, DateTime? fecha_nacimiento, string genero, string direccion, int? telefono, string correo, int? estado, DateTime? fecha_registro)
            : base (id, nombre, cedula, estado, telefono, correo)
        {
            Fecha_nacimiento = fecha_nacimiento;
            Genero = genero;
            Direccion = direccion;
            Fecha_registro = fecha_registro;
        }
    }
}
