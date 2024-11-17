using System;

namespace Rp.UI;

internal static class PropertyDescriptionExtensions
{
	public static bool IsCascadingProperty( this PropertyDescription prop, string name, Type type )
	{
		var attr = prop.GetCustomAttribute<CascadingPropertyAttribute>();
		
		if ( attr is null ) return false;
		if ( attr.Name != name ) return false;
		
		return type == prop.PropertyType;
	}
}
