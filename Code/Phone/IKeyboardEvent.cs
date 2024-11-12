namespace Rp.Phone;

public interface IKeyboardEvent : ISceneEvent<IKeyboardEvent>
{
	void OnKeyboardShow() { }
	void OnKeyboardHide() { }
	void OnKeyboardKeyPressed( string key ) { }
	void OnKeyboardEscape() { }
}
