using System;
using Rp.Phone.Apps.Messages.Services;
using Rp.Phone.UI.Components;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class App : PhoneApp, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent
{
	public override string AppName => "message";
	public override string AppTitle => "Message";
	public override string AppIcon => "textures/ui/phone/app_message.png";
	public override string? AppNotificationIcon => "fluent:comment-48-filled";

	public ConversationService ConversationService { get; private set; } = null!;

	public NavHost NavHost { get; private set; } = null!;
	
	protected override void OnAfterRender( bool firstRender )
	{
		if ( !firstRender ) return;
	
		ConversationService = Phone.GetService<ConversationService>();
		
		// Keep conversation updated when opening the app
		ConversationService.LoadConversations();
	
		NavHost.Navigate<UserConversationsTab>();
	}
	
	// public void SwitchToChat( ConversationData conversationData )
	// {
	// 	_conversationsTab.Hide();
	// 	_chatTab.Show( conversationData );
	// }
	//
	// public void SwitchToConversations()
	// {
	// 	_chatTab.Hide();
	// 	_conversationsTab.Show();
	// }

	void IPhoneEvent.OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		app.Phone.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		app.Phone.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	void IPhoneEvent.OnAppClosed( IPhoneApp app )
	{
		if ( app != this ) return;

		app.Phone.Keyboard.Hide();
	}

	protected override int ShouldRender() => HashCode.Combine( base.ShouldRender(), ConversationService, Phone.Keyboard.IsOpen );
}
