using System;
using RoverDB;
using Rp.Core;

namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	[Broadcast( NetPermission.HostOnly )]
	private void CreateConversationRpcResponse( ConversationData conversationData )
	{
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
