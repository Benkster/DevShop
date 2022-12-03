using DevShop.Data.Repos;

namespace DevShop.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		#region Variables/Properties
		private readonly DevShopContext _context;

		private CompanyRepo _companyRepo;
		private RoleRepo _roleRepo;


		public CompanyRepo CompanyRepo
		{
			get
			{
				_companyRepo = (_companyRepo == null) ? new CompanyRepo(_context) : _companyRepo;
				
				return _companyRepo;
			}
		}

		public RoleRepo RoleRepo
		{
			get
			{
				_roleRepo = (_roleRepo == null) ? new RoleRepo(_context) : _roleRepo;
				
				return _roleRepo;
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
