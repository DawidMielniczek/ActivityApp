using ApkaTa.Models;
using Newtonsoft.Json;
using System.Text;

namespace ApkaTa.Services
{
    public class UserRepository: IUserRepository
    {
        public async Task<UserViewModel> GetUserInfo(string email, string password)
        {
            var _UserInfo = new UserViewModel();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetUserInfo/" + email + "/" + password;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _UserInfo = JsonConvert.DeserializeObject<UserViewModel>(content);

                return await Task.FromResult(_UserInfo);
            }
            else
                return null;
        }

        public async Task<IEnumerable<UserViewModel>> GetInfo()
        {
            var _UserInfo = new List<UserViewModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetInfo/";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _UserInfo = JsonConvert.DeserializeObject<List<UserViewModel>>(content);

                return await Task.FromResult(_UserInfo);
            }
            else
                return null;
        }

        public readonly string baseUrl = "http://192.168.8.122:8088/";

        public async Task<bool> AddUserAccount(Users userModel)
        {

            string JsonModel = JsonConvert.SerializeObject(userModel);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "AddUserAccount";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", strContent);
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetUser()
        {
            var _userInfoList = new List<UserViewModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetUsers";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _userInfoList = JsonConvert.DeserializeObject<List<UserViewModel>>(content);

                return await Task.FromResult(_userInfoList);
            }
            else
                return null;
        }

        public async Task<UserViewModel> GetUserId(int idU)
        {
            var _UserInfo = new UserViewModel();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetUserById/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _UserInfo = JsonConvert.DeserializeObject<UserViewModel>(content);

                return await Task.FromResult(_UserInfo);
            }
            else
                return null;
        }

       
        public async Task<bool> UpdateUserInfo(UserViewModel userModel)
        {
            string JsonModel = JsonConvert.SerializeObject(userModel);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "api/Aktywni";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PutAsync("", strContent);
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }

        }


    }
}

