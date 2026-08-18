using System;
using System.Collections.Generic;

namespace Lib.Build
{
	public class CommandLineParser
	{
		public const string DEFAULT_KEY_LEADING_PATTERN = "--";

		protected Dictionary<string, string> _parsedArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		protected readonly string _keyLeadingPattern;

		public string this[string key]
		{
			get
			{
				return GetValue(key);
			}
			set
			{
				if (key != null)
				{
					_parsedArguments[key] = value;
				}
			}
		}

		public string KeyLeadingPattern
		{
			get
			{
				return _keyLeadingPattern;
			}
		}

		public CommandLineParser()
		{
			_keyLeadingPattern = "--";
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Length != 0)
			{
				Parse(commandLineArgs);
			}
		}

		public bool Contains(string key)
		{
			string adjustedKey;
			return ContainsKey(key, out adjustedKey);
		}

		public virtual string GetPeeledKey(string key)
		{
			if (!IsKey(key))
			{
				return key;
			}
			return key.Substring(_keyLeadingPattern.Length);
		}

		public virtual string GetDecoratedKey(string key)
		{
			if (IsKey(key))
			{
				return key;
			}
			return _keyLeadingPattern + key;
		}

		public virtual bool IsKey(string str)
		{
			return str.StartsWith(_keyLeadingPattern);
		}

		protected virtual void Parse(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == null)
				{
					continue;
				}
				string text = null;
				string text2 = null;
				if (IsKey(args[i]))
				{
					text = args[i];
					if (i + 1 < args.Length && !IsKey(args[i + 1]))
					{
						text2 = args[i + 1];
						i++;
					}
				}
				else
				{
					text2 = args[i];
				}
				if (text == null)
				{
					text = text2;
					text2 = null;
				}
				_parsedArguments[text] = text2;
			}
		}

		public bool GetBoolValue(string key, bool value = false)
		{
			string adjustedKey;
			if (ContainsKey(key, out adjustedKey))
			{
				bool result;
				bool.TryParse(_parsedArguments[adjustedKey], out result);
				return result;
			}
			return value;
		}

		public int GetIntValue(string key, int value = 0)
		{
			string adjustedKey;
			if (ContainsKey(key, out adjustedKey))
			{
				int result;
				int.TryParse(_parsedArguments[adjustedKey], out result);
				return result;
			}
			return value;
		}

		protected virtual string GetValue(string key, string value = null)
		{
			string adjustedKey;
			if (ContainsKey(key, out adjustedKey))
			{
				return _parsedArguments[adjustedKey];
			}
			return value;
		}

		protected virtual bool ContainsKey(string key, out string adjustedKey)
		{
			adjustedKey = key;
			if (_parsedArguments.ContainsKey(key))
			{
				return true;
			}
			if (IsKey(key))
			{
				string peeledKey = GetPeeledKey(key);
				if (_parsedArguments.ContainsKey(peeledKey))
				{
					adjustedKey = peeledKey;
					return true;
				}
				return false;
			}
			string decoratedKey = GetDecoratedKey(key);
			if (_parsedArguments.ContainsKey(decoratedKey))
			{
				adjustedKey = decoratedKey;
				return true;
			}
			return false;
		}
	}
}
