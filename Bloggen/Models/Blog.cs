namespace Bloggen.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}
