namespace Rp.UI.Extensions;

public static class ListExtensions
{
	public static void Push<T>( this IList<T> list, T value )
	{
		list.Insert( 0, value );
	}

	public static void Pop<T>( this IList<T> list, T value )
	{
		list.Remove( value );
	}

	public static void RemoveLast<T>( this IList<T> list )
	{
		list.RemoveAt( list.Count - 1 );
	}
}
