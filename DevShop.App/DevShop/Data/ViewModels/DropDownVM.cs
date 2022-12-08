namespace DevShop.Data.ViewModels
{
	/// <summary>
	/// This ViewModel is needed for Edit-Views, that have a select-element with a list of the same model, that is beeing displayed on the Edit-View.
	/// E.g.:
	/// If the user changes the name of a country in the Edit-View, but does not save it, the name of the country in the select-element will automatically update, even though the data has not been saved.
	/// To prevent this from happening, the elements in the dropdown have to be a list of type DropDownVM
	/// </summary>
	public class DropDownVM
	{
		public string Value { get; set; }

		public string DisplayName { get; set; }
	}
}
