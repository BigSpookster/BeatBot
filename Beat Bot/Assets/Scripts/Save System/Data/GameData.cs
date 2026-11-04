using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameData
{
    // Track # of times player has died (Here to test this system works for now)
    public int deathCount;

    // Track # of health upgrades the player has collected
    public int healthUpgradesCollected;

    // Constructor - Values defined here will be the default values the
    //               game starts with when there's no data to be loaded
    public GameData()
    {
        this.deathCount = 0;
        this.healthUpgradesCollected = 0;
    }
}
