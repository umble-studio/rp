using System;

namespace Bindery.Attributes;

[AttributeUsage( AttributeTargets.Property )]
public sealed class Inject : Attribute
{
	public string? Name { get; init; }

	public Inject( string? name = null )
	{
		Name = name;
	}
}
