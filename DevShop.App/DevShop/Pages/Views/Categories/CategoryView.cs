using DevShop.Data.Models;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;

namespace DevShop.Pages.Views.Categories
{
    /// <summary>
    /// Class for the Category-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class CategoryView
    {
        #region Variables
        // Holds the HTML-Code for the TreeView
        MarkupString treeViewMarkup;
        #endregion



        #region Methods
        /// <summary>
        /// Called automatically, when the page loads.
        /// Sets variables needed for the view.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // List of all categories
            List<Category> categories = await uow.CategoryRepo.GetAllModelsAsync();

            // Convert the list of categories to TreeView-elements
            List<TreeViewElementVM> categoriesTV = categories.Select(c => new TreeViewElementVM()
            {
                ElemID = c.CategoryId,
                ElemText = c.CategoryName,
                ParentID = c.ParentId,
                ElemLink = "./category/edit/" + c.CategoryId.ToString()
            }).ToList();


            // Get the HTML-Code for the TreeView
            treeViewMarkup = treeBuilder.BuildTree(categoriesTV);
        }
        #endregion
    }
}