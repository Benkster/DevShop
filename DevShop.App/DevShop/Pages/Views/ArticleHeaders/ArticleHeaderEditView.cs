using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.ArticleHeaders
{
	/// <summary>
	/// Class for the ArticleHeader-Edit-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class ArticleHeaderEditView
	{
		#region Variables/Properties
		// Displays an error message, if something went wrong
		private string errorMessage = string.Empty;

		// Specifies, whether a new header is beeing created or an existing one is beeing edited
		private bool isEdit = false;

		// Used as a model for the edit-form
		private ArticleHeader artHeader;



		// Code of the company, to which the header belongs
		[Parameter]
		public string CompCode { get; set; }

		// Number of the product-group, to which the header belongs
		[Parameter]
		public string ProductGroupNr { get; set; }

		// Number of the product, to which the header belongs
		[Parameter]
		public string ProductNr { get; set; }
		#endregion



		#region Main Methods
		/// <summary>
		/// Called automatically when the page loads.
		/// Sets all variables that are used in the view
		/// </summary>
		protected override async Task OnInitializedAsync()
		{
			// Only proceed, if all parameters are given
			if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr) && !string.IsNullOrEmpty(ProductNr))
			{
				// Gets the article-header of a given product, or creates a new one, if no header for this product has been created yet
				artHeader = await uow.ArticleHeaderRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));

				isEdit = (artHeader.ArticleHeaderId > 0) ? true : false;
			}
		}



		/// <summary>
		/// Create a new article-header/update an existing one
		/// </summary>
		private async Task Save()
		{
			// An existing header is beeing edited
			if (isEdit)
			{
				await uow.ArticleHeaderRepo.UpdateModelAsync(artHeader);

				errorMessage = "Changes have been saved.";
			}
			// A new header is beeing created
			else
			{
				artHeader.CompCode = CompCode;
				artHeader.ProductGroupNr = Convert.ToInt32(ProductGroupNr);
				artHeader.ProductNr = Convert.ToInt32(ProductNr);

				await uow.ArticleHeaderRepo.CreateModelAsync(artHeader);
				artHeader = await uow.ArticleHeaderRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));

				errorMessage = "Header created.";
			}


			StateHasChanged();
		}



		/// <summary>
		/// Delete the article-header
		/// </summary>
		private async Task Delete()
		{
			// Only delete it, if it exists
			if (isEdit)
			{
				List<Article> allArticles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);


				if (allArticles.Any(a => a.ArticleHeaderId == artHeader.ArticleHeaderId))
				{
					errorMessage = "Header can not be deleted, if one or more articles are assigned.";
				}
				else
				{
					await uow.ArticleHeaderRepo.DeleteModelAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));

					artHeader = new ArticleHeader();
					errorMessage = "Header deleted.";
				}

				StateHasChanged();
			}
		}



		/// <summary>
		/// Show an error-message, if the form submission was invalid
		/// </summary>
		private void Error()
		{
			errorMessage = "Something went wrong. Please try again.";
		}
		#endregion
	}
}
