using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class AppLabel : Panel
{
	public string Text { get; set; } = string.Empty;

	protected override int BuildHash() => HashCode.Combine( Text );
}
