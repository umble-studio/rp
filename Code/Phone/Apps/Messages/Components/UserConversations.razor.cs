using System;
using Cascade;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class UserConversations : CascadingPanel
{
	private MessageBar _messageBar = null!;
	private PhoneContact _selectedContact = null!;
	private bool _isOpen;

	public MessagesApp App { get; set; } = null!;

	private List<PhoneContact> Contacts => App.Phone.Contacts.All;

	private PhoneNumber PhoneNumber => App.Phone.SimCard!.PhoneNumber;

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();

	protected override void OnAfterRender( bool firstTime )
	{
		if ( !firstTime ) return;

		_messageBar.OnBack += () =>
		{
			var entry = App.Phone.History.GetPrevious();
			if ( entry is null ) return;

			App.Phone.SwitchToApp( entry );
		};
	}

	public void Show()
	{
		_isOpen = true;
	}

	public void Hide()
	{
		_isOpen = false;
	}

	private void BackToLauncher()
	{
		var entry = App.Phone.History.GetPrevious();
		if ( entry is null ) return;

		App.Phone.SwitchToApp( entry );
	}

	private void CreateConversation()
	{
	}

	private void OnConversationSelected( ConversationData conversationData )
	{
		App.SwitchToChat( conversationData );
	}

	private void OnSelectContact( PhoneContact contact )
	{
		_selectedContact = contact;
		Log.Info( "Selected contact: " + contact.ContactNumber );

		// Check if a conversation exists with this contact
		// If no conversation exists, create one

		// Only one conversation per contact is supported
		// So we don't need to forget about other conversations
		var conversation = App.ConversationService
			.GetConversations( contact.ContactNumber )
			.FirstOrDefault();

		// We don't have a conversation with this contact
		// So we need to create one and switch to it
		if ( conversation is null )
		{
			Log.Info( "Create new conversation with: " + contact.ContactNumber );

			App.ConversationService.CreateConversation( contact );
		}
		else
		{
			Log.Info( "Switch to conversation with: " + contact.ContactNumber );
			App.SwitchToChat( conversation );
		}
	}

	protected override int ShouldRender() => HashCode.Combine(
		_isOpen,
		App.ConversationService.Conversations,
		App.ConversationService.Conversations.Where( x => x.Messages.Count != 0 || x.Participants.Count != 0 ),
		Contacts
	);
}
