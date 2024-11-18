using System;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class IncomingCallTab : PhoneNavigationPage, INavigationEvent
{
	private IncomingCallRequest? _incomingCallRequest;

	public override string PageName => "Incoming Call";

	private void AcceptCall()
	{
		var callService = Phone.Local.GetService<CallService>();

		Log.Info( "Accept Call: " + string.Join( ", ", callService.IsOccupied ) );

		if ( callService.IsOccupied ) return;
		if ( _incomingCallRequest is null ) return;

		Log.Info( "Accept incoming call from phone: " + _incomingCallRequest.Caller );
		CallManager.AcceptIncomingCallRpcRequest( _incomingCallRequest.CallId );

		var app = Phone.Local.GetApp<FaceTimeApp>();
		app.NavHost.Navigate<CallTab>();
	}

	private void RejectCall()
	{
		var callService = Phone.Local.GetService<CallService>();
		if ( _incomingCallRequest is null ) return;

		CallManager.RejectIncomingCallRpcRequest( _incomingCallRequest.CallId );
	}

	public void OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not IncomingCallTab ) return;

		if ( args[0] is IncomingCallRequest request )
		{
			_incomingCallRequest = request;
		}

		Phone.Local.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.Local.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), _incomingCallRequest );

	public enum View
	{
		IncomingCall,
		OutgoingCall
	}
}
