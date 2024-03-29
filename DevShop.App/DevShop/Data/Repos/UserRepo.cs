﻿using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Company
	/// </summary>
	public class UserRepo : IUserRepo
    {
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public UserRepo(DevShopContext context)
        {
            _context = context;
        }
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Users of a given Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, whose users should be selected
		/// </param>
		/// <returns>
		/// A list of objects of type User
		/// </returns>
		public async Task<List<User>> GetAllModelsAsync(string _compCode)
        {
            List<User> models = await _context.Users.Where(m => m.CompCode == _compCode).ToListAsync();

            return models;
        }



		/// <summary>
		/// Get all existing Users, that have a given Role
		/// </summary>
		/// <param name="_roleNr">
		/// PK of the role, that is assigne to the User
		/// </param>
		/// <returns>
		/// A list of objects of type User
		/// </returns>
		public async Task<List<User>> GetAllModelsAsync(int _roleNr)
        {
			List<User> models = await _context.Users.Where(m => m.RoleNr == _roleNr).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single User from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type User
		/// </returns>
		public async Task<User> GetModelByPkAsync(int _pk)
		{
			User model = await _context.Users.FirstOrDefaultAsync(m => m.UserId == _pk);

			return model;
		}
		#endregion



		#region Update
		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(User _model)
		{
			_context.Users.Update(_model);
			await _context.SaveChangesAsync();
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
			User model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.Users.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
