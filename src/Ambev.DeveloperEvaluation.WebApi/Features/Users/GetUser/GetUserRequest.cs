using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Request model for getting a user by ID
/// </summary>
public class GetUserRequest
{
    /// <summary>
    /// The unique identifier of the user to retrieve
    /// </summary>
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}
