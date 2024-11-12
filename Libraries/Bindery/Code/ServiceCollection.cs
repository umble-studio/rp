using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bindery;

public sealed class ServiceCollection : IServiceCollection, IList<Service>
{
	private readonly List<Service> _services = new();

	public int Count => _services.Count;
	public bool IsReadOnly => true;

	public Service this[ int index ]
	{
		get => throw new System.NotImplementedException();
		set => throw new System.NotImplementedException();
	}

	public IEnumerator<Service> GetEnumerator()
	{
		return _services.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add( Service item )
	{
		_services.Add( item );
	}

	public void Clear()
	{
		_services.Clear();
	}

	public bool Contains( Service item )
	{
		return _services.Contains( item );
	}

	public void CopyTo( Service[] array, int arrayIndex )
	{
		_services.CopyTo( array, arrayIndex );
	}

	public bool Remove( Service item )
	{
		return _services.Remove( item );
	}

	public int IndexOf( Service item )
	{
		return _services.IndexOf( item );
	}

	public void Insert( int index, Service item )
	{
		_services.Insert( index, item );
	}

	public void RemoveAt( int index )
	{
		_services.RemoveAt( index );
	}

	public TService? GetService<TService>()
	{
		return _services.OfType<TService>().FirstOrDefault();
	}
}
