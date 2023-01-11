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

		string filePath;

		private string[] fileExt;
		#endregion



		#region Constructors
		public ArticleRepo(DevShopContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;

			fileExt = new string[] { "jpg", "jpeg", "gif", "png", "svg" };
			filePath = _env.ContentRootPath + @"wwwroot\pic\articles\";
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
		/// Get detailed information of all articles, that belong to a category or to any sub-category
		/// </summary>
		/// <param name="_categories">
		/// List of the category and sub-categories, whose articles should be selected
		/// </param>
		/// <param name="_pageNumber">
		/// Specifies the current page number (only 20 articles will be displayed per page)
		/// </param>
		/// <param name="_ignorePageNumber">
		/// If true, all articles are beeing selected, instead of just 20
		/// </param>
		/// <returns>
		/// A list of detailed information of articles
		/// </returns>
		public async Task<List<ArticleDetailedVM>> GetCategoryArticlesAsync(List<Category> _categories, int _pageNumber, bool _ignorePageNumber = false)
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



			// Get either all articles, or the max. 20 articles of the current page
			Range selArtRange = (_ignorePageNumber || _pageNumber <= 1) ? new Range(0, articles.Count) : new Range(20 * (_pageNumber - 1), 20 * _pageNumber);

			// Store all the information of each article in a list of view-models
			viewModels = articles.Select(a => new ArticleDetailedVM()
			{
				UniqueValue = a.CompCode + "_" + a.ArticleNr.ToString(),
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
				Price = Math.Round(a.Price, 2),
				Discount = (a.Discount != null) ? Math.Round(Convert.ToDecimal(a.Price - (a.Price * a.Discount)), 2) : 0,
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
				ProductGroupNr = a.ProductGroupNr,
				ProductNr = a.ProductNr,
				SortNr = a.SortNr,
				UnitAmount = a.UnitAmount,
				CategoryID = productGroups.FirstOrDefault(pg => pg.ProductGroupNr == a.ProductGroupNr && pg.CompCode == a.CompCode).CategoryId,
				CategoryName = _categories.FirstOrDefault(c => c.CategoryId == productGroups.FirstOrDefault(pg => pg.ProductGroupNr == a.ProductGroupNr && pg.CompCode == a.CompCode).CategoryId).CategoryName,
				PicSource = "/pic/icon_no-pic.svg",
				PicExists = false,
				Link = "./shop/" + a.CompCode + "/" + a.ProductGroupNr.ToString() + "/" + a.ProductNr.ToString() + "/" + a.ArticleNr.ToString()
			}).Take(selArtRange).OrderBy(a => a.SortNr).ToList();



			int index = 0;

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

						break;
					}
				}

				index++;
			}



			return viewModels;
		}



		/// <summary>
		/// Get a list of Articles, that match a given search-string
		/// </summary>
		/// <param name="_searchString">
		/// The search-text
		/// </param>
		/// <param name="_pageNumber">
		/// Specifies the current page number (only 20 articles will be displayed per page)
		/// </param>
		/// <param name="_ignorePageNumber">
		/// If true, all articles are beeing selected, instead of just 20
		/// </param>
		/// <returns>
		/// A detailed list of Articles
		/// </returns>
		public async Task<List<ArticleDetailedVM>> SearchArticleAsync(string _searchString, int _pageNumber, bool _ignorePageNumber = false)
		{
			// List of detailed information of all articles, that should be selected
			List<ArticleDetailedVM> viewModels = new List<ArticleDetailedVM>();

			// Get all articles, that match the given search-string
			List<Article> articles = await _context.Articles.Where(a => 
					a.ArticleName.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.ArticleDescription) && a.ArticleDescription.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.Ean) && a.Ean.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.ArticleCode) && a.ArticleCode.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F1) && a.F1.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F2) && a.F2.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F3) && a.F3.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F4) && a.F4.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F5) && a.F5.Contains(_searchString) ||
					!string.IsNullOrEmpty(a.F6) && a.F6.Contains(_searchString)
				).ToListAsync();

			List<Unit> units = await _context.Units.ToListAsync();
			List<Company> companies = new List<Company>();
			List<ArticleHeader> artHeaders = new List<ArticleHeader>();
			List<ProductGroup> productGroups = new List<ProductGroup>();
			List<Category> categories = new List<Category>();



			// Distinct list of the codes of each company, that sell the articles
			List<string> compCodes = articles.GroupBy(a => a.CompCode).Select(a => a.First().CompCode).ToList();

			// Get all companies, that sell the articles
			foreach (string _compCode in compCodes)
            {
				Company currentComp = await _context.Companies.FirstOrDefaultAsync(c => c.CompCode == _compCode);


				// Distinct lits of all product-group-numbers, to which each article of the current company belongs
				List<int> compGroupNrs = articles.Where(a => a.CompCode == _compCode).GroupBy(a => a.ProductGroupNr).Select(a => a.First().ProductGroupNr).ToList();

				// Get all product-groups
				foreach (int _compGroupNr in compGroupNrs)
				{
					ProductGroup currentGroup = await _context.ProductGroups.FirstOrDefaultAsync(pg => pg.ProductGroupNr == _compGroupNr && pg.CompCode == _compCode);

					productGroups.Add(currentGroup);
				}


				companies.Add(currentComp);
            }


			// Distinct list of the IDs of the headers, that are used for the articles
			List<int> artHeaderIDs = articles.GroupBy(a => a.ArticleHeaderId).Select(a => a.First().ArticleHeaderId).ToList();

			// Get all headers, that are used for the articles
			foreach (int _artHeaderID in artHeaderIDs)
            {
				ArticleHeader currentHeader = await _context.ArticleHeaders.FirstOrDefaultAsync(ah => ah.ArticleHeaderId == _artHeaderID);

				artHeaders.Add(currentHeader);
            }


			// Distinct list of the IDs of the categories, with which the articles are associated
			List<int> categoryIDs = productGroups.GroupBy(pg => pg.CategoryId).Select(pg => pg.First().CategoryId).ToList();

			// Get all categories
			foreach (int _categoryID in categoryIDs)
			{
				Category currentCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categoryID);

				categories.Add(currentCategory);
			}


			// Get either all articles, or the max. 20 articles of the current page
			Range selArtRange = (_ignorePageNumber) ? new Range(0, articles.Count) : new Range(20 * (_pageNumber - 1), 20 * _pageNumber);

			// Store all the information of each article in a list of view-models
			viewModels = articles.Select(a => new ArticleDetailedVM()
			{
				UniqueValue = a.CompCode + "_" + a.ArticleNr.ToString(),
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
				Price = Math.Round(a.Price, 2),
				Discount = (a.Discount != null) ? Math.Round(Convert.ToDecimal(a.Price - (a.Price * a.Discount)), 2) : 0,
				Ean = a.Ean,
				F1 = a.F1,
				F1Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F1name,
				F2 = a.F2,
				F2Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F2name,
				F3 = a.F3,
				F3Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F3name,
				F4 = a.F4,
				F4Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F4name,
				F5 = a.F5,
				F5Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F5name,
				F6 = a.F6,
				F6Name = artHeaders.FirstOrDefault(ah => ah.ArticleHeaderId == a.ArticleHeaderId).F6name,
				ProductGroupNr = a.ProductGroupNr,
				ProductNr = a.ProductNr,
				SortNr = a.SortNr,
				UnitAmount = a.UnitAmount,
				CategoryID = productGroups.FirstOrDefault(pg => pg.CompCode == a.CompCode && pg.ProductGroupNr == a.ProductGroupNr).CategoryId,
				CategoryName = categories.FirstOrDefault(c => c.CategoryId == productGroups.FirstOrDefault(pg => pg.CompCode == a.CompCode && pg.ProductGroupNr == a.ProductGroupNr).CategoryId).CategoryName,
				PicSource = "/pic/icon_no-pic.svg",
				PicExists = false,
				Link = "./shop/" + a.CompCode + "/" + a.ProductGroupNr.ToString() + "/" + a.ProductNr.ToString() + "/" + a.ArticleNr.ToString()
			}).Take(selArtRange).OrderBy(a => a.SortNr).ToList();



			int index = 0;

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

						break;
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
		/// Get detailed information of an Article
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
		/// Detailed information of an Article
		/// </returns>
		public async Task<ArticleDetailedVM> GetViewModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr, int _artNr)
        {
			// Get the article, whose details should be selected
			Article article = await _context.Articles.FirstOrDefaultAsync(a => a.CompCode == _compCode && a.ProductGroupNr == _prodGroupNr && a.ProductNr == _prodNr && a.ArticleNr == _artNr);
			// Get the category, that is associated with the article
			Category category = await GetCategoryOfArticleAsync(_compCode, _prodGroupNr, _prodNr, _artNr);
			// Get the billing- and packaging unit of the article
			Unit billingUnit = await _context.Units.FirstOrDefaultAsync(u => u.UnitCode == article.BillingUnit);
			Unit packagingUnit = await _context.Units.FirstOrDefaultAsync(u => u.UnitCode == article.PackagingUnit);
			// Get the company, to which the article belongs (producer of the article)
			Company company = await _context.Companies.FirstOrDefaultAsync(c => c.CompCode == article.CompCode);
			// Get the header of the article
			ArticleHeader artHeader = await _context.ArticleHeaders.FirstOrDefaultAsync(ah => ah.ArticleHeaderId == article.ArticleHeaderId && ah.CompCode == article.CompCode);


			// Store all the information in a view-model
			ArticleDetailedVM viewModel = new ArticleDetailedVM()
			{
				UniqueValue = article.CompCode + "_" + article.ArticleNr.ToString(),
				ArticleNr = article.ArticleNr,
				ArticleCode = article.ArticleCode,
				ArticleName = article.ArticleName,
				ArticleDescription = article.ArticleDescription,
				BillingUnitShort = billingUnit.UnitCode,
				BillingUnit = billingUnit.UnitName,
				PackagingUnitShort = packagingUnit.UnitCode,
				PackagingUnit = packagingUnit.UnitName,
				CompCode = article.CompCode,
				CompName = company.CompName,
				Price = Math.Round(article.Price, 2),
				Discount = (article.Discount != null) ? Math.Round(Convert.ToDecimal(article.Price - (article.Price * article.Discount)), 2) : 0,
				Ean = article.Ean,
				F1 = article.F1,
				F1Name = artHeader.F1name,
				F2 = article.F2,
				F2Name = artHeader.F2name,
				F3 = article.F3,
				F3Name = artHeader.F3name,
				F4 = article.F4,
				F4Name = artHeader.F4name,
				F5 = article.F5,
				F5Name = artHeader.F5name,
				F6 = article.F6,
				F6Name = artHeader.F6name,
				ProductGroupNr = article.ProductGroupNr,
				ProductNr = article.ProductNr,
				SortNr = article.SortNr,
				UnitAmount = article.UnitAmount,
				CategoryID = category.CategoryId,
				CategoryName = category.CategoryName,
				PicSource = "/pic/icon_no-pic.svg",
				PicExists = false,
				Link = "./shop/" + article.CompCode + "/" + article.ProductGroupNr.ToString() + "/" + article.ProductNr.ToString() + "/" + article.ArticleNr.ToString()
			};



			// Check, whether the article has a picture or not
			foreach (string _extension in fileExt)
			{
				// A picture for the article exists
				if (File.Exists(filePath + article.CompCode + @"\pic_" + article.ArticleNr.ToString() + "." + _extension))
                {
					// Save the source of the picture
					viewModel.PicSource = "/pic/articles/" + article.CompCode + "/pic_" + article.ArticleNr.ToString() + "." + _extension;
					viewModel.PicExists = true;
					break;
                }
            }



			return viewModel;
        }



		/// <summary>
		/// Get the Category of a given Article
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
		/// A single object of type Category
		/// </returns>
		public async Task<Category> GetCategoryOfArticleAsync(string _compCode, int _prodGroupNr, int _prodNr, int _artNr)
        {
			Article article = await GetModelByPkAsync(_compCode, _prodGroupNr, _prodNr, _artNr);

			// Get the product-group, to which the article belongs
			ProductGroup artGroup = await _context.ProductGroups.FirstOrDefaultAsync(pg => pg.ProductGroupNr == article.ProductGroupNr && pg.CompCode == article.CompCode);
			// Get the category, that is assigned to the product-group
			Category artCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == artGroup.CategoryId);


			return artCategory;
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



		/// <summary>
		/// Get the total amount of articles of the selected category
		/// </summary>
		/// <param name="_categories">
		/// List of the category and sub-categories, whose articles should be selected
		/// </param>
		/// <returns>
		/// The amount of articles
		/// </returns>
		public async Task<int> GetTotalArticleAmount(List<Category> _categories)
		{
			List<ArticleDetailedVM> allArticles = await GetCategoryArticlesAsync(_categories, 1, true);

			return allArticles.Count;
		}



		/// <summary>
		/// Get the total amount of articles that match the searched string
		/// </summary>
		/// <param name="_searchString">
		/// The search-text
		/// </param>
		/// <returns>
		/// The amount of articles
		/// </returns>
		public async Task<int> GetTotalArticleAmount(string _searchString)
		{
			List<ArticleDetailedVM> allArticles = await SearchArticleAsync(_searchString, 1, true);

			return allArticles.Count;
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
