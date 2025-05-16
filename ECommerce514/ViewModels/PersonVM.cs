namespace ECommerce514.ViewModels
{
    public class PersonVM
    {
        public int Age { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> Skills { get; set; }
    }
}
