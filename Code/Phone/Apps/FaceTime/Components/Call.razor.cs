using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class Call : Panel
{
	private bool _isOpen;
	
	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
	}

	private string Root => new CssBuilder()
		.AddClass( "show", _isOpen )
		.Build();
	
	public void Show()
	{
		
	}
	
	public void Hide()
	{
		
	}

	protected override int BuildHash() => HashCode.Combine( _isOpen ); 
}
