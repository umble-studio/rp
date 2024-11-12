using System;
using RoverDB;
using Rp.Core;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	[Broadcast( NetPermission.HostOnly )]
	private void CreateConversationRpcResponse( ConversationData conversationData )
	{
		// Only the 2 participants can create the conversation

		// var isMe = conversationData.Participants.Exists(x => x.PhoneNumber == Phone.Current.SimCard.PhoneNumber);
		// if ( !isMe ) return;
		//
		// if ( Phone.Current.SimCard.PhoneNumber == firstParticipant.PhoneNumber &&
		//      Phone.Current.SimCard.PhoneNumber == secondParticipant.PhoneNumber )

		_conversations.Add( conversationData );
		Log.Info( "New conversation created and added" );
	}
	
	[Broadcast( NetPermission.HostOnly )]
	private void LoadConversationsRpcResponse( List<ConversationData> conversations )
	{
		_conversations = conversations;
		Log.Info( "LoadConversationsRpcResponse: " + conversations.Count );
	}
}
