using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SliptMath.Models
{
    public class Jugador
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public int PuntuacionMax { get; set; }
        public int CantidadCalculadoras { get; set; }
        public bool SaltarTutorial { get; set; }
        public int MinutosJugar { get; set; }
        public int PuntuacionActual { get; set; }
    }
}
