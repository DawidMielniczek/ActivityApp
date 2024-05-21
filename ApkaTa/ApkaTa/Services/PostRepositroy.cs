using ApkaTa.Models;
using Newtonsoft.Json;
using System.Text;

namespace ApkaTa.Services
{
    public class PostRepositroy: IPostRepository
    {
        public readonly string baseUrl = "http://192.168.8.122:8088/";

        public async Task<bool> AddPostUser(PostUsers postUser)
        {
            string JsonModel = JsonConvert.SerializeObject(postUser);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "AddUserPost";
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


        public async Task<bool> DeletePost(int idU, int PostId)
        {
            var client = new HttpClient();

            string url = baseUrl + "Api/Aktywni/DeletePostUser/" + idU + "/" + PostId;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync("");
            if (response.IsSuccessStatusCode)
            {

                return await Task.FromResult(true);
            }
            else
                return await Task.FromResult(false);
        }

        public async Task<IEnumerable<PostUser>> GetAllPost()
        {
            var _postInfoList = new List<PostUser>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetAllPost";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _postInfoList = JsonConvert.DeserializeObject<List<PostUser>>(content);

                return await Task.FromResult(_postInfoList);
            }
            else
                return null;
        }

        public async Task<IEnumerable<PostUser>> GetYourPost(int idU)
        {
            var _yourPostInfoList = new List<PostUser>();
            var client = new HttpClient();

            string url = "http://192.168.8.122:8088/api/Aktywni/GetPostUser/" + idU;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {

                string content = response.Content.ReadAsStringAsync().Result;
                _yourPostInfoList = JsonConvert.DeserializeObject<List<PostUser>>(content);

                return await Task.FromResult(_yourPostInfoList);
            }
            else
                return null;
        }

        public async Task<bool> UpdatePost(PostUsers posts)
        {
            string JsonModel = JsonConvert.SerializeObject(posts);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "api/Aktywni/UpdatePost";
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

        public async Task<bool> UpdateViews(PostUsers posts)
        {
            string JsonModel = JsonConvert.SerializeObject(posts);
            StringContent strContent = new StringContent(JsonModel, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            string url = baseUrl + "api/Aktywni/AddWyświetlenie";
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
