using System;
using RoverDB;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	public void SendMessage( PhoneNumber receiver, MessageData message, Guid conversationId )
	{
		SendMessageRpc( Phone.Current.SimCard!.PhoneNumber, receiver, conversationId, message );
	}
}
