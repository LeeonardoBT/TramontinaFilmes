using CSharpFunctionalExtensions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TramontinaFilmes.WebApi.Dominio
{
    public sealed class Filme
    {
        private string _hashConcorrencia;
        private Filme() { }
        public Filme(Guid id, string titulo, int duracao, string sinopse, string hashConcorrencia)
        {
            Id = id;
            Titulo = titulo;
            Duracao = duracao;
            Sinopse = sinopse;
            _hashConcorrencia = hashConcorrencia;
        }

        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public int Duracao { get; set; }
        public string Sinopse { get; set; }

        public static Result<Filme> Criar(string titulo, int duracao, string sinopse)
        {
            if (titulo.Length <= 3)
                return Result.Failure<Filme>("Títilo do filme muito curto");

            var filme = new Filme(Guid.NewGuid(), titulo, duracao, sinopse, "");
            filme.AtualizarHashConcorrencia();
            return filme;
        }

        private void AtualizarHashConcorrencia()
        {
            using var hash = MD5.Create();
            var data = hash.ComputeHash(
                Encoding.UTF8.GetBytes(
                    $"{Id}{Titulo}{Duracao}{Sinopse}"));
            var sBuilder = new StringBuilder();
            foreach (var tbyte in data)
                sBuilder.Append(tbyte.ToString("x2"));
            _hashConcorrencia = sBuilder.ToString();
        }
    }
}
