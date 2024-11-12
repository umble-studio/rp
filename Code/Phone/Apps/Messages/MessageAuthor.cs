namespace Rp.Phone.Apps.Messages;

public record struct MessageAuthor : IPhoneNumber
{
	public PhoneNumber PhoneNumber { get; init; }
}
