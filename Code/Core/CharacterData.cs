using System;
using RoverDB.Attributes;

namespace Rp.Core;

[Collection( "players/characters" )]
public record CharacterData
{
	[Id, Saved] public Guid Id { get; init; }
	[Saved] public CharacterId CharacterId { get; init; }
	[Saved] public string Firstname { get; init; } = null!;
	[Saved] public string Lastname { get; init; } = null!;
}
