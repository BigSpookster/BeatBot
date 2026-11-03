using UnityEngine;

public class SceneSpawnPoint : MonoBehaviour
{
    // Variables Fields
    [SerializeField] private string spawnpointID;

    // Variables to allow other scripts to read spawnpoint data
    public string SpawnPointID => spawnpointID;
}