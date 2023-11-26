namespace Reviews_AJAX.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
