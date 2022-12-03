using DevShop.Data.Models;
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
        private List<Category> categories;
        #endregion



        #region Methods
        protected override async Task OnInitializedAsync()
        {
            categories = await uow.CategoryRepo.GetAllModelsAsync();
        }



        private async Task Delete(int _pk)
        {

        }
        #endregion
    }
}