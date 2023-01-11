using DevShop.Data.Models;
using DevShop.Data.ViewModels.ShopArticles;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Transactions;

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

		// The users search-text will be stored here
		private string searchString;

		// The page, that the user is currently on (page 1, 2, ...)
		private int currentPage;
		// Total amount of all articles, that have been found
		private int artCount;
		// The last page, that the user can view (= total amoung of pages)
		private int maxPage;

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

		[Parameter]
		public string SearchString { get; set; }
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
				currentPage = 1;

				showArtDetails = false;
				searchString = string.Empty;


				await GetArticles();


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
				currentPage = (currentPage > 0) ? currentPage : 1;
				showArtDetails = true;


				await GetArticles();


				// Only create the tree, if it has not been done yet (to reduce traffic)
				if (string.IsNullOrEmpty(categoryTree.Value))
				{
					await GenerateTreeView();
				}


				StateHasChanged();
			}
			// The user used the search-field on the entry-page of the website (index)
			else if (!string.IsNullOrEmpty(SearchString))
			{
				currentPage = 1;
				searchString = SearchString;

				await Search(currentPage, true);


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


			maxPage = Convert.ToInt32(Math.Ceiling((decimal)artCount / 20));
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
			await module.InvokeVoidAsync("ResetPage");
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



		/// <summary>
		/// Load all articles
		/// </summary>
		private async Task GetArticles()
		{
			// Load all articles that are part of the selected category or all sub-categories
			if (!showArtDetails)
			{
				selArticle = new ArticleDetailedVM();

				// Information of the selected category
				category = await uow.CategoryRepo.GetModelByPkAsync(Convert.ToInt32(CategoryID));

				// Get all sub-categories of the selected category
				List<Category> childCategories = await uow.CategoryRepo.GetChildrenAsync(Convert.ToInt32(CategoryID), true);
				// Get all articles, that are associated with the selected category or any sub-category
				shopArticles = await uow.ArticleRepo.GetCategoryArticlesAsync(childCategories, currentPage);

				artCount = await uow.ArticleRepo.GetTotalArticleAmount(childCategories);
			}
			// Load the details of an article
			else
			{
				// Get detailed information about the selected article
				selArticle = await uow.ArticleRepo.GetViewModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));



				// The user never selected a category -> clicked on one of the random articles on the entry-page (index) of the website
				if (category == null)
				{
					// Get the category, that is associated with the selected article
					category = await uow.ArticleRepo.GetCategoryOfArticleAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));
				}


				if (string.IsNullOrEmpty(searchString))
				{
					// Get all sub-categories of the category
					List<Category> childCategories = await uow.CategoryRepo.GetChildrenAsync(category.CategoryId, true);
					// Get all articles, that are associated with the category or any sub-category
					shopArticles = await uow.ArticleRepo.GetCategoryArticlesAsync(childCategories, currentPage);

					artCount = await uow.ArticleRepo.GetTotalArticleAmount(childCategories);
				}
			}
		}



		/// <summary>
		/// Search for articles
		/// </summary>
		/// <param name="_pageNumber">
		/// Number of the current page
		/// </param>
		/// <param name="_initialPageNr">
		/// If true, the page-number will be reset to 1
		/// </param>
		private async Task Search(int _pageNumber, bool _initialPageNr = true)
        {
			// The user has typed in a search-text
			if (!string.IsNullOrEmpty(searchString))
            {
				// Reset the page-number
				if (_initialPageNr)
				{
					_pageNumber = 1;
					currentPage = 1;
				}


				shopArticles = await uow.ArticleRepo.SearchArticleAsync(searchString, _pageNumber);
				artCount = await uow.ArticleRepo.GetTotalArticleAmount(searchString);

				maxPage = Convert.ToInt32(Math.Ceiling((decimal)artCount / 20));


				StateHasChanged();
			}
			// The search-field is empty
			else
			{
				await OnParametersSetAsync();
			}
        }



		/// <summary>
		/// Set a cookie in order to select the list-view or box-view, if the page reloads
		/// </summary>
		/// <param name="_cookieValue">
		/// Determines, whether the list-view or the box-view should be selected
		/// </param>
		private async Task SetCookie(string _cookieValue)
		{
			// Import the JS-file of the shop to be able to access its functions
			var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Views/Shop/ShopView.razor.js");


			// Call the function in the JS-file, that sets the cookie
			await module.InvokeVoidAsync("SetCookie", new string[] { _cookieValue });
		}



		/// <summary>
		/// Get the next 20 available articles
		/// </summary>
		private async Task NextPage()
		{
			currentPage++;


			// Get the next page of the search-results
			if (!string.IsNullOrEmpty(searchString))
			{
				await Search(currentPage, false);
			}
			// Get the next page of articles
			else
			{
				await GetArticles();
			}
		}



		/// <summary>
		/// Get the previous 20 available articles
		/// </summary>
		private async Task PrevPage()
		{
			currentPage--;


			// Get the previous page of the search-results
			if (!string.IsNullOrEmpty(searchString))
			{
				await Search(currentPage, false);
			}
			// Get the previous page of articles
			else
			{
				await GetArticles();
			}
		}
		#endregion
	}
}
