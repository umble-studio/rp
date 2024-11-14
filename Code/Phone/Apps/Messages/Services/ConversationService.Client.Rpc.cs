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
}
