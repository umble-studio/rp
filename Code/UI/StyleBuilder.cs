using System;
using System.Text;

namespace Rp.UI;

public sealed class StyleBuilder
{
	private readonly StringBuilder _builder = new();

	public StyleBuilder AddStyle( string name, object value )
	{
		Append( name, value );
		return this;
	}

	public StyleBuilder AddStyle( string name, object value, bool condition )
	{
		if ( condition )
			Append( name, value );

		return this;
	}

	public StyleBuilder AddStyle( string name, object value, Func<bool> condition )
	{
		if ( condition.Invoke() )
			Append( name, value );

		return this;
	}

	private StyleBuilder Append( string name, object value )
	{
		if ( _builder.Length is not 0 )
			_builder.Append( ' ' );

		_builder.Append( name );
		_builder.Append( ": " );
		_builder.Append( value );
		_builder.Append( ';' );

		return this;
	}

	public string Build() => _builder.ToString();
	public override string ToString() => _builder.ToString();
}
