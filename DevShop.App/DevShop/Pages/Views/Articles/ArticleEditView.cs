using DevShop.Data.Helpers;
using DevShop.Data.Models;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Articles
{
    /// <summary>
    /// Class for the Article-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class ArticleEditView
    {
        #region Variables/Properties
        // Determines, whether an existing model is beeing edited or a new one is beeing created
        private bool isEdit = false;

        private int nextPK = 0;

        // Displays an error message, if something went wrong
        private string errorMessage = string.Empty;

        // Holds the HTML-Code for the TreeView
        MarkupString treeViewMarkup;

        // Used as a model for the edit-form
        private Article article;
        // The product, to which the article belongs
        private Product selProduct;
        // The Product-Group, to which the product belongs
        private ProductGroup selProdGroup;
        // The company, to which the product, that is beeing created, belongs
        private Company selCompany;
        // The Billing-Unit for the article, that has been selected
        private Unit selBillingUnit;
        // The Packaging-Unit for the article, that has been selected
        private Unit selPackagingUnit;
        // Header for the Article
        private ArticleHeader artHeader;

        // List of all companies
        private List<Company> companies;
        // List of all product-groups in a given company
        private List<ProductGroup> productGroups;
        // List of all products of a given group
        private List<Product> products;
        // List of all units
        private List<Unit> units;



        // Code of the company, to which the article, that is beeing edited, belongs
        [Parameter]
        public string CompCode { get; set; }

        // ID of the product-group, to which the article, that is beeing edited, belongs
        [Parameter]
        public string ProductGroupNr { get; set; }

        // ID of the product, to which the article, that is beeing edited, belongs
        [Parameter]
        public string ProductNr { get; set; }

        // ID of the article, that is beeing edited
		[Parameter]
		public string ArticleNr { get; set; }
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

            units = await uow.UnitRepo.GetAllModelsAsync();


            // An existing article is beeing edited
            if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr) && !string.IsNullOrEmpty(ProductNr) && !string.IsNullOrEmpty(ArticleNr))
            {
                isEdit = true;

                // Get the data of the article, that is beeing edited
                article = await uow.ArticleRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));

                // Set the product-group and product, to which the article belongs
                selProdGroup = await uow.ProductGroupRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr));
                selProduct = await uow.ProductRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));

                // Get all product-groups of the given company
                productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);
                // Get all products of the given company and product-group
                products = await uow.ProductRepo.GetAllModelsAsync(CompCode, Convert.ToInt32(ProductGroupNr));

                // Get the Billing- and Packaging-Unit of the article
                selBillingUnit = await uow.UnitRepo.GetModelByPkAsync(article.BillingUnit);
                selPackagingUnit = await uow.UnitRepo.GetModelByPkAsync(article.PackagingUnit);

                artHeader = await uow.ArticleHeaderRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));


                // Get the next available PK for the article
                nextPK = await uow.ArticleRepo.GetNextPkAsync(CompCode);
            }
            // A new article is beeing created
            else
            {
                isEdit = false;
                article = new Article();

                selCompany = companies.FirstOrDefault();

                selBillingUnit = units.FirstOrDefault();
                selPackagingUnit = units.FirstOrDefault();


                if (selCompany != null)
                {
                    // Get the next available PK for the new article
                    nextPK = await uow.ArticleRepo.GetNextPkAsync(selCompany.CompCode);

                    productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(selCompany.CompCode);
                    selProdGroup = productGroups.FirstOrDefault();


                    if (selProdGroup != null)
					{
                        products = await uow.ProductRepo.GetAllModelsAsync(selCompany.CompCode, selProdGroup.ProductGroupNr);
                        selProduct = products.FirstOrDefault();


                        if (selProduct != null)
						{
                            artHeader = await uow.ArticleHeaderRepo.GetModelByPkAsync(selCompany.CompCode, selProdGroup.ProductGroupNr, selProduct.ProductNr);
						}
					}
                }
            }


            // Only genereate the TreeView, if it has not yet been done (=> minimize traffic and loading time when changing the selected article in the TreeView)
            if (string.IsNullOrEmpty(treeViewMarkup.Value))
            {
                GenerateTreeView();
            }


            // Update the data on the view
            StateHasChanged();
        }



        /// <summary>
        /// Create a new article or update an existing one
        /// </summary>
        private async Task Save()
        {
            // Set the product-group and product
            article.ProductGroupNr = selProdGroup.ProductGroupNr;
            article.ProductNr = selProduct.ProductNr;
            article.Product = selProduct;

            // Set Billing- and Packaging-Unit
            article.BillingUnit = selBillingUnit.UnitCode;
            article.BillingUnitNavigation = selBillingUnit;
            article.PackagingUnit = selPackagingUnit.UnitCode;
            article.PackagingUnitNavigation = selPackagingUnit;

            article.ArticleHeaderId = artHeader.ArticleHeaderId;
            article.ArticleHeader = artHeader;


            // Update an existing article
            if (isEdit)
            {
                await uow.ArticleRepo.UpdateModelAsync(article);

                nav.NavigateTo("/article/" + article.CompCode);
            }
            // Create a new article
            else
            {
                article.ArticleNr = nextPK;
                article.CompCode = selCompany.CompCode;
                await uow.ArticleRepo.CreateModelAsync(article);

                nav.NavigateTo("/article/" + selCompany.CompCode);
            }
        }



        /// <summary>
        /// Delete an article from the database
        /// </summary>
        private async Task Delete()
        {
            // Only proceed, if an existing article is beeing edited
            if (isEdit)
            {
                await uow.ArticleRepo.DeleteModelAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr), Convert.ToInt32(ArticleNr));

                nav.NavigateTo("/article/" + CompCode);
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
            List<ProductGroup> allProductGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);
            List<Product> allProducts = await uow.ProductRepo.GetAllModelsAsync(CompCode);
            List<Article> allArticles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);

            List<TreeViewElementVM> treeViewElements = ConcatArticleTree.ConcatFromLists(allProductGroups, allProducts, allArticles);


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
                nav.NavigateTo("/article/" + args.Value.ToString());
            }
        }



        /// <summary>
        /// Change the company, to which the article belongs
        /// </summary>
        private async Task ChangeAssignedCompany(ChangeEventArgs args)
        {
            selCompany = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());
            nextPK = await uow.ArticleRepo.GetNextPkAsync(args.Value.ToString());

            productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(args.Value.ToString());
            selProdGroup = productGroups.FirstOrDefault();

            if (selProdGroup != null)
            {
                products = await uow.ProductRepo.GetAllModelsAsync(args.Value.ToString(), selProdGroup.ProductGroupNr);
                selProduct = products.FirstOrDefault();


                artHeader = (selProduct != null) ? await uow.ArticleHeaderRepo.GetModelByPkAsync(args.Value.ToString(), selProdGroup.ProductGroupNr, selProduct.ProductNr) : new ArticleHeader();
            }
        }



        /// <summary>
        /// Change the selected product-group, to which the article belongs
        /// </summary>
        private async Task ChangeProdGroup(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selProdGroup = await uow.ProductGroupRepo.GetModelByPkAsync(selCompany.CompCode, Convert.ToInt32(args.Value));

                products = await uow.ProductRepo.GetAllModelsAsync(selCompany.CompCode, selProdGroup.ProductGroupNr);
                selProduct = products.FirstOrDefault();


                artHeader = (selProduct != null) ? await uow.ArticleHeaderRepo.GetModelByPkAsync(selCompany.CompCode, Convert.ToInt32(args.Value), selProduct.ProductNr) : new ArticleHeader();
            }
        }



        /// <summary>
        /// Change the selected product, to which the article belongs
        /// </summary>
        private async Task ChangeProduct(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selProduct = await uow.ProductRepo.GetModelByPkAsync(selCompany.CompCode, selProdGroup.ProductGroupNr, Convert.ToInt32(args.Value));

                artHeader = await uow.ArticleHeaderRepo.GetModelByPkAsync(selCompany.CompCode, selProdGroup.ProductGroupNr, Convert.ToInt32(args.Value));
            }
        }



        /// <summary>
        /// Change the selected billing-unit of the article
        /// </summary>
        private async Task ChangeBillingUnit(ChangeEventArgs args)
		{
            if (!string.IsNullOrEmpty(args.Value.ToString()))
			{
                selBillingUnit = await uow.UnitRepo.GetModelByPkAsync(args.Value.ToString());
			}
        }



        /// <summary>
        /// Change the selected packaging-unit of the article
        /// </summary>
        private async Task ChangePackagingUnit(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selPackagingUnit = await uow.UnitRepo.GetModelByPkAsync(args.Value.ToString());
            }
        }
        #endregion
    }
}
