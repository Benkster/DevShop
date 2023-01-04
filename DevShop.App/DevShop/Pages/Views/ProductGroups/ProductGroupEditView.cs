using DevShop.Data.Helpers;
using DevShop.Data.Models;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.ProductGroups
{
    /// <summary>
    /// Class for the ProductGroup-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class ProductGroupEditView
    {
        #region Variables/Properties
        // Determines, whether an existing model is beeing edited or a new one is beeing created
        private bool isEdit = false;

        // Parent-ID of the model (from select-element)
        private int parentID = 0;
        private int nextPK = 0;

        // Displays an error message, if something went wrong
        private string errorMessage = string.Empty;

        // Holds the HTML-Code for the TreeView
        MarkupString treeViewMarkup;

        // Used as a model for the edit-form
        private ProductGroup productGroup;
        // The category, that is assigned to the product-group
        private Category selCategory;
        // The company, to which the product-group, that is beeing created, belongs
        private Company selCompany;

        // List of product-gropus, that are allowed to be selected as parent-element
        private List<ProductGroup> formGroups;
        // List of all companies
        private List<Company> companies;
        // List of all categories
        private List<Category> categories;



        // Code of the company, wo which the product-group, that is beeing edited, belongs
		[Parameter]
		public string CompCode { get; set; }

		// ID of the product-group, that is beeing edited
		[Parameter]
        public string ProductGroupNr { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of the OnInitializedAsync, because when the user selects something in the TreeView (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all companies from the DB and sort them by code and name
            companies = await uow.CompanyRepo.GetAllModelsAsync(true);
            companies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).ToList();

            // Get all categories from the DB and sort them by name
            categories = await uow.CategoryRepo.GetAllModelsAsync();
            categories = categories.OrderBy(c => c.CategoryName).ToList();


            // An existing product-gropu is beeing edited
            if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr))
            {
                isEdit = true;
                // Get the product-group, that is beeing edited
                productGroup = await uow.ProductGroupRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr));
                // Get all product-groups, that can be selected as parent-elements
                formGroups = await uow.ProductGroupRepo.GetModelsWithoutChildrenAsync(Convert.ToInt32(ProductGroupNr), CompCode);

                if (productGroup.ParentId != null && productGroup.ParentId > 0)
                {
                    ProductGroup groupParent = await uow.ProductGroupRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(productGroup.ParentId));
                    categories = await uow.CategoryRepo.GetChildrenAsync(groupParent.CategoryId, true);
                }

                // Get the category of the current product-group
                selCategory = await uow.CategoryRepo.GetModelByPkAsync(productGroup.CategoryId);

                // Get the next available PK for the product-group
                nextPK = await uow.ProductGroupRepo.GetNextPkAsync(CompCode);
            }
            // A new product-group is beeing created
            else
            {
                isEdit = false;
                productGroup = new ProductGroup();

                selCategory = categories.FirstOrDefault();
                selCompany = companies.FirstOrDefault();


                if (selCompany != null)
                {
                    // Get the next available PK for the new product-group
                    nextPK = await uow.ProductGroupRepo.GetNextPkAsync(selCompany.CompCode);
                    // Get all product-groups, that can be selected as parent-elements
                    formGroups = await uow.ProductGroupRepo.GetAllModelsAsync(selCompany.CompCode);
                }
				else
				{
                    formGroups = await uow.ProductGroupRepo.GetAllModelsAsync();
                }
            }


            // Only genereate the TreeView, if it has not yet been done (=> minimize traffic and loading time when changing the selected product-group in the TreeView)
            if (string.IsNullOrEmpty(treeViewMarkup.Value))
            {
                GenerateTreeView();
            }


            // Update the data on the view
            StateHasChanged();
        }



        /// <summary>
        /// Create a new product-group or update an existing one
        /// </summary>
        private async Task Save()
		{
            // Set the parent-element and category
            productGroup.ParentId = parentID;
            productGroup.CategoryId = selCategory.CategoryId;
            productGroup.Category = selCategory;


            // Update an existing product-group
            if (isEdit)
			{
                await uow.ProductGroupRepo.UpdateModelAsync(productGroup);

                nav.NavigateTo("/product-group/" + productGroup.CompCode);
			}
            // Create a new product-group
			else
			{
                productGroup.ProductGroupNr = nextPK;
                productGroup.CompCode = selCompany.CompCode;
                await uow.ProductGroupRepo.CreateModelAsync(productGroup);

                nav.NavigateTo("/product-group/" + selCompany.CompCode);
			}
		}



        /// <summary>
        /// Delete a product-group from the database
        /// </summary>
        private async Task Delete()
		{
            // Only proceed, if an existing product-group is beeing edited
            if (isEdit)
            {
                // All existing product-groups
                List<ProductGroup> allGroups = await uow.ProductGroupRepo.GetAllModelsAsync();
                // All existing products
                List<Product> products = await uow.ProductRepo.GetAllModelsAsync(CompCode, Convert.ToInt32(ProductGroupNr));


                // If the product-group has one or more sub-groups or one or more products are assigned to it, the user is not allowed to delete the group
                if (allGroups.Any(g => g.ParentId == productGroup.ProductGroupNr) || products.Any())
				{
                    errorMessage = "This Product-Group is either assigned as Parent-Element for another Product-Group, or has one or more Products assigned to it, and can therefor not be deleted.";
				}
                // No sub-group or products are assigned to the product-group => delete it
                else
				{
                    await uow.ProductGroupRepo.DeleteModelAsync(CompCode, Convert.ToInt32(ProductGroupNr));

                    nav.NavigateTo("/product-group/" + CompCode);
				}
            }
		}



        /// <summary>
        /// Display an error, if the submition of the form was invalid
        /// </summary>
        private void Error()
		{
            errorMessage = "Something went wrong. Please try again.";
		}
        #endregion



        #region Side Methods
        /// <summary>
        /// Get the HTML-Code for the TreeView
        /// </summary>
        private async void GenerateTreeView()
        {
            List<ProductGroup> productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);
            List<Product> products = await uow.ProductRepo.GetAllModelsAsync(CompCode);
            List<Article> articles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);

            List<TreeViewElementVM> treeViewElements = ConcatArticleTree.ConcatFromLists(productGroups, products, articles);


            // Get the HTML-Code for the TreeView
            treeViewMarkup = treeBuilder.BuildTree(treeViewElements);

            // Update the data on the view
            StateHasChanged();
        }



        /// <summary>
        /// If the user changes the selected company, return to the overview and select the company in overview
        /// </summary>
        private void ChangeCompany(ChangeEventArgs args)
		{
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
			{
                nav.NavigateTo("/product-group/" + args.Value.ToString());
			}
		}



        /// <summary>
        /// Change the company, to which the product-group belongs
        /// </summary>
        private async Task ChangeAssignedCompany(ChangeEventArgs args)
		{
            selCompany = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());
            nextPK = await uow.ProductGroupRepo.GetNextPkAsync(args.Value.ToString());
            formGroups = await uow.ProductGroupRepo.GetAllModelsAsync(args.Value.ToString());
        }



        /// <summary>
        /// Change the selected category, that is beeing assigned to the product-group
        /// </summary>
        private async Task ChangeCategory(ChangeEventArgs args)
		{
            if (!string.IsNullOrEmpty(args.Value.ToString()))
			{
                selCategory = await uow.CategoryRepo.GetModelByPkAsync(Convert.ToInt32(args.Value));
			}
		}



        /// <summary>
        /// If the user changes the parent-element, update the categories, that are allowed to be selected
        /// </summary>
        private async void ChangeParentElem(ChangeEventArgs args)
		{
            // ID of the new parent-element
            parentID = Convert.ToInt32(args.Value);


            // The user selected an existing product-group
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                // Get the code of the company, to which the product-group belongs
                string _compCode = (selCompany != null) ? selCompany.CompCode : CompCode;
                // Get the parent-element of the product group
                ProductGroup parentGroup = await uow.ProductGroupRepo.GetModelByPkAsync(_compCode, Convert.ToInt32(args.Value));

                // Get all categories, that the user is allowed to select (i.e. all child-categories of the parent-elements category)
                categories = await uow.CategoryRepo.GetChildrenAsync(parentGroup.CategoryId, true);
                selCategory = categories.FirstOrDefault();
            }
            // The user selected "root" as parent-element
			else
			{
                // Get all existing categories
                categories = await uow.CategoryRepo.GetAllModelsAsync();
                categories = categories.OrderBy(c => c.CategoryName).ToList();

                selCategory = categories.FirstOrDefault();
            }


            // Update the data on the view
            StateHasChanged();
		}
        #endregion
    }
}
