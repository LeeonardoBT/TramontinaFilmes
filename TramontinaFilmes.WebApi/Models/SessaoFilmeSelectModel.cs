using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TramontinaFilmes.WebApi.Models
{
    public class SessaoFilmeSelectModel
    {

        public string NomeFilme { get; set; }

        public int Dia { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }

        public int HoraInicio { get; set; }

        public int MinutoInicio { get; set; }

        public int QuantidadeDisponivel { get; set; }

        public float ValorIngresso { get; set; }

        public string Hora { get; set; }
    }
}
