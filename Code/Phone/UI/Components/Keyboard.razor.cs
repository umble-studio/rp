using System;
using Rp.UI;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Keyboard : Panel
{
	public bool IsOpen { get; private set; }
	public bool Absolute { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "show", IsOpen )
		.AddClass( "absolute", Absolute )
		.Build();

	public void Show( Panel? parent = null )
	{
		if ( parent is not null )
		{
			Parent = parent;
			StateHasChanged();
		}

		IsOpen = true;
		Scene.RunEvent<IKeyboardEvent>( x => x.OnKeyboardShow(), true );
	}

	public void Hide()
	{
		IsOpen = false;
		Scene.RunEvent<IKeyboardEvent>( x => x.OnKeyboardHide(), true );
	}

	protected override int BuildHash() => HashCode.Combine( IsOpen, Absolute, Parent );
}
