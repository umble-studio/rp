using System;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class Call : Panel
{
	private bool _isOpen;
	private MessageBar _navigationBar = null!;
	private PhoneContact? _phoneContact;
	private string _speakerIcon = "fluent:speaker-off-28-filled";
	private string _muteIcon = "fluent:mic-28-filled";
	private bool _isMuted;
	private bool _isSpeaker;

	public FaceTimeApp App { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
	}

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();

	public void Show( PhoneContact contact )
	{
		_isOpen = true;
		_phoneContact = contact;
		
		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Light;
		Phone.Current.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	public void Hide()
	{
		_isOpen = false;
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
	}

	private void OnBack( PanelEvent e )
	{
		App.SwitchToContacts();
	}

	private void OnContactInfo( PanelEvent e )
	{
	}

	protected override int BuildHash() =>
		HashCode.Combine( _isOpen, _phoneContact, _isSpeaker, _isMuted );
}
