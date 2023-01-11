using DevShop.Authentication;
using DevShop.Authentication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;

namespace DevShop.Pages.Views.Authentication
{
    public class LoginModel : PageModel
    {
		#region Variables/Properties
		private readonly IHttpContextAccessor _accessor;
		private readonly IAuthManager _authManager;



		[BindProperty]
		public LoginVM LoginForm { get; set; }

		public string ErrorMessage { get; set; }
		#endregion


		#region Constructors
		public LoginModel(IHttpContextAccessor accessor, IAuthManager authManager)
		{
			_accessor = accessor;
			_authManager = authManager;
		}
		#endregion


		#region Methods
		public async Task<IActionResult> OnGetAsync()
        {
			if (_accessor.HttpContext.User.Identity.IsAuthenticated)
			{
				return Redirect("/");
			}

			return Page();
        }



		public async Task<IActionResult> OnPostAsync()
		{
			ErrorMessage = await _authManager.LoginAsync(LoginForm);

			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				return Page();
			}

			return Redirect("/");
		}
		#endregion
	}
}
