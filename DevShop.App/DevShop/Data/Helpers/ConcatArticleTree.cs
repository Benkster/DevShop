using DevShop.Data.Models;
using DevShop.Data.ViewModels.TreeBuilderVMs;

namespace DevShop.Data.Helpers
{
	/// <summary>
	/// This class provides the possibility to concatenate lists of ProductGroups, Products and Articles, in order to build up a TreeView including all these elements.
	/// </summary>
	public static class ConcatArticleTree
	{
		#region Methods
		/// <summary>
		/// Concatenate a list of ProductGroups, Products and Articles to a list of elements used for building a TreeView
		/// </summary>
		/// <param name="_prodGroupElements">
		/// A list of ProductGroups
		/// </param>
		/// <param name="_productElements">
		/// A list of Products
		/// </param>
		/// <param name="_articleElements">
		/// A list of articles
		/// </param>
		/// <returns>
		/// A list of type TreeViewElementVM
		/// </returns>
		public static List<TreeViewElementVM> ConcatFromLists(List<ProductGroup> _prodGroupElements, List<Product> _productElements, List<Article> _articleElements)
		{
			// Holds the concatenated lists
			List<TreeViewElementVM> resultTreeList = new List<TreeViewElementVM>();


			// Convert the list of ProductGroups into a list of treeview-elements
			List<TreeViewElementVM> prodGroupTreeList = _prodGroupElements.OrderBy(p => p.SortNr).Select(p => new TreeViewElementVM()
			{
				ElemID = p.ProductGroupNr,
				ElemText = p.GroupName,
				ParentID = p.ParentId,
				ElemLink = "./product-group/edit/" + p.CompCode + "/" + p.ProductGroupNr.ToString()
			}).ToList();

			// Convert the list of Products into a list of treeview-elements
			List<TreeViewElementVM> productTreeList = _productElements.OrderBy(p => p.SortNr).Select(p => new TreeViewElementVM()
			{
				ElemID = p.ProductNr + 100000,
				ElemText = p.ProductName,
				ParentID = p.ProductGroupNr,
				ElemLink = "./product/edit/" + p.CompCode + "/" + p.ProductGroupNr.ToString() + "/" + p.ProductNr.ToString()
			}).ToList();

			// Convert the list of Articles into a list of treeview-elements
			List<TreeViewElementVM> articleTreeList = _articleElements.OrderBy(a => a.SortNr).Select(a => new TreeViewElementVM()
			{
				ElemID = a.ArticleNr + 200000,
				ElemText = a.ArticleName,
				ParentID = a.ProductNr,
				ElemLink = "./article/edit/" + a.CompCode + "/" + a.ProductGroupNr.ToString() + "/" + a.ProductNr.ToString() + "/" + a.ArticleNr.ToString()
			}).ToList();



			// Concatenate the lists
			resultTreeList.AddRange(prodGroupTreeList);
			resultTreeList.AddRange(productTreeList);
			resultTreeList.AddRange(articleTreeList);


			return resultTreeList;
		}
		#endregion
	}
}
