using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class Call : Panel
{
	private bool _isOpen;
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

	protected override int BuildHash() =>
		HashCode.Combine( _isOpen, _phoneContact, _isSpeaker, _isMuted );
}
