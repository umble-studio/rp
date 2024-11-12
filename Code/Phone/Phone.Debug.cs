using Rp.Phone.Apps;
using Rp.Phone.UI.Components;

namespace Rp.Phone;

public partial class Phone
{
	[Button( "Home" )]
	private void DebugGoToHome()
	{
		SwitchToApp<LockScreen>();
	}

	[Button( "Launcher" )]
	private void DebugGoToLauncher()
	{
		SwitchToApp<Launcher>();
	}

	[Button( "Send notification" )]
	private void DebugSendNotification()
	{
		var notification = new AppNotificationBuilder( _currentApp! )
			.WithTitle( "Hey" )
			.WithMessage( "Hello world!" )
			.Build();

		Notification.CreateNotification<CallApp>( notification );
	}

	[Button( "Remove notification" )]
	private void DebugRemoveNotification()
	{
		Notification.PendingNotifications.RemoveAt( 0 );
	}

	[Button( "Alert" )]
	private void DebugAlert()
	{
		var alert = new AlertBuilder( _phoneContent )
			.WithTitle( "Hey" )
			.WithMessage( "Hello world!" )
			.WithButton( new Button
			{
				Text = "Accept",
			} )
			.WithButton( new Button
			{
				Text = "Cancel",
				Action = HideAlert,
			} )
			.Build();

		ShowAlert( alert );
	}

	[Button( "Close Alert" )]
	private void DebugCloseAlert()
	{
		HideAlert();
	}
}
