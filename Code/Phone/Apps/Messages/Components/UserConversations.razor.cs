using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class UserConversations : Panel
{
	private MessageBar _messageBar = null!;
	private PhoneContact _selectedContact = null!;
	private bool _isOpen;

	public MessagesApp App { get; set; } = null!;

	private List<PhoneContact> Contacts => Phone.Current.Contacts.All;

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( firstTime )
		{
			_messageBar.OnBack += () =>
			{
				var entry = Phone.Current.History.GetPrevious();
				if ( entry is null ) return;

				Phone.Current.SwitchToApp( entry );
			};
		}
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
		var entry = Phone.Current.History.GetPrevious();
		if ( entry is null ) return;

		Phone.Current.SwitchToApp( entry );
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
		var conversation = ConversationService.Instance
			.GetConversations( contact.ContactNumber )
			.FirstOrDefault();

		// We don't have a conversation with this contact
		// So we need to create one and switch to it
		if ( conversation is null )
		{
			Log.Info( "Create new conversation with: " + contact.ContactNumber );

			ConversationService.Instance.CreateConversation( contact );
		}
		else
		{
			Log.Info( "Switch to conversation with: " + contact.ContactNumber );
			App.SwitchToChat( conversation );
		}
	}

	protected override int BuildHash() => HashCode.Combine(
		_isOpen,
		ConversationService.Instance.Conversations,
		ConversationService.Instance.Conversations.Where( x => x.Messages.Any() || x.Participants.Any() ),
		Contacts
	);
}
