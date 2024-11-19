namespace Rp.Phone;

/// <summary>
/// A simple key-value store for the phone's cookies.
/// </summary>
public static class PhoneCookie
{
	public static readonly Dictionary<string, object> Cookies = new();

	/// <summary>
	/// Sets the value of a cookie.
	/// </summary>
	/// <param name="key">The key of the cookie.</param>
	/// <param name="value">The value of the cookie.</param>
	public static void SetCookie( string key, object value ) => Cookies[key] = value;

	/// <summary>
	/// Removes a cookie.
	/// </summary>
	/// <param name="key">The key of the cookie.</param>
	public static void RemoveCookie( string key ) => Cookies.Remove( key );

	/// <summary>
	/// Checks if a cookie exists.
	/// </summary>
	/// <param name="key">The key of the cookie.</param>
	/// <returns>True if the cookie exists, false otherwise.</returns>
	public static bool HasCookie( string key ) => Cookies.ContainsKey( key );

	/// <summary>
	/// Gets the value of a cookie.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="key">The key of the cookie.</param>
	/// <returns>The value of the cookie, or null if the cookie does not exist.</returns>
	public static T? GetCookie<T>( string key ) => (T)Cookies[key];

	/// <summary>
	/// Tries to get the value of a cookie.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="key">The key of the cookie.</param>
	/// <param name="value">The value of the cookie, or null if the cookie does not exist.</param>
	/// <returns>True if the cookie exists, false otherwise.</returns>
	public static bool TryGetCookie<T>( string key, out T value )
	{
		if ( Cookies.TryGetValue( key, out var val ) )
		{
			value = (T)val;
			return true;
		}

		value = default!;
		return false;
	}

	/// <summary>
	/// Clears all cookies.
	/// </summary>
	public static void ClearCookies() => Cookies.Clear();
}
