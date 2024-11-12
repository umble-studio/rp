namespace Rp.Core.Systems;

public interface INetworkInitializer
{
	public interface IClient
	{
		void InitializeClient();
	}

	public interface IServer
	{
		void InitializeServer( Connection channel );
	}
}
