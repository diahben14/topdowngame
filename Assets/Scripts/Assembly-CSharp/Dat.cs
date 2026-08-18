using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class Dat
{
	protected static List<Dat> registration = new List<Dat>();

	protected static Dictionary<string, float> floatDatabase = new Dictionary<string, float>();

	protected static Dictionary<string, string> stringDatabase = new Dictionary<string, string>();

	protected static bool databaseLoaded = false;

	protected float _value;

	public object _enumValue;

	protected Type enumType;

	protected string id;

	protected static string pathToDataFolder
	{
		get
		{
			return Application.streamingAssetsPath + "/data";
		}
	}

	public float Value
	{
		get
		{
			if (enumType == null)
			{
				return _value;
			}
			error("Tried to get Value from enum Dat `{0}`", id);
			return 0f;
		}
		protected set
		{
			_value = value;
		}
	}

	public int IntValue
	{
		get
		{
			return Mathf.RoundToInt(Value);
		}
	}

	public object EnumValue
	{
		get
		{
			if (enumType != null)
			{
				return _enumValue;
			}
			error("Tried to get enum value from non-enum Dat.");
			return null;
		}
		protected set
		{
			_enumValue = value;
		}
	}

	public static void Refresh()
	{
		loadDatabaseFromFile();
		int count = registration.Count;
		for (int i = 0; i < count; i++)
		{
			if (registration[i] != null)
			{
				registration[i].cacheValueFromDatabase();
			}
		}
	}

	protected static void loadDatabaseFromFile()
	{
		floatDatabase.Clear();
		stringDatabase.Clear();
		string[] files = Directory.GetFiles(pathToDataFolder);
		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Contains("meta"))
			{
				continue;
			}
			string[] array = loadTextFromFile(files[i]).Split("\n"[0]);
			bool flag = false;
			string[] array2 = null;
			for (int j = 0; j < array.Length; j++)
			{
				string[] array3 = array[j].Split(',');
				if (j == 0 && array3.Length > 2)
				{
					flag = true;
					array2 = new string[array3.Length - 1];
					Array.Copy(array3, 1, array2, 0, array3.Length - 1);
					continue;
				}
				string text = array3[0].Trim();
				if (text.Equals(string.Empty))
				{
					continue;
				}
				if (flag)
				{
					for (int k = 1; k < array3.Length; k++)
					{
						string value = array3[k].Trim();
						addToDatabase(text + "_" + array2[k - 1].Trim(), value);
					}
				}
				else
				{
					string value2 = array3[1].Trim();
					addToDatabase(text, value2);
				}
			}
		}
		databaseLoaded = true;
	}

	protected static string loadTextFromFile(string path)
	{
		try
		{
			StreamReader streamReader = new StreamReader(path, Encoding.Default);
			string result;
			using (streamReader)
			{
				result = streamReader.ReadToEnd();
				streamReader.Close();
			}
			return result;
		}
		catch (Exception ex)
		{
			error("[Problem loading data from Streaming Assets]: {0}", ex.Message);
			return "";
		}
	}

	protected static void debug_printArray(string[] a)
	{
		string text = "{";
		int num = a.Length;
		for (int i = 0; i < num; i++)
		{
			text += a[i];
			text += ((i < num - 1) ? ", " : "");
		}
		text = text + "} (len: " + num + ")";
	}

	protected static void addToDatabase(string key, string value)
	{
		NumberStyles style = NumberStyles.Float;
		CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
		float result;
		if (float.TryParse(value, style, provider, out result))
		{
			if (floatDatabase.ContainsKey(key))
			{
				error("Database already contains key `{0}`", key);
			}
			else
			{
				floatDatabase.Add(key, result);
			}
		}
		else if (stringDatabase.ContainsKey(key))
		{
			error("String Database already contains key `{0}`", key);
		}
		else
		{
			stringDatabase.Add(key, value);
		}
	}

	protected static void error(string formatString, params object[] p)
	{
		Debug.LogErrorFormat("-[Dat Error]- " + formatString, p);
	}

	public Dat(string id)
	{
		this.id = id;
		register();
		loadDatabaseIfNeededAndCacheValue();
	}

	public Dat(string id, string prefix)
	{
		this.id = prefix + "_" + id;
		register();
		loadDatabaseIfNeededAndCacheValue();
	}

	public Dat(string id, string prefix, Type enumType)
	{
		this.id = prefix + "_" + id;
		this.enumType = enumType;
		register();
		loadDatabaseIfNeededAndCacheValue();
	}

	~Dat()
	{
		deregister();
	}

	protected void register()
	{
		registration.Add(this);
	}

	protected void deregister()
	{
		registration.Remove(this);
	}

	protected void loadDatabaseIfNeededAndCacheValue()
	{
		if (!databaseLoaded)
		{
			loadDatabaseFromFile();
		}
		cacheValueFromDatabase();
	}

	protected void cacheValueFromDatabase()
	{
		if (enumType == null)
		{
			if (floatDatabase.ContainsKey(id))
			{
				Value = floatDatabase[id];
				return;
			}
			error("Couldn't find `{0}` in database", id);
		}
		else if (stringDatabase.ContainsKey(id))
		{
			string value = stringDatabase[id];
			object obj = Enum.Parse(enumType, value);
			if (obj == null)
			{
				error("Couldn't parse enum from string");
			}
			else
			{
				EnumValue = obj;
			}
		}
		else
		{
			error("Couldn't find enum key `{0}` in string database", id);
		}
	}
}
