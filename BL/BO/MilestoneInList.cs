
namespace BO;

/// <summary>
/// Represents a simplified view of a milestone, used in lists.
/// </summary>
public class MilestoneInList
{
    int Id { get; init; }
    string Description { get; set; }
    string Alias { get; set; }
    Status? Status { get; set; }
    double? CompletionPercentage { get; set; }
}
