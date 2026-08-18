using System;
using System.Collections.Generic;
using UnityEngine;

public class StateVariable : ScriptableObject
{
	protected string _cachedName;

	protected bool _nameCached;

	protected List<Action> subscribers = new List<Action>();

	protected static List<StateVariable> stateVariableList;

	protected static readonly string kStatevarPath = "State Variables";

	public string CachedName
	{
		get
		{
			if (!_nameCached)
			{
				_nameCached = true;
				_cachedName = base.name;
			}
			return _cachedName;
		}
	}

	public int IntValue
	{
		get
		{
			return _internalInt;
		}
		set
		{
			_internalInt = value;
		}
	}

	public bool BoolValue
	{
		get
		{
			return IntValue != 0;
		}
		set
		{
			if (IntValue == 0)
			{
				IntValue = (value ? 1 : 0);
			}
			else
			{
				IntValue = (value ? IntValue : 0);
			}
		}
	}

	protected int _internalInt
	{
		get
		{
			return Profile.GetInt(CachedName);
		}
		set
		{
			Profile.SetInt(CachedName, value);
			for (int i = 0; i < subscribers.Count; i++)
			{
				subscribers[i]();
			}
			Profile.Save();
		}
	}

	public static void NotifyAllSubscribers()
	{
		getListIfNeeded();
		foreach (StateVariable stateVariable in stateVariableList)
		{
			if (stateVariable == null)
			{
				Debug.LogErrorFormat("Statevariable `{0}` is null.", stateVariable.name);
				continue;
			}
			for (int i = 0; i < stateVariable.subscribers.Count; i++)
			{
				stateVariable.subscribers[i]();
			}
		}
	}

	public static StateVariable GetStateVariableByName(string name)
	{
		Debug.LogFormat("[StateVariable] Accessing state variable `{0}` by name.", name);
		getListIfNeeded();
		foreach (StateVariable stateVariable in stateVariableList)
		{
			if (stateVariable.CachedName == name)
			{
				return stateVariable;
			}
		}
		Debug.LogErrorFormat("[StateVariable] Couldn't find state variable `{0}` by name.", name);
		return null;
	}

	public void SubscribeToChanges(Action callback)
	{
		subscribers.Add(callback);
	}

	public void UnSubscribeToChanges(Action callback)
	{
		if (subscribers.Contains(callback))
		{
			subscribers.Remove(callback);
			return;
		}
		Debug.LogErrorFormat("Non-subscribed callback `{0}` tried to unsubscribe from statevar `{1}`.", callback, this);
	}

	protected static void getListIfNeeded()
	{
		if (stateVariableList != null)
		{
			return;
		}
		Debug.Log("[StateVariable] Setting up state variable list...");
		stateVariableList = new List<StateVariable>();
		UnityEngine.Object[] array = Resources.LoadAll(kStatevarPath);
		for (int i = 0; i < array.Length; i++)
		{
			StateVariable stateVariable = array[i] as StateVariable;
			if (stateVariable != null)
			{
				stateVariableList.Add(stateVariable);
			}
		}
		Debug.LogFormat("[StateVariable] ...done! Total Statevars: {0}", stateVariableList.Count);
	}
}
