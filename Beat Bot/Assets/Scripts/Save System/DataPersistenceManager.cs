using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initalizeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private bool loadDataOnNextScene = false;
    private bool isInitialSceneLoad = true;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        // Read existing save data from disk when the game starts
        this.gameData = dataHandler.Load();

        // Only create temporary default data when development mode is enabled
        if(this.gameData == null && initalizeDataIfNull)
        {
            NewGame();
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects =
            FindAllDataPersistenceObjects();

        if((isInitialSceneLoad || loadDataOnNextScene) &&
        gameData != null)
        {
            foreach(IDataPersistence dataPersistenceObj
                    in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        isInitialSceneLoad = false;
        loadDataOnNextScene = false;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        //SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // Start a new game if the data is null and we're configured to initialize data for debugging purposes
        if(this.gameData == null && initalizeDataIfNull)
        {
            NewGame();
        }

        // If no data can be Loaded, don't continue
        if(this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started");
            return;
        }
        // Push the loaded data to all oither scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        // If we don't havce any data to save
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // Pass the data to other scripts sop they can update it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    public void OnApplicationQuit()
    {
        //SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public void PrepareForDataLoad()
    {
        loadDataOnNextScene = true;
    }

    public string GetCheckpointScene()
    {
        if(gameData == null)
        {
            return "";
        }

        return gameData.checkpointScene;
    }
}
