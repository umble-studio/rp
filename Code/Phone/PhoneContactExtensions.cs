using Rp.Phone.Apps.Messages;

namespace Rp.Phone;

public static class PhoneContactExtensions
{
	public static ConversationParticipant ToConversationParticipant( this PhoneContact contact ) => new()
	{
		Avatar = contact.ContactAvatar,
		Name = contact.ContactName,
		PhoneNumber = contact.ContactNumber
	};
}
