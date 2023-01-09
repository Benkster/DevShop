using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using DevShop.Data.ViewModels.ShopArticles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Article
	/// </summary>
	public class ArticleRepo : IArticleRepo
	{
		#region Variables
		private readonly DevShopContext _context;

		private readonly IWebHostEnvironment _env;

		private string[] fileExt;
		#endregion



		#region Constructors
		public ArticleRepo(DevShopContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
			fileExt = new string[] { "jpg", "jpeg", "gif", "png", "svg" };
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Articles of a given Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belong
		/// </param>
		/// <returns>
		/// A list of objects of type Article
		/// </returns>
		public async Task<List<Article>> GetAllModelsAsync(string _compCode)
		{
			List<Article> models = await _context.Articles.Where(a => a.CompCode == _compCode).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get random Articles from the database
		/// </summary>
		/// <param name="_selAmount">
		/// The amount of random Articles to select
		/// </param>
		/// <returns>
		/// A list of random Articles
		/// </returns>
		public async Task<List<ArticleSmallVM>> GetRandomModelsAsync(int _selAmount)
		{
			Random rnd = new Random();

			// Get all articles from the database
			List<Article> allModels = await _context.Articles.ToListAsync();
			// Get the specified amount of random articles
			List<Article> randomModels = allModels.OrderBy(m => rnd.Next()).Take(_selAmount).ToList();


			// Location of the pictures of the articles
			string filePath = _env.ContentRootPath + @"wwwroot\pic\articles\";

			// Convert the list of articles to a list of view-models
			List<ArticleSmallVM> viewModels = randomModels.Select(m => new ArticleSmallVM()
			{
				ArticleNr = m.ArticleNr,
				ProductNr = m.ProductNr,
				ArticleName = m.ArticleName,
				ArticleCode = m.ArticleCode,
				CompCode = m.CompCode,
				Ean = m.Ean,
				Price = Math.Round(m.Price, 2),
				PicSource = "/pic/icon_no-pic.svg",
				Link = "./shop/" + m.CompCode + "/" + m.ProductGroupNr.ToString() + "/" + m.ProductNr.ToString() + "/" + m.ArticleNr.ToString()
			}).ToList();


			int index = 0;

			// Check whether the article has a picture or not
			foreach (ArticleSmallVM _model in viewModels)
            {
				foreach (string _extension in fileExt)
                {
					// A picture for the current article exists
					if (File.Exists(filePath + _model.CompCode + @"\pic_" + _model.ArticleNr.ToString() + "." + _extension))
                    {
						// Save the source of the picture
						viewModels[index].PicSource = "/pic/articles/" + _model.CompCode + "/pic_" + _model.ArticleNr.ToString() + "." + _extension;
						break;
                    }
                }

				index++;
            }


			return viewModels;
		}



		/// <summary>
		/// Get all articles, that belong to a category or to any sub-category
		/// </summary>
		/// <param name="_categories">
		/// List of the category and sub-categories, whose articles should be selected
		/// </param>
		/// <returns>
		/// A list of detailed information of articles
		/// </returns>
		public async Task<List<ArticleDetailedVM>> GetCategoryArticlesAsync(List<Category> _categories)
		{
			// List of detailed information of all articles, that should be selected
			List<ArticleDetailedVM> viewModels = new List<ArticleDetailedVM>();

			List<ProductGroup> productGroups = new List<ProductGroup>();
			List<Article> articles = new List<Article>();
			List<Company> companies = new List<Company>();
			List<Unit> units = await _context.Units.ToListAsync();
			List<ArticleHeader> articleHeaders = new List<ArticleHeader>();


			// Get all product-groups, that have the given category/sub-categories assigned to them
			foreach (Category _cat in _categories)
			{
				List<ProductGroup> catGroups = await _context.ProductGroups.Where(pg => pg.CategoryId == _cat.CategoryId).ToListAsync();

				productGroups.AddRange(catGroups);
			}


			// Get all articles, that belong to one of the product-groups which have the given category/sub-categories assigned to them (i.e. get all articles from the selected category and all sub-categories)
			foreach (ProductGroup _prodGroup in productGroups)
			{
				List<Article> currentGroupArticles = await _context.Articles.Where(a => a.ProductGroupNr == _prodGroup.ProductGroupNr && a.CompCode == _prodGroup.CompCode).ToListAsync();

				articles.AddRange(currentGroupArticles);
			}


			List<string> compCodes = articles.GroupBy(a => a.CompCode).Select(a => a.First().CompCode).ToList();

			// Get all companies, that sell the articles
			foreach (string _compCode in compCodes)
			{
				Company currentCompany = await _context.Companies.FirstOrDefaultAsync(c => c.CompCode == _compCode);

				if (currentCompany != null)
				{
					companies.Add(currentCompany);
				}
			}


			List<int> artHeaderIDs = articles.GroupBy(a => a.ArticleHeaderId).Select(a => a.First().ArticleHeaderId).ToList();

			// Get the information of all article-headers, that are used for the articles
			foreach (int _artHeaderID in artHeaderIDs)
			{
				ArticleHeader currentHeader = await _context.ArticleHeaders.FirstOrDefaultAsync(ah => ah.ArticleHeaderId == _artHeaderID);

				if (currentHeader != null)
				{
					articleHeaders.Add(currentHeader);
				}
			}


			// Store all the information of each article in a list of view-models
			viewModels = articles.Select(a => new ArticleDetailedVM()
			{
				ArticleNr = a.ArticleNr,
				ArticleCode = a.ArticleCode,
				ArticleName = a.ArticleName,
				ArticleDescription = a.ArticleDescription,
				BillingUnitShort = a.BillingUnit,
				BillingUnit = units.FirstOrDefault(u => u.UnitCode == a.BillingUnit).UnitName,
				PackagingUnitShort = a.PackagingUnit,
				PackagingUnit = units.FirstOrDefault(u => u.UnitCode == a.PackagingUnit).UnitName,
				CompCode = a.CompCode,
				CompName = companies.FirstOrDefault(c => c.CompCode == a.CompCode).CompName,
				Discount = a.Discount,
				Ean = a.Ean,
				F1 = a.F1,
				F1Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F1name,
				F2 = a.F2,
				F2Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F2name,
				F3 = a.F3,
				F3Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F3name,
				F4 = a.F4,
				F4Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F4name,
				F5 = a.F5,
				F5Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F5name,
				F6 = a.F6,
				F6Name = articleHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F6name,
				Price = a.Price,
				ProductGroupNr = a.ProductGroupNr,
				ProductNr = a.ProductNr,
				SortNr = a.SortNr,
				UnitAmount = a.UnitAmount,
				CategoryID = productGroups.FirstOrDefault(pg => pg.ProductGroupNr == a.ProductGroupNr && pg.CompCode == a.CompCode).CategoryId,
				CategoryName = _categories.FirstOrDefault(c => c.CategoryId == productGroups.FirstOrDefault(pg => pg.ProductGroupNr == a.ProductGroupNr && pg.CompCode == a.CompCode).CategoryId).CategoryName,
				PicSource = "/pic/icon_no-pic.svg",
				PicExists = false,
				Link = "./shop/" + a.CompCode + "/" + a.ProductGroupNr.ToString() + "/" + a.ProductNr.ToString() + "/" + a.ArticleNr.ToString()
			}).OrderBy(a => a.SortNr).ToList();



			int index = 0;

			// Location of the pictures of the articles
			string filePath = _env.ContentRootPath + @"wwwroot\pic\articles\";

			// Check whether the article has a picture or not
			foreach (ArticleDetailedVM _artDetail in viewModels)
			{
				foreach (string _extension in fileExt)
				{
					// A picture for the current article exists
					if (File.Exists(filePath + _artDetail.CompCode + @"\pic_" + _artDetail.ArticleNr.ToString() + "." + _extension))
					{
						// Save the source of the picture
						viewModels[index].PicSource = "/pic/articles/" + _artDetail.CompCode + "/pic_" + _artDetail.ArticleNr.ToString() + "." + _extension;
						viewModels[index].PicExists = true;
					}
				}

				index++;
			}



			return viewModels;
		}



		/// <summary>
		/// Get a single Article from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Article belongs
		/// </param>
		/// <param name="_prodNr">
		/// The number of the Product, to which the Article belongs
		/// </param>
		/// <param name="_artNr">
		/// The number of the Article
		/// </param>
		/// <returns>
		/// A single object of type Article
		/// </returns>
		public async Task<Article> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr, int _artNr)
		{
			Article model = await _context.Articles.FirstOrDefaultAsync(m => m.CompCode == _compCode && m.ProductGroupNr == _prodGroupNr && m.ProductNr == _prodNr && m.ArticleNr == _artNr);

			return model;
		}



		/// <summary>
		/// Get the next available number of an Article of a given Company
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <returns>
		/// An integer conatining the next available number for the Article
		/// </returns>
		public async Task<int> GetNextPkAsync(string _compCode)
		{
			// Indicates the next available PK
			int nextPk = 1;

			// Get the Article with the highest Art-Nr in the given Company
			Article model = await _context.Articles.Where(m => m.CompCode == _compCode).OrderByDescending(m => m.ArticleNr).FirstOrDefaultAsync();


			// Check, whether an Article exists for the given Company
			if (model != null)
			{
				// Count up the highest Art-Nr
				nextPk = model.ArticleNr + 1;
			}


			return nextPk;
		}
		#endregion



		#region Create/Update
		/// <summary>
		/// Create a new entry in the database
		/// </summary>
		/// <param name="_model">
		/// Data of the model
		/// </param>
		public async Task CreateModelAsync(Article _model)
		{
			_context.Articles.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Article _model)
		{
			_context.Articles.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Article belongs
		/// </param>
		/// <param name="_productNr">
		/// The number of the Product, to which the Article belongs
		/// </param>
		/// <param name="_articleNr">
		/// The number of the Article
		/// </param>
		public async Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _productNr, int _articleNr)
		{
			Article model = await GetModelByPkAsync(_compCode, _prodGroupNr, _productNr, _articleNr);

			if (model != null)
			{
				_context.Articles.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
