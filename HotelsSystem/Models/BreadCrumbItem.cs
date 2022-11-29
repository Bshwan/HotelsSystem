namespace HotelsSystem.Models
{
    public class BreadCrumbItem
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public int ParentID { get; set; }
    }
}