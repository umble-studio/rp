namespace Rp.Core.Components;

public interface INetworkInitializer
{
	public interface IClient
	{
		void InitializeClient( Connection channel );
	}

	public interface IServer
	{
		void InitializeServer( Connection channel );
	}
}
