using System;
using RoverDB;
using Rp.Core.Managers;
using Rp.UI;

namespace Rp.Core.UI;

public sealed partial class CharacterSelection : PanelComponent
{
	private CharacterData? _selectedCharacter;

	public bool IsOpen { get; private set; }
	public bool CharacterSelected => _selectedCharacter is not null;

	public static CharacterSelection? Instance { get; private set; }

	private string Root => new CssBuilder()
		.AddClass( "hidden", !IsOpen )
		.Build();

	public CharacterSelection()
	{
		Instance = this;
	}

	public void Show()
	{
		IsOpen = true;
	}

	public void Hide()
	{
		IsOpen = false;
	}

	private void OnSelectCharacter( CharacterData character )
	{
		_selectedCharacter = character;
		
		// TODO - Refactor this, this is a hack but it works for now
		CharacterManager.Instance.Current = _selectedCharacter;

		Log.Info( "Select character: " + _selectedCharacter.CharacterId );
		Hide();
	}

	protected override int BuildHash() => HashCode.Combine( 
		CharacterManager.Instance.Characters,
		CharacterManager.Instance.Characters.Count, 
		IsOpen );
}
