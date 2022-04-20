using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecommendCoffee.Portal.Client.Forms;

public record Address
{
    [MaxLength(100, ErrorMessage = "Street can't contain more than 100 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for street")]
    public string Street { get; set; } = "";
     
    [MaxLength(20, ErrorMessage = "Building number can't contain more than 20 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for building number")]
    [DisplayName("Building number")]
    public string HouseNumber { get; set; } = "";
    
    [MaxLength(20, ErrorMessage = "Zip code can't contain more than 20 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for zip code")]
    public string PostalCode { get; set; } = "";
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for city")]
    [MaxLength(100, ErrorMessage = "City can't contain more than 100 characters")]
    public string City { get; set; } = "";
    
    public string CountryCode { get; set; } = "US";
}