namespace PDP.University.Examine.Project.Web.API.Helpers
{
    public class CategoryQueryObject
    {
        public string Name { get; set; } = null;
        public string SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
    }
}
