using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;


    public PlayerManager Player;

    [Header("Save/Load")]
    [SerializeField] bool _SaveGame;
    [SerializeField] bool _LoadGame;

    [Header("Save Data Writer")]
    private SaveFileDataWriter SaveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot CurrentCharacterSlotBeingUsed;
    public CharacterSaveData CurrentCharacterData;
    private string SaveFileName;

    [Header("Current Character Data")]
    public GameObject[] EnemySaveManagerList;
    public GameObject[] DoorSaveManagerList;
    public GameObject[] BonfireList;

    [Header("Character Slots")]
    public CharacterSaveData CharacterSlot01;
    public CharacterSaveData CharacterSlot02;
    public CharacterSaveData CharacterSlot03;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterSlots();
    }
    private void Update()
    {
        if (_SaveGame)
        {
            _SaveGame = false;
            SaveGame();
        }

        if (_LoadGame)
        {
            _LoadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot _CharacterSlot)
    {
        string FileName = "";


        switch (_CharacterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                FileName = "CharacterSlot_01";
                break;            
            case CharacterSlot.CharacterSlot_02:
                FileName = "CharacterSlot_02";
                break;            
            case CharacterSlot.CharacterSlot_03:
                FileName = "CharacterSlot_03";
                break;
            default:
                break;
        }
        return FileName;
    }

    public void AttemptCreateNewGame()
    {

        SaveFileDataWriter = new SaveFileDataWriter();
        SaveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
        // check to see if we can create a new save file
        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        
        if (!SaveFileDataWriter.CheckToSeeIfFileExists())
        { //if this character slot is not take then we are going to use it
            CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            CurrentCharacterData = new CharacterSaveData();
            CurrentCharacterData.xPos = -77f;
            CurrentCharacterData.yPos = -17f;
            StartCoroutine(LoadWorldScene());
            return;
        }

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        if (!SaveFileDataWriter.CheckToSeeIfFileExists())
        { //if this character slot is not take then we are going to use it
            CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            CurrentCharacterData = new CharacterSaveData();
            CurrentCharacterData.xPos = -77f;
            CurrentCharacterData.yPos = -17f;
            StartCoroutine(LoadWorldScene());
            return;
        }

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!SaveFileDataWriter.CheckToSeeIfFileExists())
        { //if this character slot is not take then we are going to use it
            CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            CurrentCharacterData = new CharacterSaveData();
            CurrentCharacterData.xPos = -77f;
            CurrentCharacterData.yPos = -17f;
            StartCoroutine(LoadWorldScene());
            return;
        }
        Debug.Log("HERE");
        // if there are no free slots, open no free slots prompt
        MainMenuManager.Instance.OpenNoSlotsMessage();
    }

    public void LoadGame()
    {
        // load a file, with a file name based on the slot
        SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CurrentCharacterSlotBeingUsed);

        SaveFileDataWriter = new SaveFileDataWriter();
        // gernally works on mulitple machine types
        SaveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
        SaveFileDataWriter.SaveFileName = SaveFileName;
        CurrentCharacterData = SaveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        // decide chartacter file name based on character slot
        SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CurrentCharacterSlotBeingUsed);

        SaveFileDataWriter = new SaveFileDataWriter();
        // gernally works on mulitple machine types
        SaveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
        SaveFileDataWriter.SaveFileName = SaveFileName;

        //Pass the player info, from game, to thier save file
        Player = null;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        Player.SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);

        //if we are saving from the build scene do below, if not skip 
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build"))
        {     
            //get list of EnemySaveManagers
            EnemySaveManagerList = null;
            EnemySaveManagerList = GameObject.FindGameObjectsWithTag("Enemy");
            if (EnemySaveManagerList != null)
            {
                foreach (GameObject Enemy in EnemySaveManagerList)
                {
                    //Pass each enemy data,from game to file
                    Enemy.GetComponent<EnemySaveManager>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
                }
            }

            //get list of doors
            DoorSaveManagerList = null;
            DoorSaveManagerList = GameObject.FindGameObjectsWithTag("Door");
            {
                foreach (GameObject Door in DoorSaveManagerList)
                {
                    //Pass each door data,from game to file
                    Door.GetComponent<DoorSaveManager>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
                }
            }
            //get list of all Bonfire scripts in the scene 
            //Pass each door data,from game, to the player in game
            BonfireList = null;
            BonfireList = GameObject.FindGameObjectsWithTag("Bonfire");
            if (BonfireList != null)
            {
                foreach (GameObject Bonfire in BonfireList)
                {
                    //Pass each enemy data,from game to file
                    Bonfire.GetComponent<Bonfire>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
                }
            }
        }
        //clear the list of enemys
        EnemySaveManagerList = null;
        //clear the list of doors
        DoorSaveManagerList = null;

        // write the ifno onto a json file, saved to this machine
        SaveFileDataWriter.CreateNewCharacterSaveFile(CurrentCharacterData);
    }


    // load all character slots on device when starting game
    public void LoadAllCharacterSlots()
    {
        SaveFileDataWriter = new SaveFileDataWriter();
        SaveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        CharacterSlot01 = SaveFileDataWriter.LoadSaveFile();

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        CharacterSlot02 = SaveFileDataWriter.LoadSaveFile();

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        CharacterSlot03 = SaveFileDataWriter.LoadSaveFile();
    }
        
    public IEnumerator LoadWorldScene()
    {
        MainMenuManager.Instance.FMODinstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        MainMenuManager.Instance.FMODinstance.release();
        SceneManager.LoadScene("Build", LoadSceneMode.Single);
        yield return new WaitForSeconds(.1f);

        // pass the player info, from file, to the player in game
        Player = null;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        Player.LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);

        //get list of all EnemySaveManager scripts in the scene 
        //Pass each enemy data,from file, to the player in game
        EnemySaveManagerList = null;
        EnemySaveManagerList = GameObject.FindGameObjectsWithTag("Enemy");
        if (EnemySaveManagerList != null)
        {
            foreach (GameObject Enemy in EnemySaveManagerList)
            {
                Enemy.GetComponent<EnemySaveManager>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
            }
        }
        //get list of all DoorSaveManager scripts in the scene 
        //Pass each door data,from file, to the player in game
        DoorSaveManagerList = null;
        DoorSaveManagerList = GameObject.FindGameObjectsWithTag("Door");
        if (DoorSaveManagerList != null)
        {
            foreach (GameObject Door in DoorSaveManagerList)
            {
                //Pass each enemy data,from file, to the player in game 
                Door.GetComponent<DoorSaveManager>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
            }
        }
        //get list of all Bonfire scripts in the scene 
        //Pass each door data,from file, to the player in game
        BonfireList = null;
        BonfireList = GameObject.FindGameObjectsWithTag("Bonfire");
        if (BonfireList != null)
        {
            foreach (GameObject Bonfire in BonfireList)
            {
                //Pass each enemy data,from file, to the player in game 
                Bonfire.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
            }
        }

        yield return null;
    }
}
