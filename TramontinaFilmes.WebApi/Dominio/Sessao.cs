using CSharpFunctionalExtensions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TramontinaFilmes.WebApi.Dominio
{
    public sealed class Sessao
    {
        private string _hashConcorrencia;
        private Sessao() { }

        public Sessao(Guid id, Guid filmeId, int dia, int mes, int ano,
                      int horaInicio, int minutoInicio, int quantidadeLugares, float valorIngresso, string hashConcorrencia)
        {
            Id = id;
            FilmeId = filmeId;
            Dia = dia;
            Mes = mes;
            Ano = ano;
            HoraInicio = horaInicio;
            MinutoInicio = minutoInicio;
            QuantidadeLugares = quantidadeLugares;
            QuantidadeDisponivel = quantidadeLugares;
            ValorIngresso = valorIngresso;
            _hashConcorrencia = hashConcorrencia;
        }
        public Guid Id { get; set; }
        public Guid FilmeId { get; set; }

        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }

        public int HoraInicio { get; set; }
        public int MinutoInicio { get; set; }

        public int QuantidadeLugares { get; set; }

        public int QuantidadeDisponivel { get; set; }

        public float ValorIngresso { get; set; }

        public static Result<Sessao> Criar(Guid filmeId, int dia, int mes, int ano,
                                           int horaInicio, int minutoInicio, int quantidadeLugares, float valorIngresso)
        {
            var sessao = new Sessao(Guid.NewGuid(), filmeId, dia, mes, ano,
                                    horaInicio, minutoInicio, quantidadeLugares, valorIngresso, "");
            sessao.AtualizarHashConcorrencia();
            return sessao;
        }

        private void AtualizarHashConcorrencia()
        {
            using var hash = MD5.Create();
            var data = hash.ComputeHash(
                Encoding.UTF8.GetBytes(
                    $"{Id}{FilmeId}{Dia}{Mes}{Ano}{HoraInicio}{MinutoInicio}{QuantidadeLugares}{QuantidadeDisponivel}{ValorIngresso}"));
            var sBuilder = new StringBuilder();
            foreach (var tbyte in data)
                sBuilder.Append(tbyte.ToString("x2"));
            _hashConcorrencia = sBuilder.ToString();
        }
    }
}
