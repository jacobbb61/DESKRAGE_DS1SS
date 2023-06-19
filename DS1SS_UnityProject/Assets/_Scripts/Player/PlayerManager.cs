using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public float TimePlayedSeconds;
    public string Name = "";

    public int LastBonfireVisited;
    public Bonfire Bonfire_1;
    public Bonfire Bonfire_2;
    public Bonfire Bonfire_3;

    public bool CellKey;


    private LayerManagerV2 Layer;
    private PlayerControllerV2 PC;
    private void Start()
    {
        WorldSaveGameManager.Instance.Player = this;
        if (SceneManager.GetActiveScene().name == "Build")
        {
            Layer = GameObject.FindGameObjectWithTag("LayerManager").GetComponent<LayerManagerV2>();
            PC = GetComponent<PlayerControllerV2>();
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Build")) { TimePlayedSeconds += Time.deltaTime; }
    }


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.CharacterName = "character from player manager";
        CurrentCharacterData.yPos = transform.position.y;
        CurrentCharacterData.xPos = transform.position.x;
        CurrentCharacterData.PlayerLayer = Layer.CurrentLayer;

        CurrentCharacterData.Estus = PC.CurrentEstus;
        CurrentCharacterData.MaxEstus = PC.MaxEstus;
        CurrentCharacterData.HP = PC.Health;

        CurrentCharacterData.LastBonfireVisited = LastBonfireVisited;



        CurrentCharacterData.TimePlayed = TimePlayedSeconds;

        CurrentCharacterData.CellKey = CellKey;
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

        CellKey = CurrentCharacterData.CellKey;
    }
}
