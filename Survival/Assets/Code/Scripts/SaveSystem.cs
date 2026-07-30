using System.Collections.Generic;
using System.IO;

using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float currentHealth;
    public float currentStamina;
    public float currentHunger;

    public Vector3 playerPos;

    public List<Item> currentItems;

    public SaveData(PlayerStats playerStat, List<Item> items)
    {
        currentHealth = playerStat.m_currentHealth;
        currentHunger = playerStat.m_currentHunger;
        currentStamina = playerStat.m_currentStamina;
        playerPos = playerStat.gameObject.transform.position;
        currentItems = items;
    }
}
public class SaveSystem 
{

   
    public static void SaveGame(PlayerStats playerStat, List<Item>items )
    {
        SaveData data = new SaveData(playerStat,items);
        string jsonContent = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/SaveGame.json", jsonContent);
    }

    public static SaveData LoadGame()
    {
        SaveData loadData = null;
        string path = Application.persistentDataPath + "/SaveGame.json";
        if(File.Exists(path))
        {
            string loadedJsonContent = File.ReadAllText(path);
            loadData = JsonUtility.FromJson<SaveData>(loadedJsonContent);
        }
        return loadData;
    }


    
}
