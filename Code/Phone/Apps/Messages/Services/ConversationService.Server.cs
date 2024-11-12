using System;
using RoverDB;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	private bool ServerConversationExists( PhoneContact creator, PhoneContact target )
	{
		return RoverDatabase.Instance.Exists<ConversationData>( x =>
			x.Participants.Any( p => p.PhoneNumber == creator.ContactNumber ) &&
			x.Participants.Any( p => p.PhoneNumber == target.ContactNumber ) );
	}
	
	private bool ServerConversationExists( Guid conversationId )
	{
		return RoverDatabase.Instance.Exists<ConversationData>( x => x.Id == conversationId );
	}
}
