using DevShop.Data.Repos;

namespace DevShop.Data
{
	/// <summary>
	/// The UnitOfWork functions as a middleman between the Views and the Repositories.
	/// A View only has to inject the UOW to be able to access every Repo. This way, it is not necessary to implement every single Repository that is needed in the View.
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{
		#region Variables/Properties
		private readonly DevShopContext _context;
		private readonly TreeBuilder _treeBuilder = new TreeBuilder();

		private CompanyRepo _companyRepo;
		private RoleRepo _roleRepo;
		private CategoryRepo _categoryRepo;


		// Allows access to the Company-Repository
		public CompanyRepo CompanyRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_companyRepo = (_companyRepo == null) ? new CompanyRepo(_context) : _companyRepo;
				
				return _companyRepo;
			}
		}

		// Allows access to the Role-Repository
		public RoleRepo RoleRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_roleRepo = (_roleRepo == null) ? new RoleRepo(_context) : _roleRepo;
				
				return _roleRepo;
			}
		}

		// Allows access to the Category-Repository
		public CategoryRepo CategoryRepo
        {
			get
            {
				// Create a new instance, if it has not yet been done
				_categoryRepo = (_categoryRepo == null) ? new CategoryRepo(_context, _treeBuilder) : _categoryRepo;

				return _categoryRepo;
            }
        }
		#endregion



		#region Constructors
		public UnitOfWork(DevShopContext context)
		{
			_context = context;
		}
		#endregion
	}
}
