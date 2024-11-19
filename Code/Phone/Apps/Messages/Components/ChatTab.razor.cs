using System;
using System.Runtime.CompilerServices;
using Cascade;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class ChatTab : NavigationPage, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>,
	IKeyboardEvent,
	IMessageEvent, INavigationEvent
{
	private Panel _content = null!;
	private MessageBar _messageBar = null!;
	private ConversationData? _conversation;

	private List<MessageData> Messages => _conversation?.Messages ?? new List<MessageData>();
	private ConversationService ConversationService => Phone.Local.GetService<ConversationService>();
	private NavHost NavHost => Phone.Local.GetApp<MessagesApp>().NavHost;
	
	public override string PageName => "Chat";

	private string Value { get; set; } = string.Empty;

	private string Footer => new CssBuilder()
		.AddClass( "footer" )
		.AddClass( "keyboard-open", Phone.Local.Keyboard.IsOpen )
		.Build();

	protected override void OnAfterRender( bool firstTime )
	{
		if ( !firstTime ) return;
		AcceptsFocus = true;

		_messageBar.OnBack += () =>
		{
			NavHost.Navigate<UserConversationsTab>();
		};
	}

	private void SendMessage( PanelEvent e )
	{
		var message =
			new MessageData
			{
				Author = new MessageAuthor() { PhoneNumber = Phone.Local.SimCard!.PhoneNumber },
				Content = Value,
				Date = DateTime.Now
			};

		Value = string.Empty;
		_content.TryScrollToBottom();

		Sound.Play( "sounds/phone/send_message.sound" );
		Scene.RunEvent<IMessageEvent>( x => x.OnMessageSent( message ), true );

		ConversationService.SendMessageRpcRequest( _conversation!.Id, message );
	}

	void IMessageEvent.OnMessageReceived( MessageData messageData )
	{
		// Don't play sound if the message was sent by the sender of the message
		if ( NavHost.IsOpen<ChatTab>() && messageData.Author.PhoneNumber != Phone.Local.SimCard!.PhoneNumber )
			Sound.Play( "sounds/phone/receive_message.sound" );

		_content.TryScrollToBottom();
	}

	void INavigationEvent.OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not ChatTab ) return;

		Log.Info("ChatTab.OnNavigationOpen: " + args.Length);
		
		if ( args[0] is ConversationData data )
			_conversation = data;

		_content.TryScrollToBottom();

		// TODO - Remove notification when we are seeing the message associated with the notification
		Phone.Local.Notification.ClearPendingNotifications<MessagesApp>();

		Phone.Local.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		Phone.Local.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	private void BackToConversations()
	{
		NavHost.Navigate<UserConversationsTab>();
	}

	void IKeyboardEvent.OnKeyboardEscape()
	{
		// Phone.Current.Keyboard.Hide();
	}

	private void OnInputFocused()
	{
		Phone.Local.Keyboard.Show( this );
	}

	private void OnInputBlurred()
	{
		Phone.Local.Keyboard.Hide();
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), _conversation?.Participants.Count, _conversation?.Messages.Count,
			Phone.Local.Keyboard.IsOpen );
}
