using System;
using System.ComponentModel.DataAnnotations;

namespace TramontinaFilmes.WebApi.Models
{
    public class SessaoInputModel
    {
        [Required]
        public Guid FilmeId { get; set; }

        [Required]
        [Range(1, 31, ErrorMessage = "Por favor, informe um dia válido")]
        public int Dia { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Por favor, informe um mês válido")]
        public int Mes { get; set; }

        [Required]
        [Range(1900, 5000, ErrorMessage = "Por favor, informe um ano válido")]
        public int Ano { get; set; }

        [Required]
        [Range(0, 23, ErrorMessage = "Por favor, informe uma hora válida")]
        public int HoraInicio { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Por favor, informe um minuto válido")]
        public int MinutoInicio { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe uma quantidade válida")]
        public int QuantidadeLugares { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Por favor, informe um preço válido")]
        public float ValorIngresso { get; set; }
    }
}
