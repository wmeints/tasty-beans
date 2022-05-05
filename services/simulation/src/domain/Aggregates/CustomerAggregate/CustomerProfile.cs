namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

/// <summary>
/// Defines how the customer will behave over its tenure at the company.
/// </summary>
public class CustomerProfile
{
    /// <summary>
    /// Gets or sets the loyality level of the customer.
    /// </summary>
    /// <value>Value between 0 and 1 indicating how loyal a customer is.</value>
    public double Loyality { get; set; }
    
    /// <summary>
    /// Gets or sets the level of decay for the loyality of the customer.
    /// </summary>
    /// <value>Value between 0 and 1 indicating how fast the loyality level should decay over time.
    /// A value close to 1 means high decay rating. A value close to 0 means low decay rate.</value>
    public double LoyalityDecay { get; set; }
    
    /// <summary>
    /// Gets or sets the delivery quality level of the customer.
    /// </summary>
    /// <value>Value between 0 and 1 indicating how important good delivery is.</value>
    public double DeliveryQuality { get; set; }
    
    /// <summary>
    /// Gets or sets the decay for the delivery quality level.
    /// </summary>
    /// <value>A value between 0 and 1 indicating how fast the delivery quality level should increase.</value>
    public double DeliveryQualityGrowth { get; set; }
    
    /// <summary>
    /// Gets or sets the product quality level of the customer.
    /// </summary>
    /// <value>Value between 0 and 1 indicating how important getting the right product is.</value>
    public double ProductQuality { get; set; }
}