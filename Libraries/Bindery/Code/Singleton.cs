using System.Collections.Generic;
using Sandbox;

namespace Bindery;

public abstract class Singleton : Service, IHotloadManaged
{
	private object? _instance;

	protected override void OnAwake()
	{
		if ( !Active ) return;
		_instance = this;
	}

	protected override void OnDestroy()
	{
		if ( _instance != this ) return;
		_instance = null;
	}

	protected override void OnEnabled()
	{
		if ( _instance is null ) return;

		Log.Error( $"Singleton {_instance.GetType().Name} already exists" );
		Destroy();
	}

	void IHotloadManaged.Destroyed( Dictionary<string, object> state )
	{
		state["IsActive"] = _instance == this;
	}

	void IHotloadManaged.Created( IReadOnlyDictionary<string, object> state )
	{
		if ( state.GetValueOrDefault( "IsActive" ) is false ) return;
		_instance = this;
	}
}

public abstract class Singleton<T> : Component, IHotloadManaged where T : Singleton<T>
{
	public static T Instance { get; private set; } = null!;

	public Singleton()
	{
		Instance = (T)this;
	}

	protected override void OnAwake()
	{
		if ( !Active ) return;
		Instance = (T)this;
	}
	
	protected override void OnDestroy()
	{
		if ( Instance == this )
			Instance = null!;
	}

	void IHotloadManaged.Destroyed( Dictionary<string, object> state )
	{
		state["IsActive"] = Instance == this;
	}

	void IHotloadManaged.Created( IReadOnlyDictionary<string, object> state )
	{
		if ( state.GetValueOrDefault( "IsActive" ) is true )
			Instance = (T)this;
	}
}
