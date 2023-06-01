using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public static class FileHandler
{
    public static void SaveToJSON<T> (List<T> toSave, string fileName)
    {
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(fileName), content);
    }

    public static List<T> ReadFromJSON<T>(string path)
    {
        string content = ReadFile(GetPath(path));
        if (string.IsNullOrEmpty(content) || content =="{}")
        {
            List<SaveData> dataList = InitialSaveData();
            SaveToJSON<SaveData>(dataList, path);
            return ReadFromJSON<T>(path);
        }
        List<T> res = JsonHelper.FromJson<T>(content).ToList();

        return res;
    }

    private static string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName + ".json";
    }

    private static void WriteFile(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path)) {
            return File.ReadAllText(path);
        }
        return "";
    }

    private static List<SaveData> InitialSaveData()
    {
        List<SaveData> dataList = new();
        List<CustomDeck> initialDecks = new();
        CustomDeck cd = new()
        {
            DeckName = "Default",
            Cards = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
            IsValid = true
        };
        initialDecks.Add(cd);
        List<int> initialList = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        dataList.Add(new SaveData(initialList, 1000, 0, initialDecks, "Default"));
        return dataList;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T> (T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}