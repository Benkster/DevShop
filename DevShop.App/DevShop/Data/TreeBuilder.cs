using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.AspNetCore.Components;
using System.Drawing.Imaging;
using System.Text;

namespace DevShop.Data
{
    /// <summary>
    /// The TreeBuilder is a helping class with methods, that
    /// # Build up code for a TreeView
    /// # Build up code for select-elements (like excluding all own child elements from the selection for the parent-element)
    /// </summary>
    public class TreeBuilder
    {
        #region Variables
        #region Exclude Children From Selection
        // Contains all child-elements, that are beeing excluded from a select-list
        private List<SelectListDataVM> excludeElemList;
        #endregion



        #region Build TreeView
        // Keeps track of the current depth-level while generating the Code for the TreeView
        int treeDepth = 0;

        // Stores the HTML-Code for a TreeView as string 
        private StringBuilder treeViewResultHtmlStringBuilder;
        #endregion
		#endregion



		#region Constructors
		public TreeBuilder()
        {
            treeViewResultHtmlStringBuilder = new StringBuilder();
            excludeElemList = new List<SelectListDataVM>();
        }
        #endregion



        #region Methods
        #region Exclude Children From Selection
        /// <summary>
        /// Exclude all own child-elements of a specified element from a select-list.
        /// (i.e. if there's a possibility to select a parent-element in an edit-view, the user should not be able to change the parent-element to one of the own child elements)
        /// e.g.:
        /// Tech -> Parent-Element = root (=> there is no parent)
        /// Hardware -> Parent-Element = Tech
        /// Now the user should not be allowed to change the Parent-Element of "Tech" from "root" to "Hardware", because that would make no sense
        /// </summary>
        /// <param name="_fullList">
        /// A list of all elements, that exist
        /// </param>
        /// <param name="_rootID">
        /// ID of the element, the children of which should be excluded
        /// </param>
        /// <returns>
        /// A list of all elements, that are allowed to be selected
        /// </returns>
        public List<SelectListDataVM> ExcludeChildrenFromSelectList(List<SelectListDataVM> _fullList, int _rootID)
        {
            // Contains all elements, that should be excluded
            excludeElemList = new List<SelectListDataVM>();
            // Fill the list of elements, that should be excluded
            AddToExcludeElemList(_fullList, _fullList.Where(l => l.ParentID == _rootID).ToList());


            // Exclude the elements from the list of all elements
            List<SelectListDataVM> resultList = _fullList
                .Where(fl => excludeElemList.All(el => el.ElemID != fl.ElemID) && fl.ElemID != _rootID)
                .OrderBy(l => l.ParentID)
                .OrderBy(l => l.ElemName)
                .ToList();


            return resultList;
        }



        /// <summary>
        /// This recursive method adds all child-elements, that should be excluded from a selection, to a list
        /// </summary>
        /// <param name="_fullList">
        /// A list containing all existing elements
        /// </param>
        /// <param name="_childList">
        /// A list containing all child-elements of a certain element.
        /// Pass the direct child-elements of the element, whose children should be excluded from the selection, to loop through all children
        /// </param>
        private void AddToExcludeElemList(List<SelectListDataVM> _fullList, List<SelectListDataVM> _childList)
        {
            // Loop through all children of the current parent-element
            foreach (SelectListDataVM elem in _childList)
            {
                // Add the child to the list, that holds all elements, that should be excluded
                excludeElemList.Add(elem);


                // If the current child-element also has children, add them to the exclude-list as well
                if (_fullList.Any(l => l.ParentID == elem.ElemID))
                {
                    AddToExcludeElemList(_fullList, _fullList.Where(l => l.ParentID == elem.ElemID).ToList());
                }
            }
        }
		#endregion



		#region Build TreeView
		/// <summary>
		/// Create HTML-Code for a TreeView (nested ul and li-elements) out of a list of elements
		/// </summary>
		/// <param name="_treeViewElements">
		/// A list of elements, that should be displayed in the TreeView
		/// </param>
		/// <returns>
		/// A MarkupString containing the HTML-Code of the TreeView
		/// </returns>
		public MarkupString BuildTree(List<TreeViewElementVM> _treeViewElements)
        {
            // Result HTML as string
            string treeViewResultHtml = string.Empty;
            // Used to build up the string for the TreeView
            treeViewResultHtmlStringBuilder = new StringBuilder();

            treeDepth = 0;

            // Sort the list after ParentID and ID of the elements
            _treeViewElements = _treeViewElements.OrderBy(t => t.ParentID).OrderBy(t => t.ElemID).ToList();


            // Generate HTML-Code for the TreeView as string
            BuildTreeHtml(_treeViewElements, _treeViewElements.Where(t => t.ParentID == 0).ToList());

            treeViewResultHtml = treeViewResultHtmlStringBuilder.ToString();



            // Return the HTML-Code as MarkupString (makes it possible to display the string as actual HTML-elements in the view)
            return (MarkupString)treeViewResultHtml;
        }



        /// <summary>
        /// Recursively go through a list of elements to build up a TreeView with ul-li elements.
        /// HTML-Code is stored as string.
        /// </summary>
        /// <param name="_fullList">
        /// A list containing all existing elements, that should be shown in the TreeView
        /// </param>
        /// <param name="_treeViewLevelElements">
        /// A list of all elements of the current depth-level of the TreeView (i.e. direct child-elements of another element)
        /// </param>
        private void BuildTreeHtml(List<TreeViewElementVM> _fullList, List<TreeViewElementVM> _treeViewLevelElements)
		{
            // Decend down to next depth-level of TreeView
            treeDepth++;


            // Add a radio-button and label for the sub-menu (needed to open it on mobile-devices)
            if (treeDepth > 1)
			{
                treeViewResultHtmlStringBuilder.Append(
                    "<input id=\"rad_tree_openSub_" + _treeViewLevelElements.FirstOrDefault().ParentID.ToString() + "\" class=\"rad_tree_openSub\" type=\"radio\" name=\"rad_tree_openSub\">" +
                    "<label id=\"lbl_tree_openSub_" + _treeViewLevelElements.FirstOrDefault().ParentID.ToString() + "\" class=\"lbl_tree_openSub\" for=\"rad_tree_openSub_" + _treeViewLevelElements.FirstOrDefault().ParentID.ToString() + "\"></label>"
                );
			}
            // Before the TreeView begins, add radio-buttons and a label to be able to open it on mobile-devices
            else
			{
                treeViewResultHtmlStringBuilder.Append(
                    "<input id=\"rad_tree_closeSub\" type=\"radio\" name=\"rad_tree_openSub\">" +
                    "<input id=\"chk_toggleTree\" type=\"checkbox\">" +
                    "<label id=\"lbl_openTree\" class=\"lbl_toggleTree\" for=\"chk_toggleTree\"></label>" +
                    "<label id=\"lbl_closeTree_layerBack\" for=\"chk_toggleTree\"></label>"
                );
			}


            // Open the ul-element
            treeViewResultHtmlStringBuilder.Append("<ul id=\"treeLayer_" + treeDepth.ToString() + "\">");


            // Add a label to close the sub-menu (on mobile devices)
            if (treeDepth > 1)
			{
                treeViewResultHtmlStringBuilder.Append("<label id=\"lbl_tree_closeSub_" + _treeViewLevelElements.FirstOrDefault().ParentID.ToString() + "\" class=\"lbl_tree_closeSub\" for=\"rad_tree_closeSub\"></label>");
			}
            // Add a label to close the TreeView (on mobile devices)
            else
			{
                treeViewResultHtmlStringBuilder.Append("<label id=\"lbl_closeTree\" class=\"lbl_toggleTree\" for=\"chk_toggleTree\"></label>");
			}


            // Loop through all direct children of the current element
            foreach (TreeViewElementVM elem in _treeViewLevelElements)
			{
                // Add an li-element containing a link with the name of the current element
                treeViewResultHtmlStringBuilder.Append(
                    "<li class=\"treeItem treeItemLayer_" + treeDepth.ToString() + "\">" +
                    "<a class=\"treeLink\" href=\"" + elem.ElemLink + "\">" + elem.ElemText + "</a>"
                );


                // If the current child-element also has children, add them to the TreeView as well
                if (_fullList.Any(f => f.ParentID == elem.ElemID))
                {
                    BuildTreeHtml(_fullList, _fullList.Where(f => f.ParentID == elem.ElemID).ToList());
                }


                treeViewResultHtmlStringBuilder.Append("</li>");
			}

            // Close the ul-element
            treeViewResultHtmlStringBuilder.Append("</ul>");

            // Climb up one depth-level of the TreeView
            treeDepth--;
		}
        #endregion
        #endregion
    }
}
