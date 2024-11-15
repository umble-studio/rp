namespace Rp.UI;

public interface INavigationEvent : ISceneEvent<INavigationEvent>
{
	void OnNavigationClose( INavigationPage page ) { }
	void OnNavigationOpen( INavigationPage page, params object[] args ) { }
}
