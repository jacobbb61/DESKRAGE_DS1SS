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
    public BoulderManager Boulder;
    public EstusPickUpManager estusPickUpManager;

    [Header("Character Slots")]
    public CharacterSaveData CharacterSlot01;
    public CharacterSaveData CharacterSlot02;
    public CharacterSaveData CharacterSlot03;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
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

        Cursor.visible = false;
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
    public void LoadNightmare()
    {
        CurrentCharacterData.Nightmare = true;
        HardCodeNewNightmareGameCharacterSlot(CurrentCharacterData);
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
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build Nightmare"))
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

                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build Nightmare"))
            {
                 CurrentCharacterData.Estus_1 = estusPickUpManager.EstusPickedUp_1;
                 CurrentCharacterData.Estus_2 = estusPickUpManager.EstusPickedUp_2;
                 CurrentCharacterData.Estus_3 = estusPickUpManager.EstusPickedUp_3;
                 CurrentCharacterData.Estus_4 = estusPickUpManager.EstusPickedUp_4;
                 CurrentCharacterData.Estus_5 = estusPickUpManager.EstusPickedUp_5;
                 CurrentCharacterData.Estus_6 = estusPickUpManager.EstusPickedUp_6;
            }


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
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build"))
            {
            Player.GetComponent<PlayerManager>().OscarManager.SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);
            }


        }

        Boulder = Player.GetComponent<PlayerManager>().BoulderManager;
        if (Boulder != null)
        {
            CurrentCharacterData.BoulderUsed = Boulder.BoulderUsed;
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

        
        if (CurrentCharacterData.Nightmare)
        {
            SceneManager.LoadScene("Build Nightmare", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Build", LoadSceneMode.Single);
        }

        yield return new WaitForSeconds(.5f);

        //get list of all EnemySaveManager scripts in the scene 
        //Pass each enemy data,from file, to the player in game

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

        Boulder = Player.GetComponent<PlayerManager>().BoulderManager;
        if (Boulder != null)
        {
            Boulder.BoulderUsed = CurrentCharacterData.BoulderUsed;
        }


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build Nightmare"))
        {
            estusPickUpManager = GameObject.FindGameObjectWithTag("EstusManager").GetComponent<EstusPickUpManager>();

            estusPickUpManager.EstusPickedUp_1 = CurrentCharacterData.Estus_1;
            estusPickUpManager.EstusPickedUp_2 = CurrentCharacterData.Estus_2;
            estusPickUpManager.EstusPickedUp_3 = CurrentCharacterData.Estus_3;
            estusPickUpManager.EstusPickedUp_4 = CurrentCharacterData.Estus_4;
            estusPickUpManager.EstusPickedUp_5 = CurrentCharacterData.Estus_5;
            estusPickUpManager.EstusPickedUp_6 = CurrentCharacterData.Estus_6;

            estusPickUpManager.ManualStart();
        }



        //get list of all Bonfire scripts in the scene 
        //Pass each door data,from file, to the player in game
        BonfireList = null;
        BonfireList = GameObject.FindGameObjectWithTag("LayerManager");
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_1.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_2.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        BonfireList.GetComponent<LayerManagerV2>().Bonfire_3.GetComponent<Bonfire>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build"))
        { 
            Player.GetComponent<PlayerManager>().OscarManager.LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
        Player.GetComponent<PlayerManager>().OscarManager.ManualStart();
        }

        EnemySaveManagerList = null;

        //  EnemySaveManagerList = GameObject.FindGameObjectsWithTag("Enemy");
        //  EnemySaveManagerList = GameObject.FindGameObjectWithTag("EnemyList").GetComponent<EnemyList>().EnemySaveManagerList;

        EnemySaveManagerList = BonfireList.GetComponent<LayerManagerV2>().Bonfire_1.GetComponent<Bonfire>().EnemySaveManagerList;
        if (EnemySaveManagerList != null)
        {
            foreach (GameObject Enemy in EnemySaveManagerList)
            {
                Enemy.GetComponent<EnemySaveManager>().LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);
            }
        }

        // pass the player info, from file, to the player in game
        Player = null;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        Player.LoadGameFromDataToCurrentCharacterData(ref CurrentCharacterData);




        yield return null;
    }





    public void HardCodeNewGameCharacterSlot(CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.FinishedGame = false;

        // player world position
        CurrentCharacterData.xPos = -88.5f;
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
    CurrentCharacterData.DoorState_A = "Locked";
    CurrentCharacterData.DoorState_C = "Locked";
    CurrentCharacterData.DoorState_E = "Closed";
    CurrentCharacterData.DoorState_F1 = "Open";
    CurrentCharacterData.DoorState_H = "Locked";
    CurrentCharacterData.DoorState_J2 = "OneSided";
    CurrentCharacterData.DoorState_K = "Locked";
    CurrentCharacterData.DoorState_M1 = "Open";
    CurrentCharacterData.DoorState_M2 = "Open";
    CurrentCharacterData.DoorState_N = "Closed";
    CurrentCharacterData.DoorState_O = "UnBroken";
    CurrentCharacterData.DoorState_P = "Closed";
    CurrentCharacterData.DoorState_S = "Locked";
    CurrentCharacterData.DoorState_T = "Closed";
    CurrentCharacterData.DoorState_U = "Closed";

        //Oscar data
        CurrentCharacterData.OscarState = "Null";
        CurrentCharacterData.CurrentTextLine = 0;
        CurrentCharacterData.MoveInteractionOnLoad = false;
        CurrentCharacterData.IsOscarDead = false;

        //Boss date
        CurrentCharacterData.DemonArenaState = "FirstTime";
        CurrentCharacterData.DemonDeaths = 0;
        CurrentCharacterData.PursuerArenaState = "FirstTime";

        //enemy data
        CurrentCharacterData.EnemyBehaviour_1 = "Idle"; //A jailcells
        CurrentCharacterData.EnemyHealth_1 = 25;
        CurrentCharacterData.EnemyPosX_1 = 4;
        CurrentCharacterData.EnemyPosY_1 = -1.8f;
        CurrentCharacterData.EnemyDirection_1 = -1;

        CurrentCharacterData.EnemyBehaviour_2 = "Idle"; //A jailcells
        CurrentCharacterData.EnemyHealth_2 = 25;
        CurrentCharacterData.EnemyPosX_2 = 17.5f;
        CurrentCharacterData.EnemyPosY_2 = -1.8f;
        CurrentCharacterData.EnemyDirection_2 = 1;

        CurrentCharacterData.EnemyBehaviour_3 = "Idle"; //C jailcells
        CurrentCharacterData.EnemyHealth_3 = 45;
        CurrentCharacterData.EnemyPosX_3 = 50;
        CurrentCharacterData.EnemyPosY_3 = 4.2f;
        CurrentCharacterData.EnemyDirection_3 = -1;

        CurrentCharacterData.EnemyBehaviour_4 = ""; //B archer
        CurrentCharacterData.EnemyHealth_4 = 30;
        CurrentCharacterData.EnemyPosX_4 = 90;
        CurrentCharacterData.EnemyPosY_4 = 15.2f;
        CurrentCharacterData.EnemyDirection_4 = -1;

        CurrentCharacterData.EnemyBehaviour_5 = ""; //C boulder lad
        CurrentCharacterData.EnemyHealth_5 = 45;
        CurrentCharacterData.EnemyPosX_5 = 110;
        CurrentCharacterData.EnemyPosY_5 = 23.2f;
        CurrentCharacterData.EnemyDirection_5 = -1;

        CurrentCharacterData.EnemyBehaviour_6 = ""; //B archer past boulder
        CurrentCharacterData.EnemyHealth_6 = 35;
        CurrentCharacterData.EnemyPosX_6 = 123;
        CurrentCharacterData.EnemyPosY_6 = 23.2f;
        CurrentCharacterData.EnemyDirection_6 = -1;

        CurrentCharacterData.EnemyBehaviour_7 = "";   // new undead A front boulder
        CurrentCharacterData.EnemyHealth_7 = 25;
        CurrentCharacterData.EnemyPosX_7 = 128;
        CurrentCharacterData.EnemyPosY_7 = 23.5f;
        CurrentCharacterData.EnemyDirection_7 = 1;

        CurrentCharacterData.EnemyBehaviour_8 = "";   //new undead C front boulder
        CurrentCharacterData.EnemyHealth_8 = 45;
        CurrentCharacterData.EnemyPosX_8 = 142;
        CurrentCharacterData.EnemyPosY_8 = 23.2f;
        CurrentCharacterData.EnemyDirection_8 = 1;

        CurrentCharacterData.EnemyBehaviour_9 = ""; //B archer past boulder
        CurrentCharacterData.EnemyHealth_9 = 35;
        CurrentCharacterData.EnemyPosX_9 = 145;
        CurrentCharacterData.EnemyPosY_9 = 23.2f;
        CurrentCharacterData.EnemyDirection_9 = -1;

        CurrentCharacterData.EnemyBehaviour_10 = ""; //C first outskirts
        CurrentCharacterData.EnemyHealth_10 = 45;
        CurrentCharacterData.EnemyPosX_10 = 185;
        CurrentCharacterData.EnemyPosY_10 = 12.11f;
        CurrentCharacterData.EnemyDirection_10 = -1;

        CurrentCharacterData.EnemyBehaviour_11 = ""; //C second outskirts
        CurrentCharacterData.EnemyHealth_11 = 45;
        CurrentCharacterData.EnemyPosX_11 = 171;
        CurrentCharacterData.EnemyPosY_11 = 9;
        CurrentCharacterData.EnemyDirection_11 = -1;

        CurrentCharacterData.EnemyBehaviour_12 = "";   // new undead A front under
        CurrentCharacterData.EnemyHealth_12 = 25;
        CurrentCharacterData.EnemyPosX_12 = 173.5f;
        CurrentCharacterData.EnemyPosY_12 = -5.75f;
        CurrentCharacterData.EnemyDirection_12 = -1;

        CurrentCharacterData.EnemyBehaviour_13 = "";   //new undead b front under
        CurrentCharacterData.EnemyHealth_13 = 35;
        CurrentCharacterData.EnemyPosX_13 = 183.25f;
        CurrentCharacterData.EnemyPosY_13 = -12.7f;
        CurrentCharacterData.EnemyDirection_13 = -1;

        CurrentCharacterData.EnemyBehaviour_14 = ""; //B outskirts top
        CurrentCharacterData.EnemyHealth_14 = 35;
        CurrentCharacterData.EnemyPosX_14 = 177.65f;
        CurrentCharacterData.EnemyPosY_14 = 22.2f;
        CurrentCharacterData.EnemyDirection_14 = -1;

        CurrentCharacterData.EnemyBehaviour_15 = ""; // C by bridge
        CurrentCharacterData.EnemyHealth_15 = 45;
        CurrentCharacterData.EnemyPosX_15 = 203;
        CurrentCharacterData.EnemyPosY_15 = 17.2f;
        CurrentCharacterData.EnemyDirection_15 = -1;

        CurrentCharacterData.EnemyBehaviour_16 = "";   //new undead c middle upper
        CurrentCharacterData.EnemyHealth_16 = 45;
        CurrentCharacterData.EnemyPosX_16 = 185.11f;
        CurrentCharacterData.EnemyPosY_16 = 21.3f;
        CurrentCharacterData.EnemyDirection_16 = -1;

        CurrentCharacterData.EnemyBehaviour_17 = ""; // B after bridge
        CurrentCharacterData.EnemyHealth_17 = 50;
        CurrentCharacterData.EnemyPosX_17 = 235.5f;
        CurrentCharacterData.EnemyPosY_17 = 17.45f;
        CurrentCharacterData.EnemyDirection_17 = 1;

        CurrentCharacterData.EnemyBehaviour_18 = ""; //C sewer first
        CurrentCharacterData.EnemyHealth_18 = 45;
        CurrentCharacterData.EnemyPosX_18 = 158;
        CurrentCharacterData.EnemyPosY_18 = -5.85f;
        CurrentCharacterData.EnemyDirection_18 = -1;

        CurrentCharacterData.EnemyBehaviour_19 = "";   //new undead b middle upper
        CurrentCharacterData.EnemyHealth_19 = 35;
        CurrentCharacterData.EnemyPosX_19 = 178.65f;
        CurrentCharacterData.EnemyPosY_19 = 12.28f;
        CurrentCharacterData.EnemyDirection_19 = -1;

        CurrentCharacterData.EnemyBehaviour_20 = ""; //B sewer first
        CurrentCharacterData.EnemyHealth_20 = 35;
        CurrentCharacterData.EnemyPosX_20 = 148;
        CurrentCharacterData.EnemyPosY_20 = -5.75f;
        CurrentCharacterData.EnemyDirection_20 = 1;

        CurrentCharacterData.EnemyBehaviour_21 = ""; //C sewer second
        CurrentCharacterData.EnemyHealth_21 = 45;
        CurrentCharacterData.EnemyPosX_21 = 161;
        CurrentCharacterData.EnemyPosY_21 = -14.7f;
        CurrentCharacterData.EnemyDirection_21 = -1;

        CurrentCharacterData.EnemyBehaviour_22 = ""; //C sewer third
        CurrentCharacterData.EnemyHealth_22 = 45;
        CurrentCharacterData.EnemyPosX_22 = 171;
        CurrentCharacterData.EnemyPosY_22 = -14.7f;
        CurrentCharacterData.EnemyDirection_22 = -1;

        CurrentCharacterData.EnemyBehaviour_23 = "";   //new undead A middle upper
        CurrentCharacterData.EnemyHealth_23 = 25;
        CurrentCharacterData.EnemyPosX_23 = 209.1f;
        CurrentCharacterData.EnemyPosY_23 = 17.15f;
        CurrentCharacterData.EnemyDirection_23 = -1;

        CurrentCharacterData.EnemyBehaviour_24 = ""; //B sewer last
        CurrentCharacterData.EnemyHealth_24 = 35;
        CurrentCharacterData.EnemyPosX_24 = 177.5f;
        CurrentCharacterData.EnemyPosY_24 = -5.7f;
        CurrentCharacterData.EnemyDirection_24 = 1;


        CurrentCharacterData.EnemyBehaviour_100 = "";
        CurrentCharacterData.EnemyHealth_100 = 50;
        CurrentCharacterData.EnemyPosX_100 = 90;
        CurrentCharacterData.EnemyPosY_100 = 6.15f;

        CurrentCharacterData.EnemyBehaviour_25 = ""; //C tutorial door
        CurrentCharacterData.EnemyHealth_25 = 45;
        CurrentCharacterData.EnemyPosX_25 = 53.35f;
        CurrentCharacterData.EnemyPosY_25 = 4.38f;
        CurrentCharacterData.EnemyDirection_25 = 1;
    }

    public void HardCodeNewNightmareGameCharacterSlot(CharacterSaveData CurrentCharacterData)
    {
        Debug.Log("New Nightmare");
    CurrentCharacterData.FinishedGame = false;

    // player world position
    CurrentCharacterData.xPos = -88.5f;
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

    CurrentCharacterData.Estus_1 = false;
    CurrentCharacterData.Estus_2 = false;
    CurrentCharacterData.Estus_3 = false;
    CurrentCharacterData.Estus_4 = false;
    CurrentCharacterData.Estus_5 = false;
    CurrentCharacterData.Estus_6 = false;

    //door data
    CurrentCharacterData.BoulderUsed = false;
    CurrentCharacterData.DoorState_A = "Locked";
    CurrentCharacterData.DoorState_C = "Locked";
    CurrentCharacterData.DoorState_E = "Closed";
    CurrentCharacterData.DoorState_F1 = "Open";
    CurrentCharacterData.DoorState_H = "Locked";
    CurrentCharacterData.DoorState_J2 = "OneSided";
    CurrentCharacterData.DoorState_K = "Locked";
    CurrentCharacterData.DoorState_M1 = "Open";
    CurrentCharacterData.DoorState_M2 = "Open";
    CurrentCharacterData.DoorState_N = "Closed";
    CurrentCharacterData.DoorState_O = "UnBroken";
    CurrentCharacterData.DoorState_P = "Closed";
    CurrentCharacterData.DoorState_S = "Locked";
    CurrentCharacterData.DoorState_T = "Closed";
    CurrentCharacterData.DoorState_U = "Closed";

    //Oscar data
    CurrentCharacterData.OscarState = "Null";
    CurrentCharacterData.CurrentTextLine = 0;
    CurrentCharacterData.MoveInteractionOnLoad = false;
    CurrentCharacterData.IsOscarDead = false;

    //Boss date
    CurrentCharacterData.DemonArenaState = "FirstTime";
    CurrentCharacterData.DemonDeaths = 0;
    CurrentCharacterData.PursuerArenaState = "FirstTime";

        float Ahealth = 40;
        float Bhealth = 50;
        float Chealth = 60;

    //enemy data
    CurrentCharacterData.EnemyBehaviour_1 = "Idle"; //A jailcells
    CurrentCharacterData.EnemyHealth_1 = Ahealth;
    CurrentCharacterData.EnemyPosX_1 = 4;
    CurrentCharacterData.EnemyPosY_1 = -1.8f;
    CurrentCharacterData.EnemyDirection_1 = -1;

    CurrentCharacterData.EnemyBehaviour_2 = "Idle"; //A jailcells
    CurrentCharacterData.EnemyHealth_2 = Ahealth;
    CurrentCharacterData.EnemyPosX_2 = 17.5f;
    CurrentCharacterData.EnemyPosY_2 = -1.8f;
    CurrentCharacterData.EnemyDirection_2 = 1;

    CurrentCharacterData.EnemyBehaviour_3 = "Idle"; //C jailcells
    CurrentCharacterData.EnemyHealth_3 = Chealth;
    CurrentCharacterData.EnemyPosX_3 = 50;
    CurrentCharacterData.EnemyPosY_3 = 4.2f;
    CurrentCharacterData.EnemyDirection_3 = -1;

    CurrentCharacterData.EnemyBehaviour_4 = ""; //B archer
    CurrentCharacterData.EnemyHealth_4 = Bhealth;
    CurrentCharacterData.EnemyPosX_4 = 90;
    CurrentCharacterData.EnemyPosY_4 = 15.2f;
    CurrentCharacterData.EnemyDirection_4 = -1;

    CurrentCharacterData.EnemyBehaviour_5 = ""; //C boulder lad
    CurrentCharacterData.EnemyHealth_5 = Chealth;
    CurrentCharacterData.EnemyPosX_5 = 110;
    CurrentCharacterData.EnemyPosY_5 = 23.2f;
    CurrentCharacterData.EnemyDirection_5 = -1;

    CurrentCharacterData.EnemyBehaviour_6 = ""; //B archer past boulder
    CurrentCharacterData.EnemyHealth_6 = Bhealth;
    CurrentCharacterData.EnemyPosX_6 = 123;
    CurrentCharacterData.EnemyPosY_6 = 23.2f;
    CurrentCharacterData.EnemyDirection_6 = -1;

    CurrentCharacterData.EnemyBehaviour_7 = "";   // new undead A front boulder
    CurrentCharacterData.EnemyHealth_7 = Ahealth;
    CurrentCharacterData.EnemyPosX_7 = 114.21f;
    CurrentCharacterData.EnemyPosY_7 = 23.5f;
    CurrentCharacterData.EnemyDirection_7 = 1;

    CurrentCharacterData.EnemyBehaviour_8 = "";   //new undead C front boulder
    CurrentCharacterData.EnemyHealth_8 = Chealth;
    CurrentCharacterData.EnemyPosX_8 = 142;
    CurrentCharacterData.EnemyPosY_8 = 23.2f;
    CurrentCharacterData.EnemyDirection_8 = 1;

    CurrentCharacterData.EnemyBehaviour_9 = ""; //B archer past boulder
    CurrentCharacterData.EnemyHealth_9 = Bhealth;
    CurrentCharacterData.EnemyPosX_9 = 145;
    CurrentCharacterData.EnemyPosY_9 = 23.2f;
    CurrentCharacterData.EnemyDirection_9 = -1;

    CurrentCharacterData.EnemyBehaviour_10 = ""; //C first outskirts
    CurrentCharacterData.EnemyHealth_10 = Chealth;
    CurrentCharacterData.EnemyPosX_10 = 185;
    CurrentCharacterData.EnemyPosY_10 = 12.11f;
    CurrentCharacterData.EnemyDirection_10 = -1;

    CurrentCharacterData.EnemyBehaviour_11 = ""; //C second outskirts
    CurrentCharacterData.EnemyHealth_11 = Chealth;
    CurrentCharacterData.EnemyPosX_11 = 171;
    CurrentCharacterData.EnemyPosY_11 = 9;
    CurrentCharacterData.EnemyDirection_11 = -1;

    CurrentCharacterData.EnemyBehaviour_12 = "";   // new undead A front under
    CurrentCharacterData.EnemyHealth_12 = Ahealth;
    CurrentCharacterData.EnemyPosX_12 = 159.79f;
    CurrentCharacterData.EnemyPosY_12 = -5.75f;
    CurrentCharacterData.EnemyDirection_12 = -1;

    CurrentCharacterData.EnemyBehaviour_13 = "";   //new undead b front under
    CurrentCharacterData.EnemyHealth_13 = Bhealth;
    CurrentCharacterData.EnemyPosX_13 = 183.25f;
    CurrentCharacterData.EnemyPosY_13 = -12.7f;
    CurrentCharacterData.EnemyDirection_13 = -1;

    CurrentCharacterData.EnemyBehaviour_14 = ""; //B outskirts top
    CurrentCharacterData.EnemyHealth_14 = Bhealth;
    CurrentCharacterData.EnemyPosX_14 = 177.65f;
    CurrentCharacterData.EnemyPosY_14 = 22.2f;
    CurrentCharacterData.EnemyDirection_14 = -1;

    CurrentCharacterData.EnemyBehaviour_15 = ""; // C by bridge
    CurrentCharacterData.EnemyHealth_15 = Chealth;
    CurrentCharacterData.EnemyPosX_15 = 203;
    CurrentCharacterData.EnemyPosY_15 = 17.2f;
    CurrentCharacterData.EnemyDirection_15 = -1;

    CurrentCharacterData.EnemyBehaviour_16 = "";   //new undead c middle upper
    CurrentCharacterData.EnemyHealth_16 = Chealth;
    CurrentCharacterData.EnemyPosX_16 = 196f;
    CurrentCharacterData.EnemyPosY_16 = 22.2f;
    CurrentCharacterData.EnemyDirection_16 = -1;

    CurrentCharacterData.EnemyBehaviour_17 = ""; // B after bridge
    CurrentCharacterData.EnemyHealth_17 = Bhealth;
    CurrentCharacterData.EnemyPosX_17 = 235.5f;
    CurrentCharacterData.EnemyPosY_17 = 17.45f;
    CurrentCharacterData.EnemyDirection_17 = 1;

    CurrentCharacterData.EnemyBehaviour_18 = ""; //C sewer first
    CurrentCharacterData.EnemyHealth_18 = Chealth;
    CurrentCharacterData.EnemyPosX_18 = 166.36f;
    CurrentCharacterData.EnemyPosY_18 = -5.85f;
    CurrentCharacterData.EnemyDirection_18 = -1;

    CurrentCharacterData.EnemyBehaviour_19 = "";   //new undead b middle upper
    CurrentCharacterData.EnemyHealth_19 = Bhealth;
    CurrentCharacterData.EnemyPosX_19 = 178.65f;
    CurrentCharacterData.EnemyPosY_19 = 12.28f;
    CurrentCharacterData.EnemyDirection_19 = -1;

    CurrentCharacterData.EnemyBehaviour_20 = ""; //B sewer first
    CurrentCharacterData.EnemyHealth_20 = Bhealth;
    CurrentCharacterData.EnemyPosX_20 = 148;
    CurrentCharacterData.EnemyPosY_20 = -5.75f;
    CurrentCharacterData.EnemyDirection_20 = 1;

    CurrentCharacterData.EnemyBehaviour_21 = ""; //C sewer second
    CurrentCharacterData.EnemyHealth_21 = Chealth;
    CurrentCharacterData.EnemyPosX_21 = 161;
    CurrentCharacterData.EnemyPosY_21 = -14.7f;
    CurrentCharacterData.EnemyDirection_21 = -1;

    CurrentCharacterData.EnemyBehaviour_22 = ""; //C sewer third
    CurrentCharacterData.EnemyHealth_22 = Chealth;
    CurrentCharacterData.EnemyPosX_22 = 171;
    CurrentCharacterData.EnemyPosY_22 = -14.7f;
    CurrentCharacterData.EnemyDirection_22 = -1;

    CurrentCharacterData.EnemyBehaviour_23 = "";   //new undead A middle upper
    CurrentCharacterData.EnemyHealth_23 = Ahealth;
    CurrentCharacterData.EnemyPosX_23 = 209.1f;
    CurrentCharacterData.EnemyPosY_23 = 17.15f;
    CurrentCharacterData.EnemyDirection_23 = -1;

    CurrentCharacterData.EnemyBehaviour_24 = ""; //B sewer last
    CurrentCharacterData.EnemyHealth_24 = Bhealth;
    CurrentCharacterData.EnemyPosX_24 = 177.5f;
    CurrentCharacterData.EnemyPosY_24 = -5.7f;
    CurrentCharacterData.EnemyDirection_24 = 1;


    CurrentCharacterData.EnemyBehaviour_100 = "";
    CurrentCharacterData.EnemyHealth_100 = 50;
    CurrentCharacterData.EnemyPosX_100 = 90;
    CurrentCharacterData.EnemyPosY_100 = 6.15f;

        ////////////////////////////////////////////////////////////////////////////////// new

    CurrentCharacterData.EnemyBehaviour_25 = ""; //C tutorial door
    CurrentCharacterData.EnemyHealth_25 = Chealth;
    CurrentCharacterData.EnemyPosX_25 = 53.35f;
    CurrentCharacterData.EnemyPosY_25 = 4.38f;
    CurrentCharacterData.EnemyDirection_25 = 1;

    CurrentCharacterData.EnemyBehaviour_26 = ""; //A tutorial door
    CurrentCharacterData.EnemyHealth_26 = Ahealth;
    CurrentCharacterData.EnemyPosX_26 = 68.6f;
    CurrentCharacterData.EnemyPosY_26 = 7.15f;
    CurrentCharacterData.EnemyDirection_26 = 1;

    CurrentCharacterData.EnemyBehaviour_27 = ""; //B outskirts upper
    CurrentCharacterData.EnemyHealth_27 = Bhealth;
    CurrentCharacterData.EnemyPosX_27 = 182.9f;
    CurrentCharacterData.EnemyPosY_27 = 22.28f;
    CurrentCharacterData.EnemyDirection_27 = 1;

    CurrentCharacterData.EnemyBehaviour_28 = ""; //B demon room
    CurrentCharacterData.EnemyHealth_28 = Bhealth;
    CurrentCharacterData.EnemyPosX_28 = 133.35f;
    CurrentCharacterData.EnemyPosY_28 = 26.53f;
    CurrentCharacterData.EnemyDirection_28 = 1;

    CurrentCharacterData.EnemyBehaviour_29 = ""; //B out skirts lower
    CurrentCharacterData.EnemyHealth_29 = Bhealth;
    CurrentCharacterData.EnemyPosX_29 = 180.75f;
    CurrentCharacterData.EnemyPosY_29 = -5.74f;
    CurrentCharacterData.EnemyDirection_29 = 1;

    CurrentCharacterData.EnemyBehaviour_30 = ""; //B boulder 
    CurrentCharacterData.EnemyHealth_30 = Bhealth;
    CurrentCharacterData.EnemyPosX_30 = 132.49f;
    CurrentCharacterData.EnemyPosY_30 = 23.2f;
    CurrentCharacterData.EnemyDirection_30 = 1;

    CurrentCharacterData.EnemyBehaviour_31 = ""; //B before persurr 
    CurrentCharacterData.EnemyHealth_31 = Bhealth;
    CurrentCharacterData.EnemyPosX_31 = 201.25f;
    CurrentCharacterData.EnemyPosY_31 = -5.7f;
    CurrentCharacterData.EnemyDirection_31 = 1;





}
}
