using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class TabButton : Panel
{
	public string Text { get; set; } = null!;
	public string Icon { get; set; } = null!;
	public bool Toggled { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "toggled", Toggled )
		.Build();

	protected override int BuildHash() => HashCode.Combine( Text, Icon, Toggled );
}
