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
    public GameObject BonfireList;

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
            HardCodeNewGameCharacterSlot(CurrentCharacterData);
            StartCoroutine(LoadWorldScene());
            return;
        }

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        if (!SaveFileDataWriter.CheckToSeeIfFileExists())
        { //if this character slot is not take then we are going to use it
            CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            CurrentCharacterData = new CharacterSaveData();
            HardCodeNewGameCharacterSlot(CurrentCharacterData);
            StartCoroutine(LoadWorldScene());
            return;
        }

        SaveFileDataWriter.SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!SaveFileDataWriter.CheckToSeeIfFileExists())
        { //if this character slot is not take then we are going to use it
            CurrentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            CurrentCharacterData = new CharacterSaveData();
            HardCodeNewGameCharacterSlot(CurrentCharacterData);
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
            BonfireList = GameObject.FindGameObjectWithTag("LayerManager");
            BonfireList.GetComponent<LayerManagerV2>().Bonfire_1.GetComponent<Bonfire>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
            BonfireList.GetComponent<LayerManagerV2>().Bonfire_2.GetComponent<Bonfire>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
            BonfireList.GetComponent<LayerManagerV2>().Bonfire_3.GetComponent<Bonfire>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);

            //get list of EnemySaveManagers
            EnemySaveManagerList = null;
            EnemySaveManagerList = BonfireList.GetComponent<LayerManagerV2>().Bonfire_1.GetComponent<Bonfire>().EnemySaveManagerList;
            if (EnemySaveManagerList != null)
            {
                foreach (GameObject Enemy in EnemySaveManagerList)
                {
                    //Pass each enemy data,from game to file
                    Enemy.GetComponent<EnemySaveManager>().SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
                }
            }

            Player.GetComponent<PlayerManager>().OscarManager.SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);

        }
        //clear the list of enemys
        EnemySaveManagerList = null;
        //clear the list of doors
        DoorSaveManagerList = null;

        // write the ifno onto a json file, saved to this machine
        SaveFileDataWriter.CreateNewCharacterSaveFile(CurrentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        // choose a file to delete based on name
        SaveFileName = DecideCharacterSaveFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

        SaveFileDataWriter = new SaveFileDataWriter();

        SaveFileDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
        SaveFileDataWriter.SaveFileName = SaveFileName;

        SaveFileDataWriter.DeleteSaveFile();
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
        BonfireList = GameObject.FindGameObjectWithTag("LayerManager");
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_1.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_2.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_3.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);

        Player.GetComponent<PlayerManager>().OscarManager.LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        Player.GetComponent<PlayerManager>().OscarManager.ManualStart();

        yield return null;
    }





    public void HardCodeNewGameCharacterSlot(CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.FinishedGame = false;

        // player world position
        CurrentCharacterData.xPos = -90f;
        CurrentCharacterData.yPos = -18f;
        CurrentCharacterData.PlayerLayer = "Middle";

        //player stuff
        CurrentCharacterData.Estus = 0;
        CurrentCharacterData.MaxEstus = 0;
        CurrentCharacterData.HP = 100;
        CurrentCharacterData.TimePlayed = 0;

        //bonfires
        CurrentCharacterData.LastBonfireVisited = 0;
        CurrentCharacterData.BonfireEverUsed_1 = false;
        CurrentCharacterData.BonfireEverUsed_2 = false;
        CurrentCharacterData.BonfireEverUsed_3 = false;

        //door data
    CurrentCharacterData.BoulderUsed = false;
    CurrentCharacterData.DoorState_A = "Open";
    CurrentCharacterData.DoorState_C = "Locked";
    CurrentCharacterData.DoorState_E = "Closed";
    CurrentCharacterData.DoorState_F1 = "Open";
    CurrentCharacterData.DoorState_H = "Closed";
    CurrentCharacterData.DoorState_J2 = "OneSided";
    CurrentCharacterData.DoorState_K = "Closed";
    CurrentCharacterData.DoorState_M1 = "Open";
    CurrentCharacterData.DoorState_M2 = "Open";
    CurrentCharacterData.DoorState_N = "Closed";
    CurrentCharacterData.DoorState_O = "Closed";
    CurrentCharacterData.DoorState_P = "Closed";
    CurrentCharacterData.DoorState_S = "Locked";
    CurrentCharacterData.DoorState_T = "Closed";
    CurrentCharacterData.DoorState_U = "Closed";

        //Oscar data
        CurrentCharacterData.OscarState = "A";
        CurrentCharacterData.CurrentTextLine = 0;
        CurrentCharacterData.MoveInteractionOnLoad = false;
        CurrentCharacterData.IsOscarDead = false;

        //Boss date
        CurrentCharacterData.DemonArenaState = "FirstTime";

        //enemy data
        CurrentCharacterData.EnemyBehaviour_1 = "Idle";
        CurrentCharacterData.EnemyHealth_1 = 3;
        CurrentCharacterData.EnemyPosX_1 = 4;
        CurrentCharacterData.EnemyPosY_1 = -1.8f;
        CurrentCharacterData.EnemyDirection_1 = -1;

        CurrentCharacterData.EnemyBehaviour_2 = "Idle";
        CurrentCharacterData.EnemyHealth_2 = 3;
        CurrentCharacterData.EnemyPosX_2 = 17.5f;
        CurrentCharacterData.EnemyPosY_2 = -1.8f;
        CurrentCharacterData.EnemyDirection_2 = 1;

        CurrentCharacterData.EnemyBehaviour_3 = "Idle";
        CurrentCharacterData.EnemyHealth_3 = 11;
        CurrentCharacterData.EnemyPosX_3 = 50;
        CurrentCharacterData.EnemyPosY_3 = 4.2f;
        CurrentCharacterData.EnemyDirection_3 = -1;

        CurrentCharacterData.EnemyBehaviour_4 = "";
        CurrentCharacterData.EnemyHealth_4 = 11;
        CurrentCharacterData.EnemyPosX_4 = 90;
        CurrentCharacterData.EnemyPosY_4 = 15.2f;
        CurrentCharacterData.EnemyDirection_4 = -1;

        CurrentCharacterData.EnemyBehaviour_5 = "";
        CurrentCharacterData.EnemyHealth_5 = 24;
        CurrentCharacterData.EnemyPosX_5 = 110;
        CurrentCharacterData.EnemyPosY_5 = 23.2f;
        CurrentCharacterData.EnemyDirection_5 = -1;

        CurrentCharacterData.EnemyBehaviour_6 = "";
        CurrentCharacterData.EnemyHealth_6 = 11;
        CurrentCharacterData.EnemyPosX_6 = 123;
        CurrentCharacterData.EnemyPosY_6 = 23.2f;
        CurrentCharacterData.EnemyDirection_6 = -1;

        CurrentCharacterData.EnemyBehaviour_7 = "";
        CurrentCharacterData.EnemyHealth_7 = 24;
        CurrentCharacterData.EnemyPosX_7 = 0;
        CurrentCharacterData.EnemyPosY_7 = 0;
        CurrentCharacterData.EnemyDirection_7 = -1;

        CurrentCharacterData.EnemyBehaviour_8 = "";
        CurrentCharacterData.EnemyHealth_8 = 24;
        CurrentCharacterData.EnemyPosX_8 = 0;
        CurrentCharacterData.EnemyPosY_8 = 0;
        CurrentCharacterData.EnemyDirection_8 = -1;

        CurrentCharacterData.EnemyBehaviour_9 = "";
        CurrentCharacterData.EnemyHealth_9 = 11;
        CurrentCharacterData.EnemyPosX_9 = 145;
        CurrentCharacterData.EnemyPosY_9 = 23.2f;
        CurrentCharacterData.EnemyDirection_9 = 1;

        CurrentCharacterData.EnemyBehaviour_10 = "";
        CurrentCharacterData.EnemyHealth_10 = 11;
        CurrentCharacterData.EnemyPosX_10 = 156;
        CurrentCharacterData.EnemyPosY_10 = 5.2f;
        CurrentCharacterData.EnemyDirection_10 = -1;

        CurrentCharacterData.EnemyBehaviour_11 = "";
        CurrentCharacterData.EnemyHealth_11 = 11;
        CurrentCharacterData.EnemyPosX_11 = 171;
        CurrentCharacterData.EnemyPosY_11 = 9;
        CurrentCharacterData.EnemyDirection_11 = -1;

        CurrentCharacterData.EnemyBehaviour_12 = "";
        CurrentCharacterData.EnemyHealth_12 = 24;
        CurrentCharacterData.EnemyPosX_12 = 0;
        CurrentCharacterData.EnemyPosY_12 = 0;
        CurrentCharacterData.EnemyDirection_12 = -1;

        CurrentCharacterData.EnemyBehaviour_13 = "";
        CurrentCharacterData.EnemyHealth_13 = 24;
        CurrentCharacterData.EnemyPosX_13 = 0;
        CurrentCharacterData.EnemyPosY_13 = 0;
        CurrentCharacterData.EnemyDirection_13 = -1;

        CurrentCharacterData.EnemyBehaviour_14 = "";
        CurrentCharacterData.EnemyHealth_14 = 11;
        CurrentCharacterData.EnemyPosX_14 = 180;
        CurrentCharacterData.EnemyPosY_14 = 22.2f;
        CurrentCharacterData.EnemyDirection_14 = -1;

        CurrentCharacterData.EnemyBehaviour_15 = "";
        CurrentCharacterData.EnemyHealth_15 = 11;
        CurrentCharacterData.EnemyPosX_15 = 203;
        CurrentCharacterData.EnemyPosY_15 = 17.2f;
        CurrentCharacterData.EnemyDirection_15 = -1;

        CurrentCharacterData.EnemyBehaviour_16 = "";
        CurrentCharacterData.EnemyHealth_16 = 24;
        CurrentCharacterData.EnemyPosX_16 = 0;
        CurrentCharacterData.EnemyPosY_16 = 0;
        CurrentCharacterData.EnemyDirection_16 = -1;

        CurrentCharacterData.EnemyBehaviour_17 = "";
        CurrentCharacterData.EnemyHealth_17 = 11;
        CurrentCharacterData.EnemyPosX_17 = 235.5f;
        CurrentCharacterData.EnemyPosY_17 = 17.45f;
        CurrentCharacterData.EnemyDirection_17 = 1;

        CurrentCharacterData.EnemyBehaviour_18 = "";
        CurrentCharacterData.EnemyHealth_18 = 11;
        CurrentCharacterData.EnemyPosX_18 = 172;
        CurrentCharacterData.EnemyPosY_18 = -5.85f;
        CurrentCharacterData.EnemyDirection_18 = -1;

        CurrentCharacterData.EnemyBehaviour_19 = "";
        CurrentCharacterData.EnemyHealth_19 = 24;
        CurrentCharacterData.EnemyPosX_19 = 0;
        CurrentCharacterData.EnemyPosY_19 = 0;
        CurrentCharacterData.EnemyDirection_19 = -1;

        CurrentCharacterData.EnemyBehaviour_20 = "";
        CurrentCharacterData.EnemyHealth_20 = 11;
        CurrentCharacterData.EnemyPosX_20 = 148;
        CurrentCharacterData.EnemyPosY_20 = -5.75f;
        CurrentCharacterData.EnemyDirection_20 = 1;

        CurrentCharacterData.EnemyBehaviour_21 = "";
        CurrentCharacterData.EnemyHealth_21 = 11;
        CurrentCharacterData.EnemyPosX_21 = 161;
        CurrentCharacterData.EnemyPosY_21 = -14.7f;
        CurrentCharacterData.EnemyDirection_21 = -1;

        CurrentCharacterData.EnemyBehaviour_22 = "";
        CurrentCharacterData.EnemyHealth_22 = 11;
        CurrentCharacterData.EnemyPosX_22 = 171;
        CurrentCharacterData.EnemyPosY_22 = -14.7f;
        CurrentCharacterData.EnemyDirection_22 = -1;

        CurrentCharacterData.EnemyBehaviour_23 = "";
        CurrentCharacterData.EnemyHealth_23 = 24;
        CurrentCharacterData.EnemyPosX_23 = 0;
        CurrentCharacterData.EnemyPosY_23 = 0;
        CurrentCharacterData.EnemyDirection_23 = -1;

        CurrentCharacterData.EnemyBehaviour_24 = "";
        CurrentCharacterData.EnemyHealth_24 = 11;
        CurrentCharacterData.EnemyPosX_24 = 177.5f;
        CurrentCharacterData.EnemyPosY_24 = -5.7f;
        CurrentCharacterData.EnemyDirection_24 = 1;


        CurrentCharacterData.EnemyBehaviour_100 = "";
        CurrentCharacterData.EnemyHealth_100 = 24;
        CurrentCharacterData.EnemyPosX_100 = 90;
        CurrentCharacterData.EnemyPosY_100 = 6.15f;

    }
}
