using Proyecto_Final.Data;
using Proyecto_Final.Modelo;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialController : ControllerBase
    {
        private HistorialQuery hisQuery;

        public HistorialController()
        {
            hisQuery = new HistorialQuery();
        }

        // GET: api/<HistorialController>
        [HttpGet]
        public List<Historial> Get()
        {
            return hisQuery.GetHistoriales();
        }

        // GET api/<HistorialController>/5
        [HttpGet("{id}")]
        public Historial Get(int id)
        {
            return hisQuery.BuscarHistorial(id);
        }

        // POST api/<HistorialController>
        [HttpPost]
        public void Post([FromBody] Historial historial)
        {
            hisQuery.AgregarHistorial(historial);
        }

        // PUT api/<HistorialController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Historial historial)
        {
            hisQuery.ActualizarHistorial(id, historial);
        }

        // DELETE api/<HistorialController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            hisQuery.EliminarHistorial(id);
        }
    }
}
