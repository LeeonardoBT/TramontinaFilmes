namespace TramontinaFilmes.WebApi.Hosting.Configuration
{
    public class SessoesOptions
    {
        public const string Sessoes = "Sessoes";

        public bool Aberto { get; set; } = false;
        public string MensagemSucesso { get; set; } = "Sessão cadastrada com sucesso";
    }
}
