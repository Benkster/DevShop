using DevShop.Data.Models;
using DevShop.Data.ViewModels.ShopArticles;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

		// Either the selected category in the menu, or the category associated with the selected article (if no category was selected)
		private Category category;
		// The article, that the user selected to see its details
		private ArticleDetailedVM selArticle;

		// List of all articles, that are beeing displayed in the view
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

				selArticle = new ArticleDetailedVM();

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


				StateHasChanged();
			}
			// Display the details of an article
			else if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr) && !string.IsNullOrEmpty(ProductNr) && !string.IsNullOrEmpty(ArticleNr))
			{
				showArtDetails = true;

				// Get detailed information about the selected article
				selArticle = await uow.ArticleRepo.GetViewModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));


				
				// The user never selected a category -> clicked on one of the random articles on the entry-page (index) of the website
                if (category == null)
                {
					// Get the category, that is associated with the selected article
					category = await uow.ArticleRepo.GetCategoryOfArticleAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));
				}


				// Get all sub-categories of the category
				List<Category> childCategories = await uow.CategoryRepo.GetChildrenAsync(category.CategoryId, true);
				// Get all articles, that are associated with the category or any sub-category
				shopArticles = await uow.ArticleRepo.GetCategoryArticlesAsync(childCategories);


				// Only create the tree, if it has not been done yet (to reduce traffic)
				if (string.IsNullOrEmpty(categoryTree.Value))
				{
					await GenerateTreeView();
				}


				StateHasChanged();
			}
			// Nothing has been selected -> return to entry page
			else
			{
				nav.NavigateTo("/");
			}
		}



		/// <summary>
		/// Invoked after rendering the page.
		/// Is necessary to close the category-menu (tree-view), as it would stay open after selecting a different category otherwise
		/// </summary>
		/// <param name="firstRender">
		/// Indicates, whether the page is beeing rendered for the first time or not
		/// </param>
		protected override async Task OnAfterRenderAsync(bool firstRender)
        {
			// Import the JS-File of the shop to be able to access its functions
			var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Views/Shop/ShopView.razor.js");


			// Call a JS-Function, that closes the menu
			await module.InvokeVoidAsync("CloseTreeView");
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
