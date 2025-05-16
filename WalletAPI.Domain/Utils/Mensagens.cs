namespace WalletAPI.Domain.Utils
{
    public static class Mensagens
    {
        public static string SessaoInvalida { get; } = "EXTRATOS_SESSAO_INVALIDA";
        public static string UsuarioSenhaInvalidos { get; } = "Usuário ou senha inválidos";
        public static string UsuarioNaoEncontrado { get; } = "Usuário não encontrado";
        public static string DestinatarioNaoEncontrado { get; } = "Destinatário não encontrado.";
        public static string RemetenteNaoEncontrado { get; } = "Remetente não encontrado.";
        public static string TransferenciaInvalida { get; } = "Não é possível transferir para si mesmo.";
        public static string ErroEmail { get; } = "Email já está em uso";
        public static string ErroValorWallet { get; } = "Valor deve ser maior que zero.";
    }
}
