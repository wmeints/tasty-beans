namespace TastyBeans.Ratings.Api.Forms
{
    public class RegisterRatingForm
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Value { get; set; }
    }
}
