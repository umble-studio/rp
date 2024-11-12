namespace Rp.Phone;

public partial class Phone
{
	public AppHistory History { get; } = new();

	public sealed class AppHistory
	{
		/// <summary>
		/// The list of all applications in the history
		/// </summary>
		public List<IPhoneApp> Entries { get; } = new();

		/// <summary>
		/// Pushes an entry at the start of the history
		/// </summary>
		/// <param name="app"></param>
		public void Push( IPhoneApp app )
		{
			Entries.Insert( 0, app );
		}
		
		/// <summary>
		/// Removes the first entry from the history
		/// </summary>
		public void Pop()
		{
			Entries.RemoveAt( 0 );
		}

		/// <summary>
		/// Returns the second entry in the history
		/// </summary>
		/// <returns></returns>
		public IPhoneApp? GetPrevious()
		{
			return Entries.ElementAtOrDefault( 1 );
		}

		/// <summary>
		/// Returns the first entry in the history
		/// </summary>
		/// <returns></returns>
		public IPhoneApp? GetCurrent()
		{
			return Entries.ElementAtOrDefault( 0 );
		}

		/// <summary>
		/// Clears the history
		/// </summary>
		public void Clear()
		{
			Entries.Clear();
		}

		/// <summary>
		/// Returns true if the history is not empty
		/// </summary>
		/// <returns></returns>
		public bool HasHistory()
		{
			return Entries.Count > 0;
		}
	}
}
