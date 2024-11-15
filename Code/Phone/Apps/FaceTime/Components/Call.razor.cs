using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class Call : Panel
{
	private bool _isOpen;
	private PhoneContact? _phoneContact;
	
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

	protected override int BuildHash() => HashCode.Combine( _isOpen, _phoneContact );
}
