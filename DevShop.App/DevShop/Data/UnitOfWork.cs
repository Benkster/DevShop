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
		private readonly IWebHostEnvironment _env;
		private readonly TreeBuilder _treeBuilder = new TreeBuilder();

		private CompanyRepo _companyRepo;
		private RoleRepo _roleRepo;
		private CategoryRepo _categoryRepo;
		private CountryRepo _countryRepo;
		private StateRepo _stateRepo;
		private CityRepo _cityRepo;
		private UserRepo _userRepo;
		private UserDiscountRepo _userDiscountRepo;
		private ProductGroupRepo _productGroupRepo;
		private ProductRepo _productRepo;
		private ArticleRepo _articleRepo;
		private ArticleHeaderRepo _articleHeaderRepo;
		private UnitRepo _unitRepo;


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

		// Allows access to the ProductGroup-Repository
		public ProductGroupRepo ProductGroupRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_productGroupRepo = (_productGroupRepo == null) ? new ProductGroupRepo(_context, _treeBuilder) : _productGroupRepo;

				return _productGroupRepo;
			}
		}

		// Allows access to the Product-Repository
		public ProductRepo ProductRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_productRepo = (_productRepo == null) ? new ProductRepo(_context) : _productRepo;

				return _productRepo;
			}
		}

		// Allows access to the Article-Repository
		public ArticleRepo ArticleRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_articleRepo = (_articleRepo == null) ? new ArticleRepo(_context, _env) : _articleRepo;

				return _articleRepo;
			}
		}

		// Allows access to the ArticleHeader-Repository
		public ArticleHeaderRepo ArticleHeaderRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_articleHeaderRepo = (_articleHeaderRepo == null) ? new ArticleHeaderRepo(_context) : _articleHeaderRepo;

				return _articleHeaderRepo;
			}
		}

		// Allows access to the Unit-Repository
		public UnitRepo UnitRepo
		{
			get
			{
				// Create a new instance, if it has not yet been done
				_unitRepo = (_unitRepo == null) ? new UnitRepo(_context) : _unitRepo;

				return _unitRepo;
			}
		}
		#endregion



		#region Constructors
		public UnitOfWork(DevShopContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}
		#endregion
	}
}
