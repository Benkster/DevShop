using DevShop.Models;

namespace DevShop.Pages.Views.Articles
{
	/// <summary>
	/// Logic and methods for the view
	/// </summary>
	public partial class CompanyView
	{
		#region Variables
		private List<Company> companies;
		#endregion



		#region Methods
		protected override async Task OnInitializedAsync()
		{
			companies = await uow.CompanyRepo.GetAllCompaniesAsync();
		}
		#endregion
	}
}
