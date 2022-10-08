using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data
{
	public class DevShopDbContext : IdentityDbContext
	{
		public DevShopDbContext(DbContextOptions<DevShopDbContext> options)
			: base(options)
		{

		}
	}
}
