using ApkaTa.Models;

namespace ApkaTa.Services
{
    public interface IUserRepository
    {
        public  Task<UserViewModel> GetUserInfo(string email, string password);
        
        Task<UserViewModel> GetUserId(int idU);
        Task<IEnumerable<UserViewModel>> GetInfo();
        Task<IEnumerable<UserViewModel>> GetUser();

        Task<bool> UpdateUserInfo(UserViewModel userModel);

        Task<bool> AddUserAccount(Users usermodel);
    }
}
