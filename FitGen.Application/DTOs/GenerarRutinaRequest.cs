using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitGen.Aplicacion.DTOs
{
    public class GenerarRutinaRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Edad { get; set; }
        public string Objetivo { get; set; }
        public int DiasDisponibles {  get; set; }
        public int PesoActual { get; set; }
        public int PesoObjetivo { get; set; }
    }
}
