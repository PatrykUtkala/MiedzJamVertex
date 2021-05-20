using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveState(bool[] gameState)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamesState.mjv";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameStateData data = new GameStateData(gameState);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameStateData LoadState()
    {
        string path = Application.persistentDataPath + "/gamesState.mjv";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameStateData data = formatter.Deserialize(stream) as GameStateData;
            return data;
        }
        else
        {
            return null;
        }
    }
}
