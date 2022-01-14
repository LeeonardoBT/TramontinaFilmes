using TramontinaFilmes.WebApi.Dominio;
using TramontinaFilmes.WebApi.Infraestrutura;
using TramontinaFilmes.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TramontinaFilmes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessaoController : ControllerBase
    {
        private readonly ILogger<SessaoController> _logger;
        private readonly SessoesRepositorio _sessoesRepositorio;
        private readonly FilmesRepositorio _filmesRepositorio;

        public SessaoController(ILogger<SessaoController> logger, SessoesRepositorio sessoesRepositorio, FilmesRepositorio filmesRepositorio)
        {
            _logger = logger;
            _sessoesRepositorio = sessoesRepositorio;
            _filmesRepositorio = filmesRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync([FromBody] SessaoInputModel sessaoInputModel, CancellationToken cancellationToken)
        {
            var sessao = Sessao.Criar(sessaoInputModel.FilmeId, sessaoInputModel.Dia, sessaoInputModel.Mes, sessaoInputModel.Ano,
                                      sessaoInputModel.HoraInicio, sessaoInputModel.MinutoInicio, sessaoInputModel.QuantidadeLugares, sessaoInputModel.ValorIngresso);
            if (sessao.IsFailure)
            {
                _logger.LogError("Erro ao criar sessão");
                return BadRequest(sessao.Error);
            }

            var filme = await _filmesRepositorio.RecuperarPorIdAsync(sessaoInputModel.FilmeId, cancellationToken);
            if (filme == null)
            {
                string error = "Erro ao criar sessão - Filme não cadastrado";
                _logger.LogError(error);
                return BadRequest(error);
            }

            try
            {
                DateTime data = new DateTime(sessaoInputModel.Ano, sessaoInputModel.Mes, sessaoInputModel.Dia);
            }
            catch
            {
                string error = "Data inválida";
                _logger.LogError(error);
                return BadRequest(error);
            }

            await _sessoesRepositorio.InserirAsync(sessao.Value, cancellationToken);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Sessão {sessao} criada", sessao.Value.Id);

            return CreatedAtAction("RecuperarPorId", new { id = sessao.Value.Id }, sessao.Value.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] SessaoInputModel sessaoInputModel, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                string error = "Id inválido"; 
                _logger.LogError(error);
                return BadRequest("Id inválido");
            }
                
            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(guid, cancellationToken);
            if (sessao == null)
                return NotFound();

            sessao.FilmeId = sessaoInputModel.FilmeId;
            sessao.Dia = sessaoInputModel.Dia;
            sessao.Mes = sessaoInputModel.Mes;
            sessao.Ano = sessaoInputModel.Ano;
            sessao.HoraInicio = sessaoInputModel.HoraInicio;
            sessao.MinutoInicio = sessaoInputModel.MinutoInicio;
            // Recalcula a quantidade disponível da sessao
            sessao.QuantidadeDisponivel = sessaoInputModel.QuantidadeLugares - (sessao.QuantidadeLugares - sessao.QuantidadeDisponivel);

            if(sessao.QuantidadeDisponivel < 0)
            {
                string error = "Quantidade inválida, ingressos já vendidos";
                _logger.LogError(error);
                return BadRequest(error);
            }

            sessao.QuantidadeLugares = sessaoInputModel.QuantidadeLugares;
            sessao.ValorIngresso = sessaoInputModel.ValorIngresso;

            try
            {
                DateTime data = new DateTime(sessao.Ano, sessao.Mes, sessao.Dia);
            }
            catch
            {
                string error = "Data inválida";
                _logger.LogError(error);
                return BadRequest(error);
            }

            _sessoesRepositorio.Alterar(sessao);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Sessão {sessao} alterada", sessao.Id);

            return Ok(sessao);
        }

        [HttpPut("{id}/{quantidadeIngressos}")]
        public async Task<IActionResult> VenderIngressos(string id, int quantidadeIngressos, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                string error = "Id inválido";
                _logger.LogError(error);
                return BadRequest("Id inválido");
            }

            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(guid, cancellationToken);
            if (sessao == null)
            {
                return NotFound();
            }

            if(sessao.QuantidadeDisponivel < quantidadeIngressos)
            {
                string error = "Quantidade de ingressos maior que a quantidade disponível";
                _logger.LogError(error);
                return BadRequest(error);
            }

            sessao.QuantidadeDisponivel -= quantidadeIngressos;

            _sessoesRepositorio.Alterar(sessao);
            await _sessoesRepositorio.CommitAsync(cancellationToken);

            _logger.LogInformation("Ingressos vendidos {quantidade} para a {sessao}", quantidadeIngressos, sessao.Id);

            return Ok(sessao);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                string error = "Id inválido";
                _logger.LogError(error);
                return BadRequest("Id inválido");
            }

            var sessao = await _sessoesRepositorio.RecuperarPorIdAsync(guid, cancellationToken);
            if (sessao == null)
            {
                return NotFound();
            }

            return Ok(sessao);
        }

        [HttpPut()]
        public async Task<IActionResult> RecuperarSecoesPorFiltro([FromBody] SessaoFilmeInputModel sessaoFilmeInputModel, CancellationToken cancellationToken)
        {
            if(sessaoFilmeInputModel.Dia == 0 && sessaoFilmeInputModel.Mes == 0 && sessaoFilmeInputModel.Ano == 0 && sessaoFilmeInputModel.NomeFilme.Length == 0)
            {
                string error = "Nenhum parâmetro informado na consulta de filmes por dia";
                _logger.LogError(error);
                return BadRequest(error);
            }

            var sessao = await _sessoesRepositorio.RecuperarSecoesFilme(sessaoFilmeInputModel, cancellationToken);
            if (sessao == null)
            {
                return NotFound();
            }

            return Ok(sessao);
        }


        [HttpGet()]
        public async Task<IActionResult> RecuperarTodosAsync(CancellationToken cancellationToken)
        {
            var sessao = await _sessoesRepositorio.RecuperarTodosAsync();
            if (sessao == null)
            {
                return NotFound();
            }

            return Ok(sessao);
        }
    }
}
