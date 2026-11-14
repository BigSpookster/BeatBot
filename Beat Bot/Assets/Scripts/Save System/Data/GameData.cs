using UnityEngine;

[System.Serializable]
public class GameData
{
    
    public int deathCount;              // Track # of times player has died
    //public int healthUpgradesCollected; // Track # of health upgrades the player has collected
    public string checkpointID;
    public string checkpointScene;
    public SerializableDictionary<string, bool> healthUpgradesCollected;

    // Constructor - Values defined here will be the default values the
    //               game starts with when there's no data to be loaded
    public GameData()
    {
        this.deathCount = 0;
        this.checkpointID = "Startpoint";
        this.checkpointScene = "1. Junk_Start";
        this.healthUpgradesCollected = new SerializableDictionary<string, bool>();
    }
}
