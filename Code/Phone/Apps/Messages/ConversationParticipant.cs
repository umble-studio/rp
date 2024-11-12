namespace Rp.Phone.Apps.Messages;

public record ConversationParticipant : IAuthor, IPhoneNumber
{
	public PhoneNumber PhoneNumber { get; init; }
	public string Name { get; init; } = null!;
	public string? Avatar { get; init; }
}
