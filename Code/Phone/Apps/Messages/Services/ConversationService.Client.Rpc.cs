using System;
using Rp.UI.Extensions;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	[Broadcast( NetPermission.HostOnly )]
	private void CreateConversationRpcResponse( ConversationData conversationData )
	{
		_conversations.Add( conversationData );
		Scene.RunEvent<IMessageEvent>( x => x.OnConversationCreated( conversationData ) );

		Log.Info( "New conversation created and added" );
	}

	[Broadcast( NetPermission.HostOnly )]
	private void LoadConversationsRpcResponse( List<ConversationData> conversations )
	{
		_conversations = conversations;
		Log.Info( "LoadConversationsRpcResponse: " + conversations.Count );
	}
	
	[Broadcast( NetPermission.HostOnly )]
	private void SendMessageRpcResponse( Guid conversationId, MessageData message )
	{
		var conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );

		// If one of the participant didn't have opened there phone
		// The conversation will be null because conversation are loaded when the phone is opened
		if ( conversation is null )
		{
			// We load conversations to be sure that the conversation is loaded
			// Maybe need to wait a bit before getting the conversation,
			// Because the conversation might not be inserted in the database yet
			LoadConversations();

			conversation = Conversations.FirstOrDefault( x => x.Id == conversationId );
			if ( conversation is null ) return;
		}

		conversation.Messages.Add( message );
		Scene.RunEvent<IMessageEvent>( x => x.OnMessageReceived( message ), true );
	}
}
