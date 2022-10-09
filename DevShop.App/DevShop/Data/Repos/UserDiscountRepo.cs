using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of UserDiscount-objects
	/// </summary>
	public class UserDiscountRepo : IUserDiscountRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public UserDiscountRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing UserDiscounts from the DB
		/// </summary>
		/// <returns>
		/// A list of all UserDiscounts
		/// </returns>
		public async Task<List<UserDiscount>> GetAllUserDiscountsAsync()
		{
			List<UserDiscount> userDiscounts = new List<UserDiscount>();


			userDiscounts = await _context.UserDiscounts.ToListAsync();


			return userDiscounts;
		}



		/// <summary>
		/// Get the selected UserDiscount from the DB via its ID
		/// </summary>
		/// <param name="id">
		/// The ID of the UserDiscount
		/// </param>
		/// <returns>
		/// A single object of type UserDiscount
		/// </returns>
		public async Task<UserDiscount> GetUserDiscountByIdAsync(int id)
		{
			UserDiscount userDiscount = new UserDiscount();


			userDiscount = await _context.UserDiscounts.FirstOrDefaultAsync(u => u.UserDiscountID == id);


			return userDiscount;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new UserDiscount into the DB
		/// </summary>
		/// <param name="userDiscount">
		/// The UserDiscount, that should be created
		/// </param>
		public async void CreateNewUserDiscountAsync(UserDiscount userDiscount)
		{
			_context.UserDiscounts.Add(userDiscount);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing UserDiscount
		/// </summary>
		/// <param name="userDiscount">
		/// The UserDiscount, that should be updated, containing the new data
		/// </param>
		public async void UpdateUserDiscountAsync(UserDiscount userDiscount)
		{
			_context.UserDiscounts.Update(userDiscount);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a UserDiscount from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the UserDiscount, that should be deleted
		/// </param>
		public async void DeleteUserDiscountAsync(int id)
		{
			UserDiscount userDiscount = await GetUserDiscountByIdAsync(id);

			_context.UserDiscounts.Remove(userDiscount);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
