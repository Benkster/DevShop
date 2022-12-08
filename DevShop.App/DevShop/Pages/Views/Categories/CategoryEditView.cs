using DevShop.Data.Models;
using DevShop.Data.SortTypes;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using System.Drawing.Text;

namespace DevShop.Pages.Views.Categories
{
    /// <summary>
    /// Class for the Category-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class CategoryEditView
    {
        #region Variables/Properties
        // Determines, whether an existing model is beeing edited or a new one is beeing created
        private bool isEdit = false;

        // Parent-ID of the model (from select-element)
        private int parentID = 0;

        // Displays an error message, if something went wrong
        private string errorMessage = string.Empty;

        // Holds the HTML-Code for the TreeView
        MarkupString treeViewMarkup;

        // Used as a model for the edit-form
        private Category category;

        // List of categories, that are allowed to be selected as parent-element
        private List<Category> formCategories;



        // ID of the category, that is beeing edited
        [Parameter]
        public string CategoryID { get; set; }
		#endregion



		#region Methods
		#region Main-Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of the OnInitializedAsync, because when the user selects something in the TreeView (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
		protected override async Task OnParametersSetAsync()
        {
            // An existing category is beeing edited
            if (!string.IsNullOrEmpty(CategoryID))
            {
                isEdit = true;
                category = await uow.CategoryRepo.GetModelByPkAsync(Convert.ToInt32(CategoryID));
                formCategories = await uow.CategoryRepo.GetModelsWithoutChildrenAsync(Convert.ToInt32(CategoryID));
            }
            // A new category is beeing created
            else
            {
                category = new Category();
                formCategories = await uow.CategoryRepo.GetAllModelsAsync(CategorySortType.SortType.Name);
            }


            // Only genereate the TreeView, if it has not yet been done (=> minimize traffic and loading time when changing the selected category in the TreeView)
            if (string.IsNullOrEmpty(treeViewMarkup.Value))
            {
                GenerateTreeView();
            }


            // Update the data on the view
            StateHasChanged();
		}



        /// <summary>
        /// Saves a new category or updates an existing one
        /// </summary>
        private async Task Save()
        {
            // Set the selected parent-element
            category.ParentId = parentID;


            // Update an existing category
            if (isEdit)
            {
                await uow.CategoryRepo.UpdateModelAsync(category);

                // Parent-element might have changed, so the TreeView must be updated
                GenerateTreeView();
            }
            // Create a new category
            else
            {
                await uow.CategoryRepo.CreateModelAsync(category);
                nav.NavigateTo("/category");
            }
        }



        /// <summary>
        /// Delete a category from the database
        /// </summary>
        /// <returns></returns>
        private async Task Delete()
		{
            // Get all existing categories
            List<Category> allCategories = await uow.CategoryRepo.GetAllModelsAsync();


            // If the category, that should be deleted, has any child-elements, the user is not allowed to delete it
            if (allCategories.Any(c => c.ParentId == category.CategoryId))
			{
                errorMessage = "This element has one or more child-elements attached to it. Please delete all child-elements first in order to delete this element.";
			}
            // The category has no child-elements => delete it
			else
			{
                await uow.CategoryRepo.DeleteModelAsync(Convert.ToInt32(CategoryID));
                nav.NavigateTo("/category");
			}
		}



        /// <summary>
        /// Show an error message, if the form-submit was invalid
        /// </summary>
        private async Task Error()
        {
            errorMessage = "Something went wrong. Please try again";
        }
        #endregion



        #region Side-Methods
        /// <summary>
        /// Set the parent-element
        /// </summary>
        private void ChangeParentElem(ChangeEventArgs args)
        {
            parentID = Convert.ToInt32(args.Value);
        }



        /// <summary>
        /// Get the HTML-Code for the TreeView
        /// </summary>
        private async void GenerateTreeView()
        {
            // List of all categories
            List<Category> treeCategories = await uow.CategoryRepo.GetAllModelsAsync();

            // Convert the list of categories to TreeView-elements
            List<TreeViewElementVM> treeViewElements = treeCategories.Select(c => new TreeViewElementVM()
            {
                ElemID = c.CategoryId,
                ElemText = c.CategoryName,
                ParentID = c.ParentId,
                ElemLink = "./category/edit/" + c.CategoryId.ToString()
            }).ToList();


            // Get the HTML-Code for the TreeView
            treeViewMarkup = treeBuilder.BuildTree(treeViewElements);

            // Update the data on the view
            StateHasChanged();
        }
        #endregion
        #endregion
    }
}
