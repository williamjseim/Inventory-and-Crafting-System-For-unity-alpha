using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class SaveLoadSystem : MonoBehaviour
{
    string saveFolder = "SaveFolder";

    public string SavePath => $"{Application.persistentDataPath}/{saveFolder}/save.json";

    private void Start()
    {
        if (!File.Exists($"{Application.persistentDataPath}/SaveFolder"))
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/SaveFolder");
        }
    }

    [ContextMenu("save")]
    public void Save(string saveFile)
    {
        var state = LoadFile();
        SaveState(state,saveFile);
        SaveFile(state);
    }

    [ContextMenu("load")]
    public void Load(string saveFile)
    {
        var state = LoadFile();
        LoadState(state,saveFile);
    }

    public void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    Dictionary<string, object> LoadFile()
    {
        if(!File.Exists(SavePath))
        {
            Debug.Log("no savefile found");
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    void SaveState(Dictionary<string, object> state,string saveFile)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.SaveState();
        }
    }

    void LoadState(Dictionary<string, object> state, string saveFile)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object savedstate))
            {
                saveable.LoadSate(savedstate);
            }
        }
    }
}
