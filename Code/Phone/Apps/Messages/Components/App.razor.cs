using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class App : PhoneApp, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent
{
	private Chat _chatTab = null!;
	private UserConversations _conversationsTab = null!;

	public override string AppName => "message";
	public override string AppTitle => "Message";
	public override string AppIcon => "textures/ui/phone/app_message.png";
	public override string? AppNotificationIcon => "fluent:comment-48-filled";

	public ConversationService ConversationService { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
		_conversationsTab.Show();
	}

	public void SwitchToChat( ConversationData conversationData )
	{
		_conversationsTab.Hide();
		_chatTab.Show( conversationData );
	}

	public void SwitchToConversations()
	{
		_chatTab.Hide();
		_conversationsTab.Show();
	}

	void IPhoneEvent.OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		Phone.Current.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	void IPhoneEvent.OnAppClosed( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Current.Keyboard.Hide();
	}

	protected override int BuildHash() => HashCode.Combine( Phone.Current.Keyboard.IsOpen );
}
