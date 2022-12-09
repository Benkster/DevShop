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
		private CountryRepo _countryRepo;
		private StateRepo _stateRepo;
		private CityRepo _cityRepo;
		private UserRepo _userRepo;
		private UserDiscountRepo _userDiscountRepo;


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

		// Allows access to the Country-Repository
		public CountryRepo CountryRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_countryRepo = (_countryRepo == null) ? new CountryRepo(_context) : _countryRepo;

				return _countryRepo;
			}
		}

		// Allows access to the State-Repository
		public StateRepo StateRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_stateRepo = (_stateRepo == null) ? new StateRepo(_context) : _stateRepo;

				return _stateRepo;
			}
		}

		// Allows access to the City-Repository
		public CityRepo CityRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_cityRepo = (_cityRepo == null) ? new CityRepo(_context) : _cityRepo;

				return _cityRepo;
			}
		}

		// Allows access to the User-Repository
		public UserRepo UserRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_userRepo = (_userRepo == null) ? new UserRepo(_context) : _userRepo;

				return _userRepo;
			}
		}

		// Allows access to the UserDiscount-Repository
		public UserDiscountRepo UserDiscountRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_userDiscountRepo = (_userDiscountRepo == null) ? new UserDiscountRepo(_context) : _userDiscountRepo;

				return _userDiscountRepo;
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
