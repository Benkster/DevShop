using DevShop.Data.Repos;

namespace DevShop.Data
{
	/// <summary>
	/// The UOW is the central managing system that connects views with repositories.
	/// Views only have to inject the UOW and are then able to access every repository.
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{
		#region Variables/Properties
		private DevShopDbContext _context;

		private RoleRepo roleRepo;
		private UserRepo userRepo;
		private CategoryRepo categoryRepo;
		private CountryRepo countryRepo;
		private StateRepo stateRepo;
		private CityRepo cityRepo;
		private CompanyRepo companyRepo;
		private AddressRepo addressRepo;
		private ProductGroupRepo productGroupRepo;
		private ProductRepo productRepo;
		private UnitRepo unitRepo;
		private ArticleRepo articleRepo;
		private UserDiscountRepo userDiscountRepo;



		#region Repo-Properties
		/// <summary>
		/// Enables access to the Role-Repository
		/// </summary>
		public RoleRepo RoleRepo 
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				roleRepo = (roleRepo == null) ? new RoleRepo(_context) : roleRepo;

				return roleRepo;
			}
		}

		/// <summary>
		/// Enables access to the User-Repository
		/// </summary>
		public UserRepo UserRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				userRepo = (userRepo == null) ? new UserRepo(_context) : userRepo;

				return userRepo;
			}
		}

		/// <summary>
		/// Enables access to the Category-Repository
		/// </summary>
		public CategoryRepo CategoryRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				categoryRepo = (categoryRepo == null) ? new CategoryRepo(_context) : categoryRepo;

				return categoryRepo;
			}
		}

		/// <summary>
		/// Enables access to the Country-Repository
		/// </summary>
		public CountryRepo CountryRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				countryRepo = (countryRepo == null) ? new CountryRepo(_context) : countryRepo;

				return countryRepo;
			}
		}

		/// <summary>
		/// Enables access to the State-Repository
		/// </summary>
		public StateRepo StateRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				stateRepo = (stateRepo == null) ? new StateRepo(_context) : stateRepo;

				return stateRepo;
			}
		}

		/// <summary>
		/// Enables access to the City-Repository
		/// </summary>
		public CityRepo CityRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				cityRepo = (cityRepo == null) ? new CityRepo(_context) : cityRepo;

				return cityRepo;
			}
		}

		/// <summary>
		/// Enables access to the Company-Repository
		/// </summary>
		public CompanyRepo CompanyRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				companyRepo = (companyRepo == null) ? new CompanyRepo(_context) : companyRepo;

				return companyRepo;
			}
		}

		/// <summary>
		/// Enables access to the Address-Repository
		/// </summary>
		public AddressRepo AddressRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				addressRepo = (addressRepo == null) ? new AddressRepo(_context) : addressRepo;

				return addressRepo;
			}
		}

		/// <summary>
		/// Enables access to the ProductGroup-Repository
		/// </summary>
		public ProductGroupRepo ProductGroupRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				productGroupRepo = (productGroupRepo == null) ? new ProductGroupRepo(_context) : productGroupRepo;

				return productGroupRepo;
			}
		}

		/// <summary>
		/// Enables access to the Product-Repository
		/// </summary>
		public ProductRepo ProductRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				productRepo = (productRepo == null) ? new ProductRepo(_context) : productRepo;

				return productRepo;
			}
		}

		/// <summary>
		/// Enables access to the Unit-Repository
		/// </summary>
		public UnitRepo UnitRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				unitRepo = (unitRepo == null) ? new UnitRepo(_context) : unitRepo;

				return unitRepo;
			}
		}

		/// <summary>
		/// Enables access to the Article-Repository
		/// </summary>
		public ArticleRepo ArticleRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				articleRepo = (articleRepo == null) ? new ArticleRepo(_context) : articleRepo;

				return articleRepo;
			}
		}

		/// <summary>
		/// Enables access to the UserDiscount-Repository
		/// </summary>
		public UserDiscountRepo UserDiscountRepo
		{
			get
			{
				// Create a new instance of the repository, if it has not yet been done
				userDiscountRepo = (userDiscountRepo == null) ? new UserDiscountRepo(_context) : userDiscountRepo;

				return userDiscountRepo;
			}
		}
		#endregion
		#endregion



		#region Constructors
		public UnitOfWork(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion
	}
}
