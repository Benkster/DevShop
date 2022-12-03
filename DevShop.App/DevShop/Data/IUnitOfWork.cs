using DevShop.Data.Repos;

namespace DevShop.Data
{
	public interface IUnitOfWork
	{
		CompanyRepo CompanyRepo { get; }
		RoleRepo RoleRepo { get; }
	}
}
