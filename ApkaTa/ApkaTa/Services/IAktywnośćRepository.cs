using ApkaTa.Models;

namespace ApkaTa.Services
{
    public interface IAktywnośćRepository
    {
        Task<IEnumerable<Aktywność>> GetAktywności();

        Task<IEnumerable<AktywnośćUsr>> GetLastAktywność();
        Task<IEnumerable<AktywnośćUserModel>> GetLastHistoryAktywność(int idU);

        Task<IEnumerable<AktywnośćUserModel>> GetDostępneAktywności(int idU);
        Task<IEnumerable<AktywnośćUserModel>> GetNadchodzącaAktywności(int idU);
        Task<IEnumerable<AktywnośćUserModel>> GetHistoriaWyd(int idU);


        Task<bool> AddAktywnośćUser(AktywnośćUser aktywnośćUser);
        Task<bool> AddAktywnośćUsr(AktywnośćUsr aktywnośćUsr);

        Task<bool> DeleteAktywnośćUsr(int idU, int Aktywność);
        Task<bool> DeleteAktywnośćUser( int AktywnośćId);
    }
}
