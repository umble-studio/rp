namespace Rp.Phone.Apps.Messages.Services;

public partial class ConversationService
{
	#region Conversation Commands

	[ConCmd( "phone_reload_conversations" )]
	private void LoadConversationsCmd()
	{
		Log.Info( "Reloading conversations.." );
		LoadConversations();
	}
	
	#endregion
}
