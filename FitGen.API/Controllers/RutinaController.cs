using FitGen.Aplicacion.DTOs;
using FitGen.Aplicacion.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitGen.API.Controllers
{
    [Route("api/rutinas")]
    [ApiController]
    public class RutinaController : ControllerBase
    {

        private readonly GenerarRutinaUseCase _useCase;

        public RutinaController(GenerarRutinaUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("generar")]

        public async Task<IActionResult> Generar([FromBody] GenerarRutinaRequest request)
        {
            await _useCase.EjercutarAsync(request);
            return Ok(new {mensaje = "Rutina enviada correctamente" });
        }

    }
}
