namespace DevShop.Authentication.ViewModels
{
	/// <summary>
	/// This model is beeing used for the login view.
	/// It stores the data, the user types in, when trying to log in (E-Mail/Username and Password)
	/// </summary>
	public class LoginVM
	{
		public string UserName { get; set; }

		public string Password { get; set; }
	}
}
