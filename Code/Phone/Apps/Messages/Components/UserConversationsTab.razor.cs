using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class UserConversationsTab : NavigationPage
{
	private MessageBar _messageBar = null!;
	private PhoneContact _selectedContact = null!;

	private List<PhoneContact> Contacts => Phone.Local.Contacts.All;
	private ConversationService ConversationService => Phone.Local.GetService<ConversationService>();
	private NavHost NavHost => Phone.Local.GetApp<MessagesApp>().NavHost;
	
	public override string PageName => "User Conversations";

	protected override void OnAfterRender( bool firstTime )
	{
		if ( !firstTime ) return;

		_messageBar.OnBack += () =>
		{
			var entry = Phone.Local.History.GetPrevious();
			if ( entry is null ) return;

			Phone.Local.SwitchToApp( entry );
		};
	}

	private void BackToLauncher()
	{
		var entry = Phone.Local.History.GetPrevious();
		if ( entry is null ) return;

		Phone.Local.SwitchToApp( entry );
	}

	private void CreateConversation()
	{
	}

	private void OnConversationSelected( ConversationData conversationData )
	{
		NavHost.GoToChat( conversationData );
	}

	private void OnSelectContact( PhoneContact contact )
	{
		_selectedContact = contact;
		Log.Info( "Selected contact: " + contact.ContactNumber );

		// Check if a conversation exists with this contact
		// If no conversation exists, create one

		// Only one conversation per contact is supported
		// So we don't need to forget about other conversations
		var conversation = ConversationService
			.GetConversations( contact.ContactNumber )
			.FirstOrDefault();

		// We don't have a conversation with this contact
		// So we need to create one and switch to it
		if ( conversation is null )
		{
			Log.Info( "Create new conversation with: " + contact.ContactNumber );

			ConversationService.CreateConversation( contact );
		}
		else
		{
			Log.Info( "Switch to conversation with: " + contact.ContactNumber );
			NavHost.GoToChat( conversation );
		}
	}

	protected override int ShouldRender() => HashCode.Combine(
		base.ShouldRender(),
		ConversationService.Conversations,
		ConversationService.Conversations.Where( x => x.Messages.Count != 0 || x.Participants.Count != 0 ),
		Contacts
	);
}
