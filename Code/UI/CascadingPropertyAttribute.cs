using System;
using Sandbox.UI;

namespace Rp.UI;

[AttributeUsage( AttributeTargets.Property )]
public class CascadingPropertyAttribute : Attribute
{
	public string? Name { get; }

	public CascadingPropertyAttribute( string? name = null )
	{
		Name = name;
	}
}
