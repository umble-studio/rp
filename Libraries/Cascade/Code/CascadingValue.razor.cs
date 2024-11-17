using System;

namespace Cascade;

public sealed partial class CascadingValue : CascadingPanel
{
	public string Name { get; set; } = null!;
	public object Value { get; set; } = null!;
	public int Hash { get; set; }

	protected override int BuildHash() => HashCode.Combine( Name, Value, Hash );
}
