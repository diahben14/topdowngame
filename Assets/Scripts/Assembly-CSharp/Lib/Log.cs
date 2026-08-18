using System.Diagnostics;
using UnityEngine;

namespace Lib
{
	public static class Log
	{
		public static void Initialize()
		{
			UnityEngine.Debug.unityLogger.logEnabled = false;
		}

		[Conditional("ENABLE_SENTRY")]
		public static void Breadcrumb(string message, string context = "Breadcrumb")
		{
		}

		public static void Always(string message, string context = null)
		{
			_Log(LogType.Log, "[LOG] ", message);
		}

		[Conditional("ENABLE_LOG_INFO")]
		[Conditional("UNITY_EDITOR")]
		public static void Info(string message, string context = null)
		{
			_Log(LogType.Log, "[INFO] ", message, context);
		}

		[Conditional("ENABLE_LOG_WARN")]
		[Conditional("UNITY_EDITOR")]
		public static void Warn(string message, string context = null)
		{
			_Log(LogType.Warning, "[WARN] ", message, context);
		}

		[Conditional("ENABLE_LOG_ERROR")]
		[Conditional("UNITY_EDITOR")]
		public static void Error(string message, string context = null)
		{
			_Log(LogType.Error, "[ERROR] ", message, context);
		}

		private static void _Log(LogType logType, string prefix, string message, string context = null)
		{
			if (context != null)
			{
				prefix = prefix + "[" + context + "] ";
			}
			message = prefix + string.Format("[{0} {1:0.#####}] ", Time.frameCount, Time.time) + message;
			UnityEngine.Debug.unityLogger.Log(logType, message);
		}
	}
}
