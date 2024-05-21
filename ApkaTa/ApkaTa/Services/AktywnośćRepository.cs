using ApkaTa.Models;
using Newtonsoft.Json;
using System.Text;

namespace ApkaTa.Services
{
    public class AktywnośćRepository : IAktywnośćRepository
    {
        public readonly string baseUrl = "http://192.168.8.122:8088/";

        public async Task<bool> AddAktywnośćUser(AktywnośćUser aktywnośćUser)
        {
            string JsonModel = JsonConvert.SerializeObject(aktywnośćUser);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "AddActive";
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
        public async Task<bool> AddAktywnośćUsr(AktywnośćUsr usr)
        {
            string JsonModel = JsonConvert.SerializeObject(usr);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "AddActiveUser";
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

        public async Task<bool> DeleteAktywnośćUser(int AktywnośćId)
        {
            var client = new HttpClient();

            string url = baseUrl + "DeleteUserPost/" + AktywnośćId;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync("");
            if (response.IsSuccessStatusCode)
            {

                return await Task.FromResult(true);
            }
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAktywnośćUsr(int idU, int Aktywność)
        {
            var client = new HttpClient();

            string url = baseUrl + "DeleteActiveUser/" + idU + "," + Aktywność;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync("");
            if (response.IsSuccessStatusCode)
            {

                return await Task.FromResult(true);
            }
            else
                return await Task.FromResult(false);


        }

        public async Task<IEnumerable<Aktywność>> GetAktywności()
        {
            var _aktywność = new List<Aktywność>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetNazwaAktywności";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _aktywność = JsonConvert.DeserializeObject<List<Aktywność>>(content);

                return await Task.FromResult(_aktywność);
            }
            else
                return null;
        }

        public async Task<IEnumerable<AktywnośćUserModel>> GetDostępneAktywności(int idU)
        {
            var _aktywnośćInfoList = new List<AktywnośćUserModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetActive/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _aktywnośćInfoList = JsonConvert.DeserializeObject<List<AktywnośćUserModel>>(content);

                return await Task.FromResult(_aktywnośćInfoList);
            }
            else
                return null;
        } 
        public async Task<IEnumerable<AktywnośćUserModel>> GetHistoriaWyd(int idU)
        {
            var _aktywnośćInfoList = new List<AktywnośćUserModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetHistoria/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _aktywnośćInfoList = JsonConvert.DeserializeObject<List<AktywnośćUserModel>>(content);

                return await Task.FromResult(_aktywnośćInfoList);
            }
            else
                return null;
        }

        public async Task<IEnumerable<AktywnośćUsr>> GetLastAktywność()
        {
            var _lastAktywność = new List<AktywnośćUsr>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/LastAktywnosc";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _lastAktywność = JsonConvert.DeserializeObject<List<AktywnośćUsr>>(content);

                return await Task.FromResult(_lastAktywność);
            }
            else
                return null;
        }

        public async Task<IEnumerable<AktywnośćUserModel>> GetNadchodzącaAktywności(int idU)
        {
            var _nadchodzącaInfoList = new List<AktywnośćUserModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetAktywnośćNadchodząca/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _nadchodzącaInfoList = JsonConvert.DeserializeObject<List<AktywnośćUserModel>>(content);

                return await Task.FromResult(_nadchodzącaInfoList);
            }
            else
                return null;
        }
        public async Task<IEnumerable<AktywnośćUserModel>> GetLastHistoryAktywność(int idU)
        {
            var _nadchodzącaInfoList = new List<AktywnośćUserModel>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetLastHistoria/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _nadchodzącaInfoList = JsonConvert.DeserializeObject<List<AktywnośćUserModel>>(content);

                return await Task.FromResult(_nadchodzącaInfoList);
            }
            else
                return null;
        }


    }
}
