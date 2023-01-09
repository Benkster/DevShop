using DevShop.Data.Models;
using DevShop.Data.ViewModels.ShopArticles;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages
{
	/// <summary>
	/// Class for the entry-page of the shop.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class Index
	{
		#region Variables
		private MarkupString categoryTree;

		// List of random articles, that are displayed in the view
		private List<ArticleSmallVM> randomArticles;
		#endregion



		#region Main Methods
		/// <summary>
		/// Called automatically when the page loads.
		/// Sets all variables that are used in the view
		/// </summary>
		protected override async Task OnInitializedAsync()
		{
			// Get 10 random articles from the database
			randomArticles = await uow.ArticleRepo.GetRandomModelsAsync(10);


			List<Category> categories = await uow.CategoryRepo.GetAllModelsAsync();
			List<TreeViewElementVM> tvCategories = categories.Select(c => new TreeViewElementVM()
			{
				ElemID = c.CategoryId,
				ElemText = c.CategoryName,
				ParentID = c.ParentId,
				ElemLink = "./shop/" + c.CategoryId.ToString(),
				SortNr = c.ParentId
			}).ToList();


			categoryTree = treeBuilder.BuildTree(tvCategories);
		}
		#endregion
	}
}
