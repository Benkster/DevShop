using DevShop.Data.Repos;

namespace DevShop.Data
{
	/// <summary>
	/// Interface for the UOW.
	/// It defines all Properties, the UOW has to implement
	/// </summary>
	public interface IUnitOfWork
	{
		RoleRepo RoleRepo { get; }
		UserRepo UserRepo { get; }
		CategoryRepo CategoryRepo { get; }
		CountryRepo CountryRepo { get; }
		StateRepo StateRepo { get; }
		CityRepo CityRepo { get; }
		CompanyRepo CompanyRepo { get; }
		AddressRepo AddressRepo { get; }
		ProductGroupRepo ProductGroupRepo { get; }
		ProductRepo ProductRepo { get; }
		UnitRepo UnitRepo { get; }
		ArticleRepo ArticleRepo { get; }
		UserDiscountRepo UserDiscountRepo { get; }
	}
}
