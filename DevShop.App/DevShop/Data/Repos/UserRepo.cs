using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of User-objects
	/// </summary>
	public class UserRepo : IUserRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public UserRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Users from the DB
		/// </summary>
		/// <returns>
		/// A list of all Users
		/// </returns>
		public async Task<List<User>> GetAllUsersAsync()
		{
			List<User> users = new List<User>();


			users = await _context.Users.ToListAsync();


			return users;
		}



		/// <summary>
		/// Get the selected User from the DB via his ID
		/// </summary>
		/// <param name="id">
		/// The ID of the User
		/// </param>
		/// <returns>
		/// A single object of type User
		/// </returns>
		public async Task<User> GetUserByIdAsync(string id)
		{
			User user = new User();


			user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);


			return user;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new User into the DB
		/// </summary>
		/// <param name="user">
		/// The User, that should be created
		/// </param>
		public async void CreateNewUserAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing User
		/// </summary>
		/// <param name="user">
		/// The User, that should be updated, containing the new data
		/// </param>
		public async void UpdateUserAsync(User user)
		{
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a User from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the User, that should be deleted
		/// </param>
		public async void DeleteUserAsync(string id)
		{
			User user = await GetUserByIdAsync(id);

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
