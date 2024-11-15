using System;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class CircleButton : Panel
{
	public string Icon { get; set; } = string.Empty;
	public string Text { get; set; } = string.Empty;

	protected override int BuildHash() => HashCode.Combine( Icon, Text );
}
