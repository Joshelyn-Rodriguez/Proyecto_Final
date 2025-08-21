using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final.Modelo
{
    public abstract class Persona
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public int? Estado { get; set; }
        public int? Telefono { get; set; }
        public string Correo { get; set; }

        public Persona(int id, string nombre , string cedula , int? estado, int? telefono, string correo)
        {
            ID = id;
            Nombre = nombre;
            Cedula = cedula;
            Estado = estado;
            Telefono = telefono;
            Correo = correo;
        }

    }

}

