using DevShop.Data.Models;
using DevShop.Data.SortTypes;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Categories
{
    public partial class CategoryEditView
    {
        #region Variables/Properties
        private bool isEdit = false;

        private int parentID = 0;

        private string errorMessage = string.Empty;

        private Category category;

        private List<Category> categories;



        [Parameter]
        public string CategoryID { get; set; }
		#endregion



		#region Methods
		#region Main-Methods
		protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(CategoryID))
            {
                isEdit = true;
                category = await uow.CategoryRepo.GetModelByPkAsync(Convert.ToInt32(CategoryID));
                categories = await uow.CategoryRepo.GetModelsWithoutChildrenAsync(Convert.ToInt32(CategoryID));
            }
            else
            {
                category = new Category();
                categories = await uow.CategoryRepo.GetAllModelsAsync(CategorySortType.SortType.Name);
            }
        }



        private async Task Save()
        {
            category.ParentId = parentID;


            if (isEdit)
            {
                await uow.CategoryRepo.UpdateModelAsync(category);
            }
            else
            {
                await uow.CategoryRepo.CreateModelAsync(category);
            }


            nav.NavigateTo("/category");
        }



        private async Task Error()
        {
            errorMessage = "Something went wrong. Please try again";
        }
        #endregion



        #region Side-Methods
        private void ChangeParentElem(ChangeEventArgs args)
        {
            parentID = Convert.ToInt32(args.Value);
        }
        #endregion
        #endregion
    }
}
