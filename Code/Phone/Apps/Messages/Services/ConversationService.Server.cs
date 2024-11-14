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

	private bool ServerTryGetConversation( Guid conversationId, out ConversationData conversation )
	{
		var c = RoverDatabase.Instance.SelectOne<ConversationData>( x => x.Id == conversationId );

		if ( c is null )
		{
			conversation = default!;
			return false;
		}

		conversation = c;
		return true;
	}
}
