using Xunit;
using Moq;
using FitGen.Aplicacion.DTOs;
using FitGen.Aplicacion.UseCases;
using FitGen.Dominio.Interfaces;

namespace FitGen.Tests
{
    public class GenerarRutinaUseCaseTests
    {
        [Fact]
        public async Task EjecutarAsync_ConDatosValidos_DebeGenerarYEnviarRutina()
        {
            var mockOpenAI = new Mock<IOpenAIService>();
            var mockEmail = new Mock<IEmailService>();

            mockOpenAI
                .Setup(s => s.GenerarRutinaAsync(It.IsAny<string>()))
                .ReturnsAsync("Día 1: Sentadillas 3x10\nDía 2: Pecho 3x8");

            var useCase = new GenerarRutinaUseCase(mockOpenAI.Object, mockEmail.Object);

            var request = new GenerarRutinaRequest
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "juan@gmail.com",
                Edad = 25,
                Objetivo = "Ganar músculo",
                DiasDisponibles = 4,
                PesoActual = 80,
                PesoObjetivo = 85
            };

            await useCase.EjercutarAsync(request);

            mockOpenAI.Verify(
                s => s.GenerarRutinaAsync(It.IsAny<string>()),
                Times.Once
            );

            mockEmail.Verify(
                s => s.EnviarRutinaAsync(request.Email, It.IsAny<string>(), request.Nombre, request.Apellido),
                Times.Once
            );
        }

        [Fact]
        public async Task EjecutarAsync_ConEmailInvalido_DebeThrowArgumentException()
        {

            var mockEmail = new Mock<IEmailService>();
            var mockOpenAI = new Mock<IOpenAIService>();

            var useCase = new GenerarRutinaUseCase(mockOpenAI.Object, mockEmail.Object);

            var request = new GenerarRutinaRequest
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "emailinvalido",
                Edad = 25,
                Objetivo = "Ganar músculo",
                DiasDisponibles = 4,
                PesoActual = 80,
                PesoObjetivo = 85
            };

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.EjercutarAsync(request));
        }

        [Fact]
        public async Task EjecutarAsync_SiGroqFalla_DebeThrowException()
        {

            var mockEmail = new Mock<IEmailService>();
            var mockOpenAI = new Mock<IOpenAIService>();

            var useCase = new GenerarRutinaUseCase(mockOpenAI.Object, mockEmail.Object);

            mockOpenAI
            .Setup(s => s.GenerarRutinaAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Groq no respondió"));

            var request = new GenerarRutinaRequest
            {
                Nombre = "Juan",
                Apellido = "Pérez",
                Email = "juan@gmail.com",
                Edad = 25,
                Objetivo = "Ganar músculo",
                DiasDisponibles = 4,
                PesoActual = 80,
                PesoObjetivo = 85
            };

            await Assert.ThrowsAsync<Exception>(() => useCase.EjercutarAsync(request));
        }
    }
}