using System;
using System.Reflection;

namespace Rp.UI;

internal static class PropertyInfoExtensions
{
	public static bool IsCascadingProperty( this PropertyInfo prop, string name, Type type )
	{
		var attr = prop.GetCustomAttribute<CascadingPropertyAttribute>();
		if ( attr is null ) return false;
		if ( attr.Name != name ) return false;
		
		return type == prop.PropertyType;
	}
}
