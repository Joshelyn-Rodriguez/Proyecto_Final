

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
    public class MedicoController : ControllerBase
    {

        MedicoQuery medQuery;

        public MedicoController()
        {
            medQuery = new MedicoQuery();
        }


        // GET: api/<MedicoController>
        [HttpGet]
        public List<Medico> Get()
        {
            List<Medico> medicos = medQuery.GetMedicos();
            return medicos;
        }


        // GET api/<MedicoController>/5

        [HttpGet("{id}")]
        public Medico Get(int id)
        {
            Medico medico = medQuery.BuscarMedico(id);
            return medico;
        }


        // POST api/<MedicoController>
        [HttpPost]
        public void Post([FromBody] Medico medico)
        {
            medQuery.AgregarMedico(medico);
        }

        // PUT api/<MedicoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Medico medico)
        {
            medQuery.ActualizarMedico(id, medico);
        }

        // DELETE api/<MedicoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            medQuery.InactivarMedico(id);
        }
    }
}
