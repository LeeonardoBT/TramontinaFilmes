using System;
using System.ComponentModel.DataAnnotations;

namespace TramontinaFilmes.WebApi.Models
{
    public class SessaoFilmeInputModel
    {
        public string NomeFilme { get; set; }

        public int Dia { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }
    }
}
