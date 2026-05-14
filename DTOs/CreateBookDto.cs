namespace BookApi.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public DateTime PublishDate { get; set; }
    }
}
