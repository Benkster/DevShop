using System;
using System.Collections.Generic;
using DevShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data;

public partial class DevShopContext : DbContext
{
	#region Constructors
	public DevShopContext()
	{
	}

	public DevShopContext(DbContextOptions<DevShopContext> options)
		: base(options)
	{
	}
	#endregion


	#region Properties
	public virtual DbSet<Address> Addresses { get; set; }

	public virtual DbSet<Article> Articles { get; set; }

	public virtual DbSet<ArticleHeader> ArticleHeaders { get; set; }

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<City> Cities { get; set; }

	public virtual DbSet<Company> Companies { get; set; }

	public virtual DbSet<Country> Countries { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<ProductGroup> ProductGroups { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<State> States { get; set; }

	public virtual DbSet<Unit> Units { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserDiscount> UserDiscounts { get; set; }
	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Address>(entity =>
		{
			entity.HasKey(e => e.AddressId).HasName("pk_Addresses");

			entity.Property(e => e.AddressId).HasColumnName("AddressID");
			entity.Property(e => e.HouseNr).HasMaxLength(20);
			entity.Property(e => e.Info).HasMaxLength(300);
			entity.Property(e => e.StateId).HasColumnName("StateID");
			entity.Property(e => e.Street).HasMaxLength(150);
			entity.Property(e => e.Zip)
				.HasMaxLength(5)
				.HasColumnName("ZIP");

			entity.HasOne(d => d.City).WithMany(p => p.Addresses)
				.HasForeignKey(d => new { d.StateId, d.Zip })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Addresses_Cities");
		});

		modelBuilder.Entity<Article>(entity =>
		{
			entity.HasKey(e => new { e.ArticleNr, e.ProductNr, e.ProductGroupNr, e.CompCode }).HasName("pk_Articles");

			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.Article1)
				.HasMaxLength(150)
				.HasColumnName("Article");
			entity.Property(e => e.ArticleCode).HasMaxLength(30);
			entity.Property(e => e.ArticleDescription).HasMaxLength(700);
			entity.Property(e => e.ArticleHeaderId).HasColumnName("ArticleHeaderID");
			entity.Property(e => e.BillingUnit).HasMaxLength(3);
			entity.Property(e => e.Discount).HasColumnType("decimal(3, 2)");
			entity.Property(e => e.Ean)
				.HasMaxLength(13)
				.HasColumnName("EAN");
			entity.Property(e => e.F1).HasMaxLength(250);
			entity.Property(e => e.F2).HasMaxLength(250);
			entity.Property(e => e.F3).HasMaxLength(250);
			entity.Property(e => e.F4).HasMaxLength(250);
			entity.Property(e => e.F5).HasMaxLength(250);
			entity.Property(e => e.F6).HasMaxLength(250);
			entity.Property(e => e.PackagingUnit).HasMaxLength(3);
			entity.Property(e => e.Price).HasColumnType("money");

			entity.HasOne(d => d.ArticleHeader).WithMany(p => p.Articles)
				.HasForeignKey(d => d.ArticleHeaderId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Articles_ArticleHeaders");

			entity.HasOne(d => d.BillingUnitNavigation).WithMany(p => p.ArticleBillingUnitNavigations)
				.HasForeignKey(d => d.BillingUnit)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Articles_Billing_Units");

			entity.HasOne(d => d.PackagingUnitNavigation).WithMany(p => p.ArticlePackagingUnitNavigations)
				.HasForeignKey(d => d.PackagingUnit)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Articles_Packaging_Units");

			entity.HasOne(d => d.Product).WithMany(p => p.Articles)
				.HasForeignKey(d => new { d.ProductNr, d.ProductGroupNr, d.CompCode })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Articles_Products");
		});

		modelBuilder.Entity<ArticleHeader>(entity =>
		{
			entity.HasKey(e => e.ArticleHeaderId).HasName("pk_ArticleHeaders");

			entity.Property(e => e.ArticleHeaderId).HasColumnName("ArticleHeaderID");
			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.F1name)
				.HasMaxLength(50)
				.HasColumnName("F1Name");
			entity.Property(e => e.F2name)
				.HasMaxLength(50)
				.HasColumnName("F2Name");
			entity.Property(e => e.F3name)
				.HasMaxLength(50)
				.HasColumnName("F3Name");
			entity.Property(e => e.F4name)
				.HasMaxLength(50)
				.HasColumnName("F4Name");
			entity.Property(e => e.F5name)
				.HasMaxLength(50)
				.HasColumnName("F5Name");
			entity.Property(e => e.F6name)
				.HasMaxLength(50)
				.HasColumnName("F6Name");

			entity.HasOne(d => d.Product).WithMany(p => p.ArticleHeaders)
				.HasForeignKey(d => new { d.ProductNr, d.ProductGroupNr, d.CompCode })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_ArticleHeaders_Products");
		});

		modelBuilder.Entity<Category>(entity =>
		{
			entity.HasKey(e => e.CategoryId).HasName("pk_Categories");

			entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
			entity.Property(e => e.Category1)
				.HasMaxLength(150)
				.HasColumnName("Category");
			entity.Property(e => e.Description).HasMaxLength(350);
			entity.Property(e => e.ParentId).HasColumnName("ParentID");
		});

		modelBuilder.Entity<City>(entity =>
		{
			entity.HasKey(e => new { e.StateId, e.Zip }).HasName("pk_Cities");

			entity.Property(e => e.StateId).HasColumnName("StateID");
			entity.Property(e => e.Zip)
				.HasMaxLength(5)
				.HasColumnName("ZIP");
			entity.Property(e => e.City1)
				.HasMaxLength(150)
				.HasColumnName("City");

			entity.HasOne(d => d.State).WithMany(p => p.Cities)
				.HasForeignKey(d => d.StateId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Cities_States");
		});

		modelBuilder.Entity<Company>(entity =>
		{
			entity.HasKey(e => e.CompCode).HasName("pk_Companies");

			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.CompAddName).HasMaxLength(250);
			entity.Property(e => e.CompName).HasMaxLength(250);
			entity.Property(e => e.Description).HasMaxLength(500);
			entity.Property(e => e.Mail).HasMaxLength(100);
			entity.Property(e => e.Tel).HasMaxLength(30);
			entity.Property(e => e.Website).HasMaxLength(100);

			entity.HasMany(d => d.Addresses).WithMany(p => p.CompCodes)
				.UsingEntity<Dictionary<string, object>>(
					"CompanyAddress",
					r => r.HasOne<Address>().WithMany()
						.HasForeignKey("AddressId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("fk_CompanyAddresses_Addresses"),
					l => l.HasOne<Company>().WithMany()
						.HasForeignKey("CompCode")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("fk_CompanyAddresses_Companies"),
					j =>
					{
						j.HasKey("CompCode", "AddressId").HasName("pk_CompanyAddresses");
					});
		});

		modelBuilder.Entity<Country>(entity =>
		{
			entity.HasKey(e => new { e.CountryId, e.CountryCode }).HasName("pk_Countries");

			entity.HasIndex(e => e.CountryCode, "uc_Countries").IsUnique();

			entity.Property(e => e.CountryId)
				.ValueGeneratedOnAdd()
				.HasColumnName("CountryID");
			entity.Property(e => e.CountryCode).HasMaxLength(3);
			entity.Property(e => e.Country1)
				.HasMaxLength(150)
				.HasColumnName("Country");
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(e => new { e.ProductNr, e.ProductGroupNr, e.CompCode }).HasName("pk_Products");

			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.Product1)
				.HasMaxLength(150)
				.HasColumnName("Product");
			entity.Property(e => e.ProductDescription).HasMaxLength(700);

			entity.HasOne(d => d.ProductGroup).WithMany(p => p.Products)
				.HasForeignKey(d => new { d.ProductGroupNr, d.CompCode })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Products_ProductGroups");
		});

		modelBuilder.Entity<ProductGroup>(entity =>
		{
			entity.HasKey(e => new { e.ProductGroupNr, e.CompCode }).HasName("pk_ProductGroups");

			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
			entity.Property(e => e.GroupDescription).HasMaxLength(700);
			entity.Property(e => e.GroupName).HasMaxLength(150);
			entity.Property(e => e.ParentId).HasColumnName("ParentID");

			entity.HasOne(d => d.Category).WithMany(p => p.ProductGroups)
				.HasForeignKey(d => d.CategoryId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_ProductGroups_Categories");

			entity.HasOne(d => d.CompCodeNavigation).WithMany(p => p.ProductGroups)
				.HasForeignKey(d => d.CompCode)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_ProductGroups_Companies");
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.RoleNr).HasName("pk_Roles");

			entity.Property(e => e.RoleNr).ValueGeneratedNever();
			entity.Property(e => e.Description).HasMaxLength(250);
			entity.Property(e => e.RoleName)
				.HasMaxLength(100)
				.HasColumnName("RoleName");
		});

		modelBuilder.Entity<State>(entity =>
		{
			entity.HasKey(e => e.StateId).HasName("pk_States");

			entity.Property(e => e.StateId).HasColumnName("StateID");
			entity.Property(e => e.CountryCode).HasMaxLength(3);
			entity.Property(e => e.CountryId).HasColumnName("CountryID");
			entity.Property(e => e.State1)
				.HasMaxLength(150)
				.HasColumnName("State");

			entity.HasOne(d => d.Country).WithMany(p => p.States)
				.HasForeignKey(d => new { d.CountryId, d.CountryCode })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_States_Countries");
		});

		modelBuilder.Entity<Unit>(entity =>
		{
			entity.HasKey(e => e.UnitCode).HasName("pk_Units");

			entity.HasIndex(e => e.Unit1, "uc_Units").IsUnique();

			entity.Property(e => e.UnitCode).HasMaxLength(3);
			entity.Property(e => e.Unit1)
				.HasMaxLength(50)
				.HasColumnName("Unit");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("pk_Users");

			entity.Property(e => e.UserId).HasColumnName("UserID");
			entity.Property(e => e.AuthCookie).HasMaxLength(400);
			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.FirstName).HasMaxLength(150);
			entity.Property(e => e.LastLogin).HasColumnType("datetime");
			entity.Property(e => e.LastName).HasMaxLength(150);
			entity.Property(e => e.Mail).HasMaxLength(300);
			entity.Property(e => e.Password).HasMaxLength(700);
			entity.Property(e => e.PostTitle).HasMaxLength(50);
			entity.Property(e => e.PreTitle).HasMaxLength(50);
			entity.Property(e => e.Tel).HasMaxLength(30);
			entity.Property(e => e.UserName).HasMaxLength(50);

			entity.HasOne(d => d.CompCodeNavigation).WithMany(p => p.Users)
				.HasForeignKey(d => d.CompCode)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Users_Companies");

			entity.HasOne(d => d.RoleNrNavigation).WithMany(p => p.Users)
				.HasForeignKey(d => d.RoleNr)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_Users_Roles");
		});

		modelBuilder.Entity<UserDiscount>(entity =>
		{
			entity.HasKey(e => e.UserDiscountId).HasName("pk_UserDiscounts");

			entity.Property(e => e.UserDiscountId).HasColumnName("UserDiscountID");
			entity.Property(e => e.CompCode).HasMaxLength(6);
			entity.Property(e => e.Discount).HasColumnType("decimal(3, 2)");
			entity.Property(e => e.UserId).HasColumnName("UserID");

			entity.HasOne(d => d.User).WithMany(p => p.UserDiscounts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_UserDiscounts_Users");

			entity.HasOne(d => d.Article).WithMany(p => p.UserDiscounts)
				.HasForeignKey(d => new { d.ArticleNr, d.ProductNr, d.ProductGroupNr, d.CompCode })
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_UserDiscounts_Articles");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
