using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type UserDiscount
	/// </summary>
	public class UserDiscountRepo : IUserDiscountRepo
    {
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public UserDiscountRepo(DevShopContext context)
        {
            _context = context;
        }
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Discounts of a given User from the database
		/// </summary>
		/// <param name="_userID">
		/// The ID of the User, whose Discounts should be selected
		/// </param>
		/// <returns>
		/// A list of objects of type UserDiscount
		/// </returns>
		public async Task<List<UserDiscount>> GetAllModelsAsync(int _userID)
		{
			List<UserDiscount> models = await _context.UserDiscounts.Where(m => m.UserId == _userID).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single UserDiscount from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type UserDiscount
		/// </returns>
		public async Task<UserDiscount> GetModelByPkAsync(int _pk)
		{
			UserDiscount model = await _context.UserDiscounts.FirstOrDefaultAsync(m => m.UserDiscountId == _pk);

			return model;
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_pk">
		/// Primary Key of the model, that should be removed
		/// </param>
		public async Task DeleteModelAsync(int _pk)
		{
			UserDiscount model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.UserDiscounts.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
