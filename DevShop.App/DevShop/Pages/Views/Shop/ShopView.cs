using DevShop.Data.Models;
using DevShop.Data.ViewModels.ShopArticles;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Shop
{
	/// <summary>
	/// Class for the shop-view.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class ShopView
	{
		#region Variables/Properties
		// Determines, whether an articles' details are viewed or not
		private bool showArtDetails;

		// HTML-Code for the menu (= tree) of the shop
		private MarkupString categoryTree;

		private Category category;

		private List<ArticleDetailedVM> shopArticles;



		// ID of the category, the articles of which should be displayed
		[Parameter]
		public string CategoryID { get; set; }

		// Code of the company to display the details of an article
		[Parameter]
		public string CompCode { get; set; }

		// Nr of the product-group to display the details of an article
		[Parameter]
		public string ProductGroupNr { get; set; }

		// Nr of the product to display the details of an article
		[Parameter]
		public string ProductNr { get; set; }

		// Nr of the article, whose details should be displayed
		[Parameter]
		public string ArticleNr { get; set; }
		#endregion



		#region Main Methods
		/// <summary>
		/// Called, when the parameters of the URL change.
		/// This is needed instead of the OnInitializedAsync, because when the user selects a different category from the menu or wants to see the details of an article (which changes the parameters in the url),
		/// the view has to update.
		/// </summary>
		protected override async Task OnParametersSetAsync()
		{
			// Display all articles that are part of the selected category or all sub-categories
			if (!string.IsNullOrEmpty(CategoryID))
			{
				showArtDetails = false;

				// Information of the selected category
				category = await uow.CategoryRepo.GetModelByPkAsync(Convert.ToInt32(CategoryID));

				// Get all sub-categories of the selected category
				List<Category> childCategories = await uow.CategoryRepo.GetChildrenAsync(Convert.ToInt32(CategoryID), true);
				// Get all articles, that are associated with the selected category or any sub-category
				shopArticles = await uow.ArticleRepo.GetCategoryArticlesAsync(childCategories);


				// Only create the tree, if it has not been done yet (to reduce traffic)
				if (string.IsNullOrEmpty(categoryTree.Value))
				{
					await GenerateTreeView();
				}
			}
			// Display the details of an article
			else if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr) && !string.IsNullOrEmpty(ProductNr) && !string.IsNullOrEmpty(ArticleNr))
			{
				showArtDetails = true;


				// Only create the tree, if it has not been done yet (to reduce traffic)
				if (string.IsNullOrEmpty(categoryTree.Value))
				{
					await GenerateTreeView();
				}
			}
			// Nothing has been selected -> return to entry page
			else
			{
				nav.NavigateTo("/");
			}
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// Generate the HTML for the menu (tree) of the shop
		/// </summary>
		private async Task GenerateTreeView()
		{
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
