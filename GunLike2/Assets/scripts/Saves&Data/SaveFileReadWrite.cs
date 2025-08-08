using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileReadWrite : MonoBehaviour
{
    public class SaveDataFile
    {
        public string usrID = "NULL";
        public List<string> unlocks = new List<string>();
    }

    string file; bool createdNew;
    string filePath; string fileName = "SaveData.json";
    SaveDataFile data;
    void Start()
    {
        createdNew = false;
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        data = new SaveDataFile();

        CheckEmpty();
        if (!createdNew) { Deserialize(); }
        data.usrID = Random.Range(1111, 9999).ToString();
        //Serialize();
    }
    private void OnApplicationQuit()
    {
        Serialize();
    }
    void CheckEmpty()
    {
        if (File.Exists(filePath)) { return; }
        else { CreateSaveData(); }
    }
    void CreateSaveData()
    {
        File.CreateText(filePath);
        SaveDataFile newData = new SaveDataFile();
        newData.usrID = "NEW";
        newData.unlocks.Add("pistol");
        newData.unlocks.Add("revolver");
        File.WriteAllText(filePath, JsonUtility.ToJson(newData));
        file = JsonUtility.ToJson(newData);
        data = JsonUtility.FromJson<SaveDataFile>(file);
        Debug.Log("File created at : " + filePath);
        createdNew = true;
    }
    void Deserialize()
    {
        file = File.ReadAllText(filePath);
        data = JsonUtility.FromJson<SaveDataFile>(file);
    }
    void Serialize()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }
}
