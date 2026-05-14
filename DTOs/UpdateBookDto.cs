namespace BookApi.DTOs
{
    public class UpdateBookDto
    {
        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public DateTime PublishDate { get; set; }
    }
}
