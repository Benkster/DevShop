namespace DevShop.Data.ViewModels.TreeBuilderVMs
{
    /// <summary>
    /// This class serves as a general model representing items inside a select-element.
    /// It allowes the use of methods from the TreeBuilder, no matter what type the base-list is of.
    /// Just convert the type of the base-list to a list of type of this class.
    /// e.g.:
    /// A list of type Category has to be converted to a list of type SelectListDataVM, to be able to access the methods from the TreeBuilder-Class.
    /// </summary>
    public class SelectListDataVM
    {
        public int ElemID { get; set; }

        public string ElemName { get; set; }

        public int? ParentID { get; set; }
    }
}
