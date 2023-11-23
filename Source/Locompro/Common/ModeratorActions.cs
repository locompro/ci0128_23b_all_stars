namespace Locompro.Common;

/// <summary>
/// Defines the various actions that a moderator can take in response to user submissions.
/// This enumeration is used to specify the type of action being performed on a submission,
/// such as rejecting or approving it.
/// </summary>
public enum ModeratorActions
{
    /// <summary>
    /// Represents the default action. This value is used to indicate that no specific moderator action has been taken.
    /// It can be used as a placeholder or to initialize variables when no other action is applicable.
    /// </summary>
    Default,

    /// <summary>
    /// Represents the action of rejecting a submission. This action is taken by a moderator
    /// when a submission does not meet the required criteria or standards.
    /// </summary>
    RejectSubmission,

    /// <summary>
    /// Represents the action of approving a submission. This action is taken by a moderator
    /// when a submission meets all the necessary criteria and standards and is deemed suitable for acceptance.
    /// </summary>
    ApproveSubmission
}