using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class AppBadge : Panel
{
	public int Count { get; set; }
	
	private string CountStr => Count > 9 ? "9+" : Count.ToString();

	private string Root => new CssBuilder()
		.AddClass( "hidden", Count is 0 )
		.Build();

	protected override int BuildHash() => HashCode.Combine( Count );
}
