using ApkaTa.Models;

namespace ApkaTa.Services
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostUser>> GetAllPost();
        Task<IEnumerable<PostUser>> GetYourPost(int idU);
        Task<bool> AddPostUser(PostUsers postUsers);
        Task<bool> DeletePost(int idU, int PostId);
        Task<bool> UpdateViews(PostUsers posts);


        Task<bool> UpdatePost(PostUsers posts);
    }
}
