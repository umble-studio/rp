using System;

namespace Cascade;

[AttributeUsage( AttributeTargets.Property )]
public sealed class CascadingPropertyAttribute : Attribute
{
	public string? Name { get; }

	public CascadingPropertyAttribute( string? name = null )
	{
		Name = name;
	}
}
