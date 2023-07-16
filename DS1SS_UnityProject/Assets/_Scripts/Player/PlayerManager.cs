using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public bool FinishedGame = false;
    public bool StartedGame;
    public float TimePlayedSeconds;
    public string Name = "";

    public int LastBonfireVisited;
    public Bonfire Bonfire_1;
    public Bonfire Bonfire_2;
    public Bonfire Bonfire_3;

    public bool AKey; // Cell key
    public bool KKey;
    public bool SKey;
    public Key AKeyScript;
    public Key SKeyScript;

    public BoulderManager BoulderManager;
    public OscarManager OscarManager;

    public AsylumDemonArena DemonArena;
    public PursuerArena PursuerArena;


    public LayerManagerV2 Layer;
    private PlayerControllerV2 PC;
    private void Start()
    {
        WorldSaveGameManager.Instance.Player = this;
        if (SceneManager.GetActiveScene().name == "Build")
        {
            Layer = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
            PC = GetComponent<PlayerControllerV2>();
            StartedGame = true;
        }

        QualitySettings.vSyncCount = 1;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build")) { TimePlayedSeconds += Time.deltaTime; }
    }


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.FinishedGame = FinishedGame;
        CurrentCharacterData.StartedGame = StartedGame;
        CurrentCharacterData.yPos = transform.position.y;
        CurrentCharacterData.xPos = transform.position.x;
        CurrentCharacterData.PlayerLayer = Layer.CurrentLayer;

        CurrentCharacterData.Estus = PC.CurrentEstus;
        CurrentCharacterData.MaxEstus = PC.MaxEstus;
        CurrentCharacterData.HP = PC.Health;

        CurrentCharacterData.LastBonfireVisited = LastBonfireVisited;



        CurrentCharacterData.TimePlayed = TimePlayedSeconds;

        CurrentCharacterData.AKey = AKey;
        CurrentCharacterData.KKey = KKey;
        CurrentCharacterData.SKey = SKey;

        CurrentCharacterData.BoulderUsed = BoulderManager.BoulderUsed;



        if (DemonArena.currentState == "Active") { DemonArena.currentState = "Idle"; }
        if (PursuerArena.currentState == "Active") { PursuerArena.currentState = "Idle"; }

        CurrentCharacterData.DemonArenaState = DemonArena.currentState;
        CurrentCharacterData.PursuerArenaState = PursuerArena.currentState;


    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        //Name = CurrentCharacterData.CharacterName;
        Vector2 MyPos = new Vector2 (CurrentCharacterData.xPos, CurrentCharacterData.yPos);
        transform.position = MyPos;
        Layer = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
        Layer.CurrentLayer= CurrentCharacterData.PlayerLayer;
        Layer.LoadLayer(CurrentCharacterData.PlayerLayer);

        PC.Health = CurrentCharacterData.HP;
        PC.CurrentEstus = CurrentCharacterData.Estus;
        PC.MaxEstus = CurrentCharacterData.MaxEstus;

        LastBonfireVisited = CurrentCharacterData.LastBonfireVisited;

        TimePlayedSeconds = CurrentCharacterData.TimePlayed;

        AKey = CurrentCharacterData.AKey;
        KKey = CurrentCharacterData.KKey;
        SKey = CurrentCharacterData.SKey;

        AKeyScript.ManualStart();
        SKeyScript.ManualStart();

        BoulderManager.BoulderUsed = CurrentCharacterData.BoulderUsed;

        if (DemonArena != null) { DemonArena.currentState = CurrentCharacterData.DemonArenaState; DemonArena.ManualStart(); }
        if (PursuerArena != null) { PursuerArena.currentState = CurrentCharacterData.PursuerArenaState; PursuerArena.ManualStart(); }
    }
}
