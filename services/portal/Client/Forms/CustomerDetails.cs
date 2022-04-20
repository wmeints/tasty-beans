using System.ComponentModel.DataAnnotations;

namespace RecommendCoffee.Portal.Client.Forms;

public record CustomerDetails
{
    [MaxLength(100, ErrorMessage = "First name can't contain more than 100 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for first name")]
    public string FirstName { get; set; } = "";
    
    [MaxLength(100, ErrorMessage = "Last name can't contain more than 100 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage= "Please provide a value for last name")]
    public string LastName { get; set; } = "";
    
    [MaxLength(500, ErrorMessage = "E-mail address can't contain more than 500 characters")]
    [DataType(DataType.EmailAddress)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for e-mail address")]
    public string EmailAddress { get; set; } = "";
    
    [MaxLength(13, ErrorMessage = "Telephone number can't contain more than 13 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for telephone number")]
    public string TelephoneNumber { get; set; } = "";
    
    [ValidateComplexType]
    public Address InvoiceAddress { get; set; } = new Address();
    
    [ValidateComplexType]
    public Address ShippingAddress { get; set; } = new Address();
}