using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitGen.Dominio.Interfaces
{
    public interface IEmailService
    {
        Task EnviarRutinaAsync(string email, string contenidoRutina,string nombre,string apellido);
    }
}
