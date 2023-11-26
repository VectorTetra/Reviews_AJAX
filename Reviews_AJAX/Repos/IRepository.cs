using Reviews_AJAX.Models;

namespace Reviews_AJAX.Repos
{
    public interface IRepository
    {
        Task<List<UserReviewVM>> GetReviews();
        Task<List<User>> GetUsers();
        Task<User?> FindUser(int id);
        Task CreateReview(UserReviewVM reviewVM);
        Task<bool> TryToRegister(RegisterVM registerVM);
        Task<string?> TryToLogin(LoginVM loginVM);
    }
}
