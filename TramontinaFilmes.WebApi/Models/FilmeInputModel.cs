using System;
using System.ComponentModel.DataAnnotations;

namespace TramontinaFilmes.WebApi.Models
{
    public class FilmeInputModel
    {
        [Required]
        [MinLength(2)]
        public string Titulo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe uma duração válida")]
        public int Duracao { get; set; }

        [Required]
        [MinLength(10)]
        public string Sinopse { get; set; }
    }
}
