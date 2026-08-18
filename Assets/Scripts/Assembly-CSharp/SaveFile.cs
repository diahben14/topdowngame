using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class SaveFile : MonoBehaviour
{
	private static bool fileLoaded;

	private static Dictionary<string, int> intStore = new Dictionary<string, int>();

	private static Dictionary<string, float> floatStore = new Dictionary<string, float>();

	private static Dictionary<string, string> stringStore = new Dictionary<string, string>();

	private const string kSavefileExtension = ".txt";

	private const string defaultSaveDestination = "workingSavefile.txt";

	private const char keyValueSeperator = '|';

	private static string saveDestinationName = "";

	public static string[] GetSaveFileNameList()
	{
		return GetSaveFileNameListFromPath(getPath(""));
	}

	public static string[] GetSaveFileNameListFromPath(string path)
	{
		FileInfo[] files = new DirectoryInfo(path).GetFiles();
		string[] array = new string[files.Length];
		int newSize = 0;
		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Name.Contains(".txt") && !files[i].Name.Contains(".meta"))
			{
				array[newSize++] = files[i].Name;
			}
		}
		Array.Resize(ref array, newSize);
		return array;
	}

	public static string[] GetSaveFilePathListFromPath(string path)
	{
		string[] files = Directory.GetFiles(path);
		int newSize = 0;
		string[] array = new string[files.Length];
		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Contains(".txt") && !files[i].Contains(".meta"))
			{
				array[newSize++] = files[i];
			}
		}
		Array.Resize(ref array, newSize);
		return array;
	}

	public static void LoadFromFile(string filename)
	{
		LoadFromPath(getPath(filename));
	}

	public static void LoadFromPath(string path)
	{
		if (File.Exists(path))
		{
			using (StreamReader streamReader = new StreamReader(path))
			{
				if (streamReader != null)
				{
					clearDictionaries();
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						string[] array = text.Split('|');
						string k = array[0];
						string text2 = array[1];
						int result;
						float result2;
						if (int.TryParse(text2, out result))
						{
							SetInt(k, result);
						}
						else if (float.TryParse(text2, out result2))
						{
							SetFloat(k, result2);
						}
						else
						{
							SetString(k, text2);
						}
					}
				}
			}
			fileLoaded = true;
		}
		else
		{
			Debug.LogErrorFormat("Couldn't get path {0}", path);
		}
	}

	private static void setupDefaultIfNecessary()
	{
		if (!fileLoaded && !(saveDestinationName != ""))
		{
			SetSaveDestination("workingSavefile.txt");
			if (fileExistsInSaveDirectory("workingSavefile.txt"))
			{
				LoadFromFile("workingSavefile.txt");
			}
		}
	}

	public static void SetSaveDestination(string filename)
	{
		if (filename.Contains(".txt"))
		{
			saveDestinationName = filename;
		}
		else
		{
			saveDestinationName = filename + ".txt";
		}
	}

	public static void ResetSaveDestination()
	{
		saveDestinationName = "workingSavefile.txt";
	}

	public static void SaveToDisk()
	{
		if (!fileExistsInSaveDirectory(saveDestinationName))
		{
			createFile(saveDestinationName);
		}
		using (StreamWriter streamWriter = new StreamWriter(getPath(saveDestinationName)))
		{
			if (streamWriter != null)
			{
				foreach (KeyValuePair<string, int> item in intStore)
				{
					streamWriter.WriteLine(item.Key + "|" + item.Value);
				}
				foreach (KeyValuePair<string, float> item2 in floatStore)
				{
					string text = item2.Value.ToString();
					if (text.IndexOf('.') == -1)
					{
						text += ".0";
					}
					streamWriter.WriteLine(item2.Key + "|" + text);
				}
				{
					foreach (KeyValuePair<string, string> item3 in stringStore)
					{
						streamWriter.WriteLine(item3.Key + "|" + item3.Value);
					}
					return;
				}
			}
			Debug.LogErrorFormat("Couldn't get file {0}", saveDestinationName);
		}
	}

	public static void DeleteAll()
	{
		clearDictionaries();
		SaveToDisk();
	}

	public static int GetInt(string k)
	{
		setupDefaultIfNecessary();
		if (!intStore.ContainsKey(k))
		{
			return 0;
		}
		return intStore[k];
	}

	public static float GetFloat(string k)
	{
		setupDefaultIfNecessary();
		if (!floatStore.ContainsKey(k))
		{
			return 0f;
		}
		return floatStore[k];
	}

	public static string GetString(string k)
	{
		setupDefaultIfNecessary();
		if (!stringStore.ContainsKey(k))
		{
			return "";
		}
		return stringStore[k];
	}

	public static void SetInt(string k, int v)
	{
		setupDefaultIfNecessary();
		intStore[k] = v;
	}

	public static void SetFloat(string k, float v)
	{
		setupDefaultIfNecessary();
		floatStore[k] = v;
	}

	public static void SetString(string k, string v)
	{
		setupDefaultIfNecessary();
		stringStore[k] = v;
	}

	private static void clearDictionaries()
	{
		intStore.Clear();
		floatStore.Clear();
		stringStore.Clear();
	}

	private static string getPath(string fileName)
	{
		return Application.persistentDataPath + Path.DirectorySeparatorChar + fileName;
	}

	private static bool fileExistsInSaveDirectory(string filename)
	{
		return File.Exists(getPath(filename));
	}

	private static void createFile(string filename)
	{
		using (File.CreateText(getPath(filename)))
		{
		}
	}
}
