using DevShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data
{
	public class DevShopDbContext : IdentityDbContext
	{
		#region Properties
		public virtual DbSet<Role> Roles { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<State> States { get; set; }
		public virtual DbSet<City> Cities { get; set; }
		public virtual DbSet<Company> Companies { get; set; }
		public virtual DbSet<Address> Addresses { get; set; }
		public virtual DbSet<ProductGroup> ProductGroups { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Unit> Units { get; set; }
		public virtual DbSet<Article> Articles { get; set; }
		public virtual DbSet<UserDiscount> UserDiscounts { get; set; }
		#endregion



		public DevShopDbContext(DbContextOptions<DevShopDbContext> options)
			: base(options)
		{

		}



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			#region Keys
			builder.Entity<Article>()
				.HasKey(a => new { a.ArticleNr, a.ProductNr, a.CompCode });


			builder.Entity<City>()
				.HasKey(c => new { c.StateID, c.ZIP });


			builder.Entity<Country>()
				.HasKey(c => new { c.CountryID, c.CountryCode });


			builder.Entity<Product>()
				.HasKey(p => new { p.ProductNr, p.ProductGroupNr });


			builder.Entity<ProductGroup>()
				.HasKey(pg => new { pg.ProductGroupNr, pg.CompCode });
			#endregion
		}
	}
}
