namespace HotelsSystem.Models
{
    public class SPResult
    {
        public int Result { get; set; }
        public string? MSG { get; set; }
        public string? LastValue { get; set; }
        public string? CNSTR { get; set; }
        public List<int> Roles = new List<int>();

    }
}