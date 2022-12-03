using DevShop.Data.ViewModels.TreeBuilderVMs;

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
        // Contains all child-elements, that are beeing excluded from a select-list
        private List<SelectListDataVM> excludeElemList;
        #endregion



        #region Constructors
        public TreeBuilder()
        {
            excludeElemList = new List<SelectListDataVM>();
        }
        #endregion



        #region Methods
        #region Public Methods
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
        #endregion



        #region Private Methods
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
        #endregion
    }
}
