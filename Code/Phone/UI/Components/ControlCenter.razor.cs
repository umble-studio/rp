using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class ControlCenter : Panel
{
	public bool IsOpen { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "open", IsOpen )
		.Build();

	protected override int BuildHash() => HashCode.Combine( IsOpen );
}
