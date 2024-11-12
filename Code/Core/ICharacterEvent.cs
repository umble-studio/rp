namespace Rp.Core;

public interface ICharacterEvent : ISceneEvent<ICharacterEvent>
{
	void OnCharacterCreated( ushort characterId ) { }
	void OnCharacterDeleted( ushort characterId ) { }
	void OnCharacterUpdated( ushort characterId ) { }
	void OnCharacterSelected( ushort characterId ) { }
	void OnCharacterLoaded( ushort characterId ) { }
}
