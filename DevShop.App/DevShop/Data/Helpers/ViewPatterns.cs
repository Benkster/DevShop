namespace DevShop.Data.Helpers
{
	/// <summary>
	/// This class provides variables, that are holding a specific regex-pattern.
	/// Those patterns can be used for the HTML-elements in the views.
	/// This way, a pattern, that is used multiple times on different views, only has to be created once.
	/// </summary>
	public static class ViewPatterns
	{
		#region General Patterns
		public static string TelPattern = @"(^\+[0-9]{1,3})(\s?[0-9]*)+([0-9]$)";

		public static string WebsitePattern = @"^(https?):\/\/(([a-zA-Z]|[0-9])+\.)+[a-zA-Z]+$";
		#endregion



		#region Company Patterns
		public static string CompCodePattern = "^[A-Z]{2}[0-9]{4}$";
		#endregion
	}
}
