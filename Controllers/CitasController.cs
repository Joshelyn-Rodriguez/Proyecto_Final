

using Proyecto_Final.Data;
using Proyecto_Final.Modelo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {

        CitasQuery citQuery;

        public CitasController()
        {
            citQuery = new CitasQuery();
        }


        // GET: api/<CitasController>
        [HttpGet]
        public List<Cita> Get()
        {
            List<Cita> citas = citQuery.GetCitas();
            return citas;
        }


        // GET api/<CitasController>/5

        [HttpGet("{id}")]
        public Cita Get(int id)
        {
            Cita cita = citQuery.BuscarCita(id);
            return cita;
        }


        // POST api/<CitasController>
        [HttpPost]
        public void Post([FromBody] Cita cita)
        {
            citQuery.AgregarCita(cita);
        }

        // PUT api/<CitasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Cita cita)
        {
            citQuery.ActualizarCita(id, cita);
        }

        // DELETE api/<CitasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            citQuery.InactivarCita(id);
        }






    }
}
