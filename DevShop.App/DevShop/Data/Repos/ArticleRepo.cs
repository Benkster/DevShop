using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using DevShop.Data.ViewModels.ShopArticles;
using Microsoft.EntityFrameworkCore;

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
