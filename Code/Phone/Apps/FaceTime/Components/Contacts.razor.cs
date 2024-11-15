using System;
using Rp.Phone.UI.Components;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class Contacts : Panel
{
	private bool _isOpen;
	private MessageBar _navigationBar = null!;

	public FaceTimeApp App { get; set; } = null!;

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
	}

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();

	public void Show()
	{
		_isOpen = true;
	}

	public void Hide()
	{
		_isOpen = false;
	}

	private void OnSelectContact( PhoneContact contact )
	{
		App.SwitchToCall( contact );
	}

	protected override int BuildHash() => HashCode.Combine( _isOpen );
}
