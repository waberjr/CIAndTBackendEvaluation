using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

/// <summary>
/// Request model for deleting a user
/// </summary>
public class DeleteUserRequest
{
    /// <summary>
    /// The unique identifier of the user to delete
    /// </summary>
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}
