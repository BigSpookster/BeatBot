using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    // Variable Fields
    [SerializeField] private string sceneName;
    [SerializeField] private string destinationSpawnPointID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player interacts with object
        if(collision.CompareTag("Player"))
        {
            // Tell next scene which entrance to use before loading next scene
            GameSession.Instance.SetNextSpawnPoint(sceneName, destinationSpawnPointID);
            SceneManager.LoadScene(sceneName);
        }
    }
}