using System;
using System.IO;
using FMOD;
using FMOD.Studio;
using UnityEngine;

namespace FMODUnity
{
	public static class RuntimeUtils
	{
		private const string BankExtension = ".bank";

		public static string GetCommonPlatformPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			return path.Replace('\\', '/');
		}

		public static VECTOR ToFMODVector(this Vector3 vec)
		{
			VECTOR result = default(VECTOR);
			result.x = vec.x;
			result.y = vec.y;
			result.z = vec.z;
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(this Vector3 pos)
		{
			ATTRIBUTES_3D result = default(ATTRIBUTES_3D);
			result.forward = Vector3.forward.ToFMODVector();
			result.up = Vector3.up.ToFMODVector();
			result.position = pos.ToFMODVector();
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(this Transform transform)
		{
			ATTRIBUTES_3D result = default(ATTRIBUTES_3D);
			result.forward = transform.forward.ToFMODVector();
			result.up = transform.up.ToFMODVector();
			result.position = transform.position.ToFMODVector();
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(Transform transform, Rigidbody rigidbody = null)
		{
			ATTRIBUTES_3D result = transform.To3DAttributes();
			if ((bool)rigidbody)
			{
				result.velocity = rigidbody.velocity.ToFMODVector();
			}
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody rigidbody = null)
		{
			ATTRIBUTES_3D result = go.transform.To3DAttributes();
			if ((bool)rigidbody)
			{
				result.velocity = rigidbody.velocity.ToFMODVector();
			}
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(Transform transform, Rigidbody2D rigidbody)
		{
			ATTRIBUTES_3D result = transform.To3DAttributes();
			if ((bool)rigidbody)
			{
				VECTOR velocity = default(VECTOR);
				velocity.x = rigidbody.velocity.x;
				velocity.y = rigidbody.velocity.y;
				velocity.z = 0f;
				result.velocity = velocity;
			}
			return result;
		}

		public static ATTRIBUTES_3D To3DAttributes(GameObject go, Rigidbody2D rigidbody)
		{
			ATTRIBUTES_3D result = go.transform.To3DAttributes();
			if ((bool)rigidbody)
			{
				VECTOR velocity = default(VECTOR);
				velocity.x = rigidbody.velocity.x;
				velocity.y = rigidbody.velocity.y;
				velocity.z = 0f;
				result.velocity = velocity;
			}
			return result;
		}

		internal static FMODPlatform GetCurrentPlatform()
		{
			return FMODPlatform.Windows;
		}

		internal static string GetBankPath(string bankName)
		{
			string streamingAssetsPath = Application.streamingAssetsPath;
			if (Path.GetExtension(bankName) != ".bank")
			{
				return string.Format("{0}/{1}.bank", streamingAssetsPath, bankName);
			}
			return string.Format("{0}/{1}", streamingAssetsPath, bankName);
		}

		internal static string GetPluginPath(string pluginName)
		{
			string text = pluginName + ".dll";
			return string.Concat(Application.dataPath + "/Plugins/", text);
		}

		public static void EnforceLibraryOrder()
		{
			int currentalloced;
			int maxalloced;
			Memory.GetStats(out currentalloced, out maxalloced);
			Guid id;
			Util.parseID("", out id);
		}

		public static bool VerifyPlatformLibsExist()
		{
			string text = Application.dataPath + "/Plugins/";
			if (Path.HasExtension(text) && !File.Exists(text))
			{
				UnityEngine.Debug.LogWarning("[FMOD] Unable to locate '" + text + "'.");
				UnityEngine.Debug.LogWarning("[FMOD] This platform requires verification 'https://fmod.com/profile#permissions' and an additional package from 'https://fmod.com/download'.");
				return false;
			}
			return true;
		}
	}
}
