namespace DevShop.Data.ViewModels.TreeBuilderVMs
{
	/// <summary>
	/// This model is beeing used as a general model for creating a TreeView out of a list of elements.
	/// Create a list of type TreeViewElementVM to be able to use the methods of the TreeBuilder.
	/// </summary>
	public class TreeViewElementVM
	{
		public int ElemID { get; set; }

		public string ElemText { get; set; }

		public int? ParentID { get; set; }

		public string ElemLink { get; set; }

        public int? SortNr { get; set; }
    }
}
