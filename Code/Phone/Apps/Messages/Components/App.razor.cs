using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;
using Rp.UI;

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

	[CascadingProperty( "Phone" )] public Phone Phone { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;

		// Keep conversation updated when opening the app
		ConversationService.Instance.LoadConversations();

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

		Phone.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		Phone.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	void IPhoneEvent.OnAppClosed( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Keyboard.Hide();
	}

	protected override int BuildHash() => HashCode.Combine( Phone, Phone.Keyboard.IsOpen );
}
