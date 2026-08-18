using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class FMODAnimationStateBehaviourEntry
{
	[Range(0f, 1f)]
	public float normalizedTime;

	[EventRef]
	public string fmodEvent;
}
