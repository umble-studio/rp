namespace Rp.Core.Systems;

public interface INetworkInitializer
{
	/// <summary>
	/// Called automatically when the client is loaded
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Called automatically when the client is loaded
		/// </summary>
		void InitializeClient();
	}

	/// <summary>
	/// Called automatically when a client is connected to the server
	/// </summary>
	public interface IServer
	{
		/// <summary>
		/// Called automatically when a client is connected to the server
		/// </summary>
		void InitializeServer( Connection channel );
	}
}
