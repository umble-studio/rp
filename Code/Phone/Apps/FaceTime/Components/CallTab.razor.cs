using System;
using System.Dynamic;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class CallTab : PhoneNavigationPage, INavigationEvent
{
	private CallSession? _callSession;
	private PhoneContact? _pendingPhoneContact;
	private string _speakerIcon = "fluent:speaker-off-28-filled";
	private string _muteIcon = "fluent:mic-28-filled";
	private bool _isMuted;
	private bool _isSpeaker;

	public override string PageName => "Call";

	private PhoneContact? Caller =>
		_callSession is null ? null : Phone.Local.Contacts.GetContactByNumber( _callSession.Caller );

	private PhoneContact? Callee =>
		_callSession is null ? null : Phone.Local.Contacts.GetContactByNumber( _callSession.Callee );

	private string GetCallDuration()
	{
		if ( _callSession is null ) return "00:00";

		var value = DateTime.Now - _callSession!.StartedAt;
		return value.ToString( @"mm\:ss" );
	}

	private void OnSpeakerClicked( bool toggle )
	{
		_isSpeaker = toggle;
		_speakerIcon = toggle ? "fluent:speaker-28-filled" : "fluent:speaker-off-28-filled";
	}

	private void OnMuteClicked( bool toggle )
	{
		_isMuted = toggle;
		_muteIcon = toggle ? "fluent:mic-off-28-filled" : "fluent:mic-28-filled";

		if ( _isMuted ) Sound.Play( "sounds/phone/call_mute.sound" );
		else Sound.Play( "sounds/phone/call_unmute.sound" );
	}

	private void OnKeypad()
	{
		Host.Navigate<KeypadTab>();
	}

	private void OnBack( PanelEvent e )
	{
		Host.Navigate<FavoriteTab>();
	}

	private void OnContactInfo()
	{
	}

	private void EndCall()
	{
		var callService = Phone.Local.GetService<CallService>();
		callService.EndOutgoingCall();
	}

	public void ShowPendingCallView( PhoneContact phoneContact )
	{
		_callSession = null;
		_pendingPhoneContact = phoneContact;
	}

	public void ShowCallView( CallSession callSession )
	{
		_callSession = callSession;
		_pendingPhoneContact = null;
	}

	public void OnNavigationOpen( INavigationPage page, params object[] args )
	{
		if ( page is not CallTab ) return;

		_pendingPhoneContact = null;
		_callSession = null;

		Phone.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	protected override int ShouldRender() =>
		HashCode.Combine( base.ShouldRender(), _pendingPhoneContact, _callSession, _isSpeaker,
			_isMuted, GetCallDuration() );
}
