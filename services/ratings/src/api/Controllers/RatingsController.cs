using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Ratings.Api.Forms;
using RecommendCoffee.Ratings.Application.CommandHandlers;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using RecommendCoffee.Shared.Api;

namespace RecommendCoffee.Ratings.Api.Controllers;

[ApiController]
[Route("/ratings")]
public class RatingsController : ControllerBase
{
    private readonly RegisterRatingCommandHandler _registerRatingCommandHandler;

    public RatingsController(RegisterRatingCommandHandler registerRatingCommandHandler)
    {
        _registerRatingCommandHandler = registerRatingCommandHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRatingForm form)
    {
        var response = await _registerRatingCommandHandler.ExecuteAsync(
            new RegisterRatingCommand(form.CustomerId, form.ProductId, form.Value));

        ModelState.AddValidationErrors(response.Errors);

        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Accepted();
    }
}
