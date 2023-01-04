using DevShop.Data.Helpers;
using DevShop.Data.Models;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Articles
{
    /// <summary>
    /// Class for the Article-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class ArticleView
    {
        #region Variables/Properties
        // Prevent the list of companies from resetting after changing the selected company
        private bool changedCompany = false;

        // Holds the HTML-Code for the TreeView
        private MarkupString treeViewMarkup;

        // List of all companies, to which the Products belong
        private List<Company> companies;



        // Code of the company, that has been selected
        [Parameter]
        public string CompCode { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of the OnInitializedAsync, because when the user selects something that changes the parameters of ther URL, the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all companies from the DB and sort them by code and name
            companies = await uow.CompanyRepo.GetAllModelsAsync(true);
            companies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).ToList();


            // A company has been selected
            if (!string.IsNullOrEmpty(CompCode))
            {
                // Get all product-groups, that belong to the selected company
                List<ProductGroup> productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);
                // Get all products, that belong to the selected company
                List<Product> products = await uow.ProductRepo.GetAllModelsAsync(CompCode);
                // Get all articles, that belong to the selected company
                List<Article> articles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);

                // Concatenate the lists of product-groups, products and articles to a single list, that is used for building up the TreeView
                List<TreeViewElementVM> treeViewElements = ConcatArticleTree.ConcatFromLists(productGroups, products, articles);


                // Build the TreeView
                treeViewMarkup = treeBuilder.BuildTree(treeViewElements);
            }
            // No company has been selected
            else
            {
                // Clear the TreeView
                treeViewMarkup = new MarkupString();
            }


            // Update the data on the view
            StateHasChanged();
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// If a user changes the selected company, append the code of the company as a parameter to the URL
        /// </summary>
        private void ChangeCompany(ChangeEventArgs args)
        {
            // Prevent the list of companies from resetting
            changedCompany = true;


            // Only proceed, if an existing company has been selected ("0" => "Please select an article")
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                nav.NavigateTo("/article/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
