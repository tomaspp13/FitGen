using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitGen.Dominio.Entidades
{
    public class Rutina
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Objetivo { get; set; }
        public string Contenido {  get; set; }
        public int Edad { get; set; }
        public int DiasDisponibles {  get; set; }
        public int PesoActual {  get; set; }
        public int PesoObjetivo { get; set; }
        public DateTime FechaGeneracion {  get; set; }
    }
}
