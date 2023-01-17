using System.Threading.Tasks;

namespace JD.CTC.Shared.Interfaces
{
    public enum Perfil
    {
        Admin, Master, Opr
    }

    public interface IUsuarioLogado
    {
        Task<bool> IsAuthenticated();
        Task<bool> IsAuthorizedAsync(params Perfil[] perfil);
        bool IsAuthorized(params Perfil[] perfil);
        Task<bool> IsAdmin();
        Task<bool> IsMaster();
        Task<bool> IsOperational();
        Task<int> ISPB();
        Task<long> CNPJ();
        Task<int> CDUser();
        Task<int> CDPerfil();
        Task<string> Role();
        Task<string> Instituicao();
        bool ModoLogin();
    }
}
