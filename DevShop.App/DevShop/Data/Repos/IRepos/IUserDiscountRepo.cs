using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for User-Discount-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface IUserDiscountRepo
    {
        Task DeleteModelAsync(int _pk);
        Task<List<UserDiscount>> GetAllModelsAsync(int _userID);
        Task<UserDiscount> GetModelByPkAsync(int _pk);
    }
}
