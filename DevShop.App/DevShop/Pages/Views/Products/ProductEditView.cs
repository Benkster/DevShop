using DevShop.Data.Helpers;
using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DevShop.Pages.Views.Products
{
    /// <summary>
    /// Class for the Product-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class ProductEditView
    {
        #region Variables/Properties
        // Determines, whether an existing model is beeing edited or a new one is beeing created
        private bool isEdit = false;
        // Determines, whether a picture of the product exists or not
        private bool fileExists = false;

        private int nextPK = 0;

        // Displays an error message, if something went wrong
        private string errorMessage = string.Empty;
        // Displays a message, if a file is beeing uploaded
        private string fileUploadMessage = string.Empty;
        // Path of the picture of the selected product
        private string filePicPath = string.Empty;

        // Holds the HTML-Code for the TreeView
        MarkupString treeViewMarkup;

        // Used as a model for the edit-form
        private Product product;
        // The Product-Group, to which the product belongs
        private ProductGroup selProdGroup;
        // The company, to which the product, that is beeing created, belongs
        private Company selCompany;
        // Picture of the product
        private ImageFile fileImage;

        // List of all companies
        private List<Company> companies;
        // List of all product-groups in a given company
        private List<ProductGroup> productGroups;



        // Code of the company, to which the product, that is beeing edited, belongs
        [Parameter]
        public string CompCode { get; set; }

        // ID of the product-group, to which the product, that is beeing edited, belongs
        [Parameter]
        public string ProductGroupNr { get; set; }

        // ID of the product, that is beeing edited
		[Parameter]
		public string ProductNr { get; set; }
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

            fileImage = new ImageFile();

            fileUploadMessage = string.Empty;


            // An existing product is beeing edited
            if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(ProductGroupNr) && !string.IsNullOrEmpty(ProductNr))
            {
                isEdit = true;

                // Check, whether a picture has been uploaded for this product or not
                string filePath = @"wwwroot\pic\products\" + CompCode + @"\pic_" + ProductNr;
                Dictionary<bool, string> checkFileExists = fileManager.FileExists(filePath);
                fileExists = checkFileExists.FirstOrDefault().Key;


                // If a picture of the product has been uploaded, store the path of the picture in a variable (will be displayed as a preview)
                if (fileExists)
                {
                    filePicPath = "/pic/products/" + CompCode + "/pic_" + ProductNr + "." + checkFileExists.FirstOrDefault().Value;
                }

                // Get the product, that is beeing edited
                product = await uow.ProductRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));
                selProdGroup = await uow.ProductGroupRepo.GetModelByPkAsync(CompCode, Convert.ToInt32(ProductGroupNr));

                productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);


                // Get the next available PK for the product
                nextPK = await uow.ProductRepo.GetNextPkAsync(CompCode);
            }
            // A new product is beeing created
            else
            {
                isEdit = false;
                fileExists = false;
                filePicPath = string.Empty;

                product = new Product();

                selCompany = companies.FirstOrDefault();


                if (selCompany != null)
                {
                    // Get the next available PK for the new product
                    nextPK = await uow.ProductRepo.GetNextPkAsync(selCompany.CompCode);

                    productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(selCompany.CompCode);
                    selProdGroup = productGroups.FirstOrDefault();
                }
            }


            // Only genereate the TreeView, if it has not yet been done (=> minimize traffic and loading time when changing the selected product in the TreeView)
            if (string.IsNullOrEmpty(treeViewMarkup.Value))
            {
                GenerateTreeView();
            }


            // Update the data on the view
            StateHasChanged();
        }



        /// <summary>
        /// Create a new product or update an existing one
        /// </summary>
        private async Task Save()
        {
            // Set the product-group
            product.ProductGroupNr = selProdGroup.ProductGroupNr;
            product.ProductGroup = selProdGroup;


            // Update an existing product
            if (isEdit)
            {
                await uow.ProductRepo.UpdateModelAsync(product);

                nav.NavigateTo("/product/" + product.CompCode);
            }
            // Create a new product-group
            else
            {
                product.ProductNr = nextPK;
                product.CompCode = selCompany.CompCode;
                await uow.ProductRepo.CreateModelAsync(product);

                nav.NavigateTo("/product/" + selCompany.CompCode);
            }
        }



        /// <summary>
        /// Delete a product from the database
        /// </summary>
        private async Task Delete()
        {
            // Only proceed, if an existing product is beeing edited
            if (isEdit)
            {
                List<Article> articles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);


                // User is not allowed to delete the product, if one or more Articles are assigned to it
                if (articles.Any(a => a.ProductNr == Convert.ToInt32(ProductNr)))
                {
                    errorMessage = "There are one or more Articles assigned to this product. Please delete them first in order to delete this product.";
                }
                // No article is assigned to the product => delete it
                else
                {
                    await uow.ProductRepo.DeleteModelAsync(CompCode, Convert.ToInt32(ProductGroupNr), Convert.ToInt32(ProductNr));

                    nav.NavigateTo("/product/" + CompCode);
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
            List<ProductGroup> allProductGroups = await uow.ProductGroupRepo.GetAllModelsAsync(CompCode);
            List<Product> products = await uow.ProductRepo.GetAllModelsAsync(CompCode);
            List<Article> articles = await uow.ArticleRepo.GetAllModelsAsync(CompCode);

            List<TreeViewElementVM> treeViewElements = ConcatArticleTree.ConcatFromLists(allProductGroups, products, articles);


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
                nav.NavigateTo("/product/" + args.Value.ToString());
            }
        }



        /// <summary>
        /// Change the company, to which the product belongs
        /// </summary>
        private async Task ChangeAssignedCompany(ChangeEventArgs args)
        {
            selCompany = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());
            nextPK = await uow.ProductRepo.GetNextPkAsync(args.Value.ToString());

            productGroups = await uow.ProductGroupRepo.GetAllModelsAsync(args.Value.ToString());
            selProdGroup = productGroups.FirstOrDefault();
        }



        /// <summary>
        /// Change the selected product-group, to which the product belongs
        /// </summary>
        private async Task ChangeProdGroup(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selProdGroup = await uow.ProductGroupRepo.GetModelByPkAsync(selCompany.CompCode, Convert.ToInt32(args.Value));
            }
        }



        /// <summary>
        /// Loads in the file, that has been selected as picture for the product.
        /// </summary>
        private async Task StoreFile(InputFileChangeEventArgs args)
        {
            // Only load it in, if an existing product is beeing edited
            if (isEdit)
            {
                // Set the name of the file
                string fileName = "pic_" + ProductNr;
                // Store the data of the file
                fileImage = await fileManager.StoreFile(args, fileName);

                fileUploadMessage = string.Empty;


                StateHasChanged();
            }
        }



        /// <summary>
        /// Uploads the file, that has been selected as the picture of the product
        /// </summary>
        private async Task UploadFile()
        {
            // Only upload a file, if a product is beeing edited and the user has already selected a file to upload
            if (isEdit && fileImage != null && !string.IsNullOrEmpty(fileImage.FileName))
            {
                // Location, where the file will be stored
                string filePath = @"wwwroot\pic\products\" + CompCode;

                // Upload the file
                bool uploadSuccess = await fileManager.UploadFile(filePath, fileImage);


                // File has been uploaded
                if (uploadSuccess)
                {
                    fileUploadMessage = "File uploaded";

                    // Specify, that an image now exists and get get the path of the image to display it on the view
                    fileExists = true;
                    filePicPath = "/pic/products/" + CompCode + "/pic_" + ProductNr + "." + fileImage.FileType.Split("/")[1];

                    fileImage = new ImageFile();
                }
                // Error while trying to upload the file
                else
                {
                    fileUploadMessage = "Could not upload file";
                }


                StateHasChanged();
            }
        }



        /// <summary>
        /// Delete the image of the product
        /// </summary>
        private void DeleteFile()
        {
            // Only delete it, if a product is beeing edited
            if (isEdit)
            {
                // Location of the image
                string filePath = @"wwwroot\pic\products\" + CompCode + @"\pic_" + ProductNr;

                // Delete the file
                bool deleteSuccess = fileManager.DeleteFile(filePath);


                // Image was deleted
                if (deleteSuccess)
                {
                    fileUploadMessage = "File deleted";

                    fileExists = false;
                    filePicPath = string.Empty;
                }
                // Error while deleting the file
                else
                {
                    fileUploadMessage = "Could not delete file";
                }


                StateHasChanged();
            }
        }
        #endregion
    }
}
