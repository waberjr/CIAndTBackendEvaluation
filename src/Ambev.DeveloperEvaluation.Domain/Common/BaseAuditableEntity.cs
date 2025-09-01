namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the entity information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}