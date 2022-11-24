namespace HotelsSystem.Routes
{
    public class Go
    {
        public static string To(string page, int id) => $"{page}/{Hasher.Hash(id)}";
    }
}