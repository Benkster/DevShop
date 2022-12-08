using DevShop.Data.Repos;

namespace DevShop.Data
{
	public interface IUnitOfWork
	{
		CompanyRepo CompanyRepo { get; }
		RoleRepo RoleRepo { get; }
        CategoryRepo CategoryRepo { get; }
		CountryRepo CountryRepo { get; }
        StateRepo StateRepo { get; }
		CityRepo CityRepo { get; }
	}
}
