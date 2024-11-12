using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.UI;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class Chat : Panel, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent,
	IMessageEvent
{
	private Panel _content = null!;
	private MessageBar _messageBar = null!;
	private ConversationData? _conversation;
	private bool _isOpen;

	public MessagesApp App { get; set; } = null!;

	private List<MessageData> Messages => _conversation?.Messages ?? new List<MessageData>();

	private string Value { get; set; } = string.Empty;

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();

	private string Footer => new CssBuilder()
		.AddClass( "footer" )
		.AddClass( "keyboard-open", Phone.Current.Keyboard.IsOpen )
		.Build();
	
	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
		AcceptsFocus = true;

		_messageBar.OnBack += () =>
		{
			App.SwitchToConversations();
		};
	}

	private void SendMessage( PanelEvent e )
	{
		var message =
			new MessageData
			{
				Author = new MessageAuthor()
				{
					// Name = "Me",
					// Avatar = "textures/ui/phone/avatars/avatar_02.jpg",
					PhoneNumber = Phone.Current.SimCard!.PhoneNumber
				},
				Content = Value,
				Date = DateTime.Now
			};

		Value = string.Empty;
		_content.TryScrollToBottom();

		Scene.RunEvent<IMessageEvent>( x => x.OnMessageSent( message ), true );
		
		ConversationService.Instance.SendMessageRpcRequest( Phone.Current.SimCard.PhoneNumber, _conversation!.Id,
			message );
	}

	void IMessageEvent.OnMessageReceived( MessageData messageData )
	{
		Log.Info( "Scrolling to bottom" );
		_content.TryScrollToBottom();
	}

	public void Show( ConversationData conversationData )
	{
		_conversation = conversationData;
		_isOpen = true;
		_content.TryScrollToBottom();

		// TODO - Remove notification when we are seeing the message associated with the notification
		Phone.Current.Notification.ClearPendingNotifications<MessagesApp>();
	}

	public void Hide()
	{
		_isOpen = false;
	}

	private void BackToConversations()
	{
		App.SwitchToConversations();
	}

	void IKeyboardEvent.OnKeyboardEscape()
	{
		Phone.Current.Keyboard.Hide();
	}

	private void OnInputFocused()
	{
		Phone.Current.Keyboard.Show( this );
	}

	private void OnInputBlurred()
	{
		Phone.Current.Keyboard.Hide();
	}

	protected override int BuildHash() =>
		HashCode.Combine( _isOpen, _conversation?.Participants.Count, _conversation?.Messages.Count,
			Phone.Current.Keyboard.IsOpen );
}
