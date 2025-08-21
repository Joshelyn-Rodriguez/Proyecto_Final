
using Proyecto_Final.Autenticación;
using Proyecto_Final.Data;
using Proyecto_Final.Modelo;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto_Final.Controllers
{

    [BasicAuthentication]
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {

        PacienteQuery pacQuery;

        public PacienteController( )
        {
            pacQuery = new PacienteQuery();
        }


        // GET: api/<PacienteController>
        [HttpGet]
        public List<Paciente> Get()
        {
            List<Paciente> pacientes = pacQuery.GetPacientes();
            return pacientes;
        }


        // GET api/<PacienteController>/5

        [HttpGet("{id}")]
        public Paciente Get(int id)
        {
            Paciente paciente = pacQuery.BuscarPaciente(id);
            return paciente;
        }


        // POST api/<PacienteController>
        [HttpPost]
        public void Post([FromBody] Paciente paciente)
        {
            pacQuery.AgregarPaciente(paciente);
        }

        // PUT api/<PacienteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Paciente paciente)
        {
            pacQuery.ActualizarPaciente(id, paciente);
        }

        // DELETE api/<PacienteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            pacQuery.InactivarPaciente(id);
        }
    }
}
