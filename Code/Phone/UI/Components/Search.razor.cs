using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Search : Panel
{
	public string Placeholder { get; set; } = "Search";

	protected override int BuildHash() => HashCode.Combine( Placeholder );
}
