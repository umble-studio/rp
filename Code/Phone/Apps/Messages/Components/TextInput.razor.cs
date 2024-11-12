using System;
using Rp.Phone.UI.Components;
using Rp.UI;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class TextInput : Panel
{
	public string Value { get; set; } = string.Empty;
	public Action? Focused { get; set; }
	public Action? Blurred { get; set; }

	private string Root => new CssBuilder()
		.Build();

	protected override void OnEscape( PanelEvent e )
	{
		Scene.RunEvent<IKeyboardEvent>( x => x.OnKeyboardEscape(), true );
	}

	private void OnKeyPressed( ButtonEvent e )
	{
		Scene.RunEvent<IKeyboardEvent>( x => x.OnKeyboardKeyPressed( e.Button ), true );
	}

	protected override void OnClick( MousePanelEvent e )
	{
		e.StopPropagation();
	}
	
	protected override void OnFocus( PanelEvent e )
	{
		Focused?.Invoke();
	}

	protected override void OnBlur( PanelEvent e )
	{
		Blurred?.Invoke();
	}

	protected override int BuildHash() => HashCode.Combine( Value );
}
