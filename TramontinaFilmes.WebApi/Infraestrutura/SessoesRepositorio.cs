using TramontinaFilmes.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using TramontinaFilmes.WebApi.Models;

namespace TramontinaFilmes.WebApi.Infraestrutura
{
    public sealed class SessoesRepositorio
    {
        private readonly CinemaDbContext _dbContext;
        private readonly FilmesRepositorio _filmesRepositorio;

        public SessoesRepositorio(CinemaDbContext dbContext, FilmesRepositorio filmesRepositorio)
        {
            _dbContext = dbContext;
            _filmesRepositorio = filmesRepositorio;
        }

        public async Task InserirAsync(Sessao novaSessao, CancellationToken cancellationToken = default)
        {
            await _dbContext.Sessoes.AddAsync(novaSessao, cancellationToken);
        }

        public void Alterar(Sessao sessao)
        {
            // Nada a fazer EF CORE fazer o Tracking da Entidade quando recuperamos a mesma
        }

        public async Task<Sessao> RecuperarPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext
                            .Sessoes
                            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task<IEnumerable<SessaoFilmeReturnModel>> RecuperarSecoesFilme(SessaoFilmeInputModel sessaoFilmeInputModel, CancellationToken cancellationToken = default)
        {
            string sequenceMaxQuery = "SELECT sessoes.* " +
                                      "  FROM TramontinaFilmes.dbo.Sessoes sessoes, " +
                                      "       TramontinaFilmes.dbo.Filmes filmes " +
                                      " WHERE sessoes.FilmeId = filmes.Id " +
                                      (sessaoFilmeInputModel.NomeFilme.Length == 0 ? " " : " AND filmes.Titulo LIKE '%" + sessaoFilmeInputModel.NomeFilme + "%'") +
                                      (sessaoFilmeInputModel.Dia == 0 ? " " : " AND sessoes.Dia = " + sessaoFilmeInputModel.Dia) +
                                      (sessaoFilmeInputModel.Mes == 0 ? " " : " AND sessoes.Mes = " + sessaoFilmeInputModel.Mes) +
                                      (sessaoFilmeInputModel.Ano == 0 ? " " : " AND sessoes.Ano = " + sessaoFilmeInputModel.Ano);

            List<Sessao> sessoes = await _dbContext.Sessoes.FromSqlRaw(sequenceMaxQuery).ToListAsync();
            List<SessaoFilmeReturnModel> sessoesReturn = new List<SessaoFilmeReturnModel>();

            sessoesReturn.Clear();

            for (int i = 0; i < sessoes.Count; i++)
            {
                SessaoFilmeReturnModel returnValue = new SessaoFilmeReturnModel();

                Filme filme = await _filmesRepositorio.RecuperarPorIdAsync(sessoes[i].FilmeId);

                returnValue.NomeFilme = filme.Titulo;
                returnValue.Data = sessoes[i].Dia + "/" + sessoes[i].Mes + "/" + sessoes[i].Ano;
                returnValue.Hora = sessoes[i].HoraInicio + ":" + sessoes[i].MinutoInicio;
                returnValue.QuantidadeDisponivel = sessoes[i].QuantidadeDisponivel;
                returnValue.ValorIngresso = sessoes[i].ValorIngresso;
                sessoesReturn.Add(returnValue);
            }

            return sessoesReturn;
        }

        public async Task<IEnumerable<Sessao>> RecuperarTodosAsync()
        {
            return await _dbContext
                            .Sessoes
                            .AsQueryable().ToListAsync();
        }
    }
}
