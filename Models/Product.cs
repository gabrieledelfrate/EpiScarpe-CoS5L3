namespace EpiScarpe_Co.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CoverImage { get; set; }
        public string AdditionalImage1 { get; set; }
        public string AdditionalImage2 { get; set; }
        public bool IsDisplayedOnHomePage { get; set; }
    }
}
