using System.ComponentModel.DataAnnotations;

namespace TastyBeans.Portal.Client.Forms;

public record PaymentMethodDetails
{
    [Required(ErrorMessage = "Please select a card type")]
    public CardType? CardType { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for card number")]
    public string CardNumber { get; set; } = "";
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for expiration date")]
    [RegularExpression("\\d{2}/\\d{2}", ErrorMessage = "Please provide a valid expiration date (mm/yy)")]
    public string ExpirationDate { get; set; } = "";
    
    [MaxLength(3, ErrorMessage = "Security code can't contain more than 3 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for security code")]
    public string SecurityCode { get; set; } = "";
    
    [MaxLength(150, ErrorMessage = "Card holder name can't contain more than 150 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for card holder name")]
    public string CardHolderName { get; set; } = "";
}