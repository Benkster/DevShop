using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of State-objects
	/// </summary>
	public class StateRepo : IStateRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public StateRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing States from the DB
		/// </summary>
		/// <returns>
		/// A list of all States
		/// </returns>
		public async Task<List<State>> GetAllStatesAsync()
		{
			List<State> states = new List<State>();


			states = await _context.States.ToListAsync();


			return states;
		}



		/// <summary>
		/// Get the selected State from the DB via its ID
		/// </summary>
		/// <param name="id">
		/// The ID of the State
		/// </param>
		/// <returns>
		/// A single object of type State
		/// </returns>
		public async Task<State> GetStateByIdAsync(int id)
		{
			State state = new State();


			state = await _context.States.FirstOrDefaultAsync(s => s.StateID == id);


			return state;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new State into the DB
		/// </summary>
		/// <param name="state">
		/// The State, that should be created
		/// </param>
		public async void CreateNewStateAsync(State state)
		{
			_context.States.Add(state);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing State
		/// </summary>
		/// <param name="state">
		/// The State, that should be updated, containing the new data
		/// </param>
		public async void UpdateStateAsync(State state)
		{
			_context.States.Update(state);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a State from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the State, that should be deleted
		/// </param>
		public async void DeleteStateAsync(int id)
		{
			State state = await GetStateByIdAsync(id);

			_context.States.Remove(state);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
