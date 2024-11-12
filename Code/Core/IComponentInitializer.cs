namespace Rp.Core;

public interface IComponentInitializer
{
	/// <summary>
	/// Called only on the host
	/// </summary>
	/// <param name="channel"></param>
	void OnActive( Connection channel );
}
