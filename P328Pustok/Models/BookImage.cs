namespace P328Pustok.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ImagesId { get; set; }
        public string ImageName { get; set; }

        public bool? PosterStatus { get; set; }

        public List<Images> ListImages { get; set; }
        public Images Images { get; set; }
        public Book Book { get; set; }
    }
}
