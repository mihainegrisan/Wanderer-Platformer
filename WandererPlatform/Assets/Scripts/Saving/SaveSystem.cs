using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static object localReferenceOfData;
    private static readonly string Path = Application.persistentDataPath + "/game.save";
    
    public static void Save(object saveData) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Path, FileMode.Create);
        Debug.Log("File Created");
        localReferenceOfData = saveData;
        
        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
        Debug.Log("Game Saved");
    }

    public static object Load() {
        if (File.Exists(Path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(Path, FileMode.Open);
            
            object saveData = (SaveData)formatter.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Loaded from File");
            return saveData;
        }
        
        Debug.Log($"Save file not found in {Path}.");
        //Save(SaveData.current);
        //return localReferenceOfData;
        return null;

    }
}