using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for UserDiscount-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IUserDiscountRepo
	{
		void CreateNewUserDiscountAsync(UserDiscount userDiscount);
		void DeleteUserDiscountAsync(int id);
		Task<List<UserDiscount>> GetAllUserDiscountsAsync();
		Task<UserDiscount> GetUserDiscountByIdAsync(int id);
		void UpdateUserDiscountAsync(UserDiscount userDiscount);
	}
}
