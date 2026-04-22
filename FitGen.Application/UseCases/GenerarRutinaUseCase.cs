using FitGen.Dominio.Interfaces;
using FitGen.Aplicacion.DTOs;

namespace FitGen.Aplicacion.UseCases
{
    public class GenerarRutinaUseCase
    {
        private readonly IOpenAIService _openAIService;
        private readonly IEmailService _emailService;

        public GenerarRutinaUseCase(IOpenAIService openAIService, IEmailService emailService)
        {
            _openAIService = openAIService;
            _emailService = emailService;
        }

        public async Task EjercutarAsync(GenerarRutinaRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@") || !request.Email.Contains(".com"))
                throw new ArgumentException("El email es inválido.");

            if (request.Edad < 10 || request.Edad > 100)
                throw new ArgumentException("La edad debe estar entre 10 y 100 años.");

            if (request.PesoActual <= 35)
                throw new ArgumentException("El peso actual es inválido.");

            if (request.PesoObjetivo <= 35)
                throw new ArgumentException("El peso objetivo es inválido.");

            if (request.DiasDisponibles < 1 || request.DiasDisponibles > 7)
                throw new ArgumentException("Los días disponibles deben estar entre 1 y 7.");


            string prompt = $"Generá únicamente una rutina de ejercicios para {request.Nombre} {request.Apellido}, " +
                $"con {request.Edad} años, con un peso actual de {request.PesoActual} Kg. " +
                $"Su objetivo es pesar {request.PesoObjetivo} Kg y lo que quiere es: {request.Objetivo}. " +
                $"Días disponibles por semana: {request.DiasDisponibles}. " +
                $"No incluyas consejos de dieta, ni introducciones, ni conclusiones. " +
                $"Solo listá los ejercicios por día con series y repeticiones.";

            string rutina = await _openAIService.GenerarRutinaAsync(prompt);

            await _emailService.EnviarRutinaAsync(request.Email,rutina,request.Nombre,request.Apellido);

        }
    }
}
