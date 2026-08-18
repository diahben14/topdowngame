using System;
using UnityEngine;

namespace Lib.Build
{
	[Serializable]
	public class SceneReference : ISerializationCallbackReceiver
	{
		[SerializeField]
		private string scenePath = string.Empty;

		public string ScenePath
		{
			get
			{
				return scenePath;
			}
			set
			{
				scenePath = value;
			}
		}

		public static implicit operator string(SceneReference sceneReference)
		{
			return sceneReference.ScenePath;
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
		}
	}
}
