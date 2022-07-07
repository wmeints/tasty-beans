using Microsoft.AspNetCore.Mvc;
using TastyBeans.Ratings.Api.Forms;
using TastyBeans.Ratings.Application.CommandHandlers;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using TastyBeans.Shared.Api;

namespace TastyBeans.Ratings.Api.Controllers;

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
