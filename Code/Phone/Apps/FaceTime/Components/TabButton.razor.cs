using System;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class TabButton : Panel
{
	public string Text { get; set; } = null!;
	public string Icon { get; set; } = null!;

	protected override int BuildHash() => HashCode.Combine( Text, Icon );
}
