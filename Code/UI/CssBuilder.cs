using System;
using System.Text;

namespace Rp.UI;

public sealed class CssBuilder
{
	private readonly StringBuilder _builder = new();

	public CssBuilder AddClass( string name )
	{
		Append( name );
		return this;
	}

	public CssBuilder AddClass( string name, bool condition )
	{
		if ( condition )
			Append( name );

		return this;
	}

	public CssBuilder AddClass( string name, Func<bool> condition )
	{
		if ( condition.Invoke() )
			Append( name );

		return this;
	}

	private CssBuilder Append( string name )
	{
		if ( _builder.Length is 0 )
			_builder.Append( name );
		else
			_builder.Append( ' ' ).Append( name );

		return this;
	}

	public string Build() => _builder.ToString();
	public override string ToString() => _builder.ToString();
}
