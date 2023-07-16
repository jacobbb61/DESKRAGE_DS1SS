using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
public class OscarManager : MonoBehaviour
{

    /*
     To kill oscar from another script, set the CurrentState to "DF", the CurrentTextLine to 0, then call the next line function

    oscar still needs the layer change from locations and to check if the player is on the smae layer 
     */







    public bool CanInput = true;
    public bool InRange = false;
    public bool IsOscarDead = false; //needs to be saved
    public bool MoveInteractionOnLoad = false; //needs to be saved
    public bool OfferEstus_3 = false; //needs to be saved
    public bool OfferEstus_6 = false; //needs to be saved

    public PlayerControllerV2 PC;
    public InteractableV2 Interactable;
    public SpriteRenderer Assets;
    public GameObject MiddleLayerObject;
    public GameObject FrontLayerObject;
    public GameObject InteractPrompt;

    [Header("State")]
    public string CurrentState; //needs to be saved
    public Animator Anim;
    public List<Vector2> Locations = new List<Vector2>();


    [Header("Dialog UI")]
    public GameObject DialogObject;
    public TextMeshProUGUI DialogTextObject;
    public string CurrentText;
    public int CurrentTextLine = 0;
    public bool IsTalking = false;

    [Header("Question UI")]
    public GameObject QuestionObject;
    public RectTransform QuestionHightlightPos;
    public bool QuestionOrder;
    public bool QuestionOpen;

    [Header("Item UI")]
    public GameObject ItemPopUp;
    public RawImage ItemSymbol;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemQuantity;

    public Texture EstusSymbol;
    public string EstusName;
    public Texture KeySymbol;
    public string KeyName;


    [Header("Dialogue Script")]
    public List<string> StateATextLines = new List<string>();
    public List<string> StateYESTextLines = new List<string>();
    public List<string> StateNOTextLines = new List<string>();
    public List<string> StateBTextLines = new List<string>();
    public List<string> StateCTextLines = new List<string>();
    public List<string> StateDFTextLines = new List<string>();
    public List<string> StateGTextLines = new List<string>();
    public List<string> StateHTextLines = new List<string>();
    public List<string> StateITextLines = new List<string>();

    [Header("Audio Events")]
    private FMOD.Studio.EventInstance instance;
    private Coroutine RoutineToStop;
    public StudioEventEmitter Emitter;
    public List<EventReference> StateAAudioClips = new List<EventReference>();
    public List<EventReference> StateYESAudioClips = new List<EventReference>();
    public List<EventReference> StateNOAudioClips = new List<EventReference>();
    public List<EventReference> StateBAudioClips = new List<EventReference>();
    public List<EventReference> StateCAudioClips = new List<EventReference>();
    public List<EventReference> StateDFAudioClips = new List<EventReference>();
    public List<EventReference> StateGAudioClips = new List<EventReference>();
    public List<EventReference> StateHAudioClips = new List<EventReference>();
    public List<EventReference> StateIAudioClips = new List<EventReference>();

    public void Start()
    {

        QuestionOpen = false;
        QuestionObject.SetActive(false);

        DialogObject.SetActive(false);
        IsTalking = false;

        CurrentTextLine = 0;
        SetLineText();

    }

    private void OnEnable()
    {
        ManualStart();
    }

    public void ManualStart()
    {

        QuestionOpen = false;
        QuestionObject.SetActive(false);

        DialogObject.SetActive(false);
        IsTalking = false;

        //CurrentTextLine = 0;
        SetLineText();

        SetDeath();
        SetLocation();
        if (gameObject.activeInHierarchy) { SetAnimation(); }
    }

    public void DiedToDemon()
    {
         NextInteraction(); 
    }
    public void KilledDemon()
    {
       if (CurrentState == "A" || CurrentState == "B" || CurrentState == "YES" || CurrentState == "NO") { CurrentState = "C"; MoveInteractionOnLoad = false; CurrentTextLine = 0; }
       if (CurrentState == "Null") { CurrentState = "I"; MoveInteractionOnLoad = false; CurrentTextLine = 0; }


        Reload();
    }
    public void Reload()
    {
        QuestionOpen = false;
        QuestionObject.SetActive(false);

        DialogObject.SetActive(false);
        IsTalking = false;

        //CurrentTextLine = 0;

   

        if (MoveInteractionOnLoad)
        {
            if (CurrentState == "I") { CurrentState = "C"; CurrentTextLine = 0; }
            if (CurrentState == "C") { CurrentState = "E"; CurrentTextLine = 0; }
            if (CurrentState == "H") { CurrentState = "E"; CurrentTextLine = 0; }
        }
        SetDeath();
        SetLocation();
        if (gameObject.activeInHierarchy) { SetAnimation(); }
    }


    public void NextInteraction()
    {
        switch (CurrentState) 
        {
            case "YES":
                CurrentState = "B"; CurrentTextLine = 0;
                break;
            case "B":
                CurrentState = "G"; CurrentTextLine = 0;
                break;
            case "G":
                CurrentState = "H"; CurrentTextLine = 0;
                MoveInteractionOnLoad = false;
                break;
            default:
                break;
        }
    }
    public void SetDeath()
    {
        switch (CurrentState)
        {
            case "C":
                CurrentState = "E";
                break;
            case "F":
                CurrentState = "E";
                break;
            case "H":
                CurrentState = "E";
                break;
            default:
                break;
        }
        if (CurrentState == "E") { IsOscarDead = true; }
        if (CurrentState == "D") { IsOscarDead = true; }
    }
    public void SetLocation() 
    {
        switch (CurrentState) 
        {      
            case "A":
                transform.parent = FrontLayerObject.transform;
                Assets.sortingLayerName = "FrontSortingLayer";
                transform.position = Locations[0];              
                break;
            case "G":
                transform.parent = FrontLayerObject.transform;
                Assets.sortingLayerName = "FrontSortingLayer";
                transform.position = Locations[1];
                break;
            case "B":
                transform.parent = FrontLayerObject.transform;
                Assets.sortingLayerName = "FrontSortingLayer";
                transform.position = Locations[2];
                break;

            case "I":
                transform.parent = MiddleLayerObject.transform;
                Assets.sortingLayerName = "MiddleSortingLayer";
                transform.position = Locations[3];
                break;

            case "H":
                transform.parent = MiddleLayerObject.transform;
                Assets.sortingLayerName = "MiddleSortingLayer";
                transform.position = Locations[4];
                break;
            case "E":
                transform.parent = MiddleLayerObject.transform;
                Assets.sortingLayerName = "MiddleSortingLayer";
                transform.position = Locations[4];
                break;
            case "C":
                transform.parent = MiddleLayerObject.transform;
                Assets.sortingLayerName = "MiddleSortingLayer";
                transform.position = Locations[4];
                break;
        }
    }
    public void SetAnimation()
    {
        switch (CurrentState)
        {
            case "Null":
                Anim.Play("OscarAnim_SittingIdle"); //Sitting
                break;
            case "A":
                Anim.Play("OscarAnim_SittingIdle"); //Sitting
                break;
            case "YES":
                Anim.Play("OscarAnim_SittingIdle"); //Sitting
                break;
            case "NO":
                Anim.Play("OscarAnim_SittingIdle"); //Sitting
                break;
            case "B":
                Anim.Play("OscarAnim_StandingIdle"); //standing
                break;
            case "C":
                Anim.Play("OscarAnim_SittingIdle"); //dying
                break;
            case "D":
                Anim.Play("OscarAnim_SittingIdle"); //dying
                break;
            case "E":
                Anim.Play("OscarAnim_SittingIdle"); //dying
                break;
            case "F":
                Anim.Play("OscarAnim_SittingIdle"); //dying
                break;
            case "G":
                Anim.Play("OscarAnim_StandingIdle"); //standing
                break;
            case "H":
                Anim.Play("OscarAnim_SittingIdle"); //dying broken legs
                break;
            case "I":
                Anim.Play("OscarAnim_SittingIdle"); //dying
                break;


        }
    }
    public void SetLineText()
    {
        CurrentText = "";

        StateATextLines.Add("...Oh, you... You're no Hollow, eh? Thank goodness...");
        StateATextLines.Add("...That boulder was you?...HA...I thank you for returning the favour...");
        StateATextLines.Add("...Was afraid this was it for me... I wish to ask something of you...");
        StateATextLines.Add("...You and I, we're both Undead... Hear me out, will you?...");

        StateYESTextLines.Add("...I had set out on a mission... But perhaps you can assist me…");
        StateYESTextLines.Add("…There is an old saying in my family... Thou who art Undead… ");
        StateYESTextLines.Add("…art chosen... In thine exodus from the Undead Asylum…");
        StateYESTextLines.Add("…maketh pilgrimage to the land of Ancient Lords... When thou ringeth the Bell… ");
        StateYESTextLines.Add("…of Awakening, the fate of the Undead thou shalt know... ");
        StateYESTextLines.Add("…Well, now you know… aha quite the tale, no?...");
        StateYESTextLines.Add("…Once we make our way out of this Asylum we shall see if it holds any merit... ");
        StateYESTextLines.Add("…Oh, one more thing... Here, take this... An Estus Flask, an Undead favourite…");
        StateYESTextLines.Add("…Oh, and this too…");
        StateYESTextLines.Add("...That’s about it… Best of luck friend…");

        StateNOTextLines.Add("… Yes, I see… Perhaps I was too hopeful… Hah hah…");
        StateNOTextLines.Add("…May our paths cross once more… Farewell…");

        StateBTextLines.Add("...Perhaps you have more luck courting than slaying…");
        StateBTextLines.Add("…Ha ha I jest.. Prithee forgive my brash comment…");
        StateBTextLines.Add("...hmm… there must be another way…");

        StateCTextLines.Add("...Uhh, ahh, a knight… Unlike any I’ve seen before…");
        StateCTextLines.Add("…take these… m-my estus flasks… you’ll need them…");

        StateDFTextLines.Add("...Hrggkt... But... Why…");

        StateGTextLines.Add("...Ah… still determined I see… you’re built of sturdier stuff than I …hmm… ");
        StateGTextLines.Add("…Best to spread our efforts… I’ll search the asylum for other exits while you fight…");
        StateGTextLines.Add("…I wish you luck in your… battles…");
        StateGTextLines.Add("...Hmm… perhaps the balcony…");

        StateHTextLines.Add("...Ah… shame you had to find me in such a state… my legs…");
        StateHTextLines.Add("…such a fool…I-I wish to ask one last thing of you… ");
        StateHTextLines.Add("…tell them… tell them I- … s-stay here w-with me for but a moment, please…");
        StateHTextLines.Add("…M-may the flames.. guide thee...");

        StateITextLines.Add("...Ah, there you are… I see you’ve felled that wretched beast... ");
        StateITextLines.Add("…Gave me quite a bit of trouble on the rooftops…");
        StateITextLines.Add("…You go on ahead… I shall catch up with you in due time…");
    }

    public void Y()
    {

            Debug.Log("Y Button Pressed");
            CanInput = false;

        if(CurrentState == "Null") { CurrentState = "A"; }

            if (!IsTalking) { OpenDialog(); }
            NextLine();
        InteractPrompt.SetActive(false);
        // StopAnyAudio();//stop audio and timer     

        if (IsOscarDead)
        {
            if (PC.MaxEstus == 3) { GiveEstus(3); }
            else if (PC.MaxEstus == 0) { GiveEstus(6); }
        }

    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true && InRange == true)
        {
            Debug.Log("A Button Pressed");
            CanInput = false;
            ChooseQuestion();
        }
    }
    public void B(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true && InRange == true)
        {
            Debug.Log("B Button Pressed");
            CanInput = false;
            CurrentState = "NO";
            CloseQuestion();
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true && InRange == true)
        {
            CanInput = false;
            Debug.Log("Left Button Pressed");
            QuestionOrder = !QuestionOrder;
            MoveQuestionHighlight();
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true && InRange == true)
        {
            CanInput = false;
            Debug.Log("Right Button Pressed");
            QuestionOrder = !QuestionOrder;
            MoveQuestionHighlight();
        }
    }





    public void NextLine()
    {
        PC.PlayerFinishInteraction();
        if (IsTalking) { StopAnyAudio(); } //stop previous audio to play/skip to next
        IsTalking = true; //currently talking
        switch (CurrentState) //set dialog order a
        {
            case "A":
                if (CurrentTextLine < 3)
                {
                    PlayAudio(StateAAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateATextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                }
                else
                {
                    OpenDialog();
                    PlayAudio(StateAAudioClips[CurrentTextLine]);
                    CurrentTextLine = 3; // repeat the 4th line of dialog text
                    CurrentText = StateATextLines[3]; //tell dialogue text what to show
                    IsTalking = false; //done talking
                    OpenQuestion();
                } //repeats question

                break;
            case "YES":
                if (CurrentTextLine < 10)
                {
                    PlayAudio(StateYESAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateYESTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                }
                else
                {
                    
                    CurrentTextLine = 9; // repeat the nth line of dialog text
                    CurrentText = StateYESTextLines[9]; //tell dialogue text what to show
                    PlayAudio(StateYESAudioClips[CurrentTextLine]);
                    IsTalking = false; //done talking
                    CloseDialog();
                } //repeats
                if (CurrentTextLine == 8) { GiveEstus(3); }
                if (CurrentTextLine == 9 && IsTalking) { GiveKey(); MoveInteractionOnLoad = true; }
                break;
            case "NO":
                if (CurrentTextLine < 2)
                {
                    PlayAudio(StateNOAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateNOTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                } else { CurrentState = "A"; CurrentTextLine = 3; RoutineToStop = StartCoroutine(WaitForAudioToEnd()); CloseDialog();  } //reset to A question
                break;
            case "B":
                if (CurrentTextLine < 3)
                {
                    PlayAudio(StateBAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateBTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                } else
                {                  
                    CurrentTextLine = 2; // repeat the nth line of dialog text
                    CurrentText = StateBTextLines[2]; //tell dialogue text what to show
                    PlayAudio(StateBAudioClips[CurrentTextLine]);
                    IsTalking = false; //done talking
                    CloseDialog();
                    MoveInteractionOnLoad = true;
                } //repeats
                    break;
            case "C":
                if (CurrentTextLine < 2)
                {
                    PlayAudio(StateCAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateCTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                    MoveInteractionOnLoad = true;
                } else { CloseDialog(); IsOscarDead = true; } //dies
                if (CurrentTextLine == 2) { GiveEstus(3); }
                break;
            case "DF":
                if (CurrentTextLine < 1)
                {
                    PlayAudio(StateDFAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateDFTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                } else { CloseDialog(); OscarKilled(); } //dies
                break;
            case "G":
                if (CurrentTextLine < 4)
                {
                    PlayAudio(StateGAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateGTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                } else
                {
                    MoveInteractionOnLoad = true;
                    CurrentTextLine = 3; // repeat the nth line of dialog text
                    CurrentText = StateGTextLines[3]; //tell dialogue text what to show
                    PlayAudio(StateGAudioClips[CurrentTextLine]);
                    IsTalking = false; //done talking
                    CloseDialog();  } //repeats
                break;
            case "H":
                if (CurrentTextLine < 4)
                {
                    PlayAudio(StateHAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateHTextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                    MoveInteractionOnLoad = true;
                } else { CloseDialog(); IsOscarDead = true; } //dies
                break;
            case "I":
                if (CurrentTextLine < 3)
                {
                    PlayAudio(StateIAudioClips[CurrentTextLine]);
                    RoutineToStop = StartCoroutine(WaitForAudioToEnd());//start audio timer            
                    CurrentText = StateITextLines[CurrentTextLine]; //tell dialogue text what to show
                    CurrentTextLine++;
                    MoveInteractionOnLoad = true;
                } else
                {
                    PlayAudio(StateGAudioClips[CurrentTextLine]);
                    CurrentTextLine = 2; // repeat the nth line of dialog text
                    CurrentText = StateGTextLines[2]; //tell dialogue text what to show
                    IsTalking = false; //done talking
                    CloseDialog();
                }
                break;
        }



        DialogTextObject.text = CurrentText;
        CanInput = true;

    }



    public void CloseDialog()
    {
        StopAnyAudio(); 
        DialogTextObject.text = "";
        DialogObject.SetActive(false);
        CanInput = true;
    }
    public void OpenDialog()
    {
        DialogObject.SetActive(true);
        DialogTextObject.text = CurrentText; 
    }
    public void OpenQuestion()
    {
        PC.State = "Interacting";
        PC.Anim.Play("PlayerAnim_Idle");
        QuestionObject.SetActive(true);
        QuestionOpen = true;
        QuestionOrder = true;
        //Player cannot roll, sprint or jump
        CanInput = true;
        MoveQuestionHighlight();
    }
    public void CloseQuestion()
    {
        PC.PlayerFinishInteraction();
        QuestionObject.SetActive(false);
        QuestionOpen = false;
        //Player can roll, sprint or jump
        CanInput = true;
    }
    public void MoveQuestionHighlight()
    {

        if (QuestionOrder)
        {
            QuestionHightlightPos.anchoredPosition = new Vector2(-80, 0);
        }
        else
        {
            QuestionHightlightPos.anchoredPosition = new Vector2(80, 0);
        }

        CanInput = true;
    }
    public void ChooseQuestion()
    {
        PC.PlayerFinishInteraction();
        CurrentTextLine = 0;
        StopAnyAudio();
        if (QuestionOrder)
        {
            CurrentState = "YES";
            CloseQuestion();
            CurrentText = StateYESTextLines[CurrentTextLine];
            DialogTextObject.text = CurrentText;
            CurrentTextLine = 0;
        }
        else
        {
            CurrentState = "NO";
            CloseQuestion();
            IsTalking = false;
        }
        NextLine();
    }


    public void PlayAudio(EventReference AudioReferenceToPlay)
    {


        Emitter.Stop();
        Emitter.EventReference = AudioReferenceToPlay;        // Emmiter system needs spacializer on the FMOD audio clips
        Emitter.Play();

     

        var AudioPos = FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position);

        instance = FMODUnity.RuntimeManager.CreateInstance(AudioReferenceToPlay); //choose audio  
        instance.set3DAttributes(AudioPos);
                                                                                       // instance.start();
        instance.release(); //release audio from memory       
        Debug.Log("Audio Played");
    }
    IEnumerator WaitForAudioToEnd()
    {

        instance.getDescription(out FMOD.Studio.EventDescription Des);
        Des.getLength(out int lengthMili);
        float lenght = (lengthMili / 1000) +1f;
        Debug.Log(lenght);


        Debug.Log("Waiting " + lenght);
        yield return new WaitForSeconds(lenght); //wait till end of audio
        Debug.Log("Done Waiting");
        if (InRange) { NextLine(); }//play next line
        else { CloseDialog(); IsTalking = false; }
    }
    public void StopAnyAudio()
    {
        Emitter.Stop();

        //  instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); //stop current audio
        if (RoutineToStop != null) {StopCoroutine(RoutineToStop); } //stop current timer
        
        Debug.Log("Audio Stopped");
    }


    public void GiveEstus(int num)
    {
      PC.GiveEstus(num);
      ItemSymbol.texture = EstusSymbol;
      ItemName.text = EstusName;
      ItemQuantity.text = num.ToString();
      StartCoroutine(UIPopUp());
    }
    public void GiveKey()
    {
        Debug.Log("Given Key");
        PC.GetComponent<PlayerManager>().KKey = true;
        ItemSymbol.texture = KeySymbol;
        ItemName.text = KeyName;
        ItemQuantity.text = "1";
        StartCoroutine(UIPopUp());
    }

    IEnumerator UIPopUp()
    {

        ItemPopUp.SetActive(true);
        yield return new WaitForSeconds(3f);
        ItemPopUp.SetActive(false);

    }



    public void OscarKilled()
    {
        IsOscarDead = true;
        MoveInteractionOnLoad = false;
        if (PC.MaxEstus == 3) { GiveEstus(3); }
        else if (PC.MaxEstus == 0) { GiveEstus(6); }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Also check if the player is on the same layer as this
        {
            InRange = true;
            DialogTextObject.text = CurrentText;
            OpenDialog();
            PC.Interactable = Interactable;
            if (!IsOscarDead) //interaction animation 
            {
                InteractPrompt.SetActive(true);
                if (IsSitting()) { Anim.Play("OscarAnim_SittingInteract"); }
                else { Anim.Play("OscarAnim_StandingInteract"); }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRange = false;
            DialogObject.SetActive(false);
            CloseQuestion();
            CloseDialog();
            PC.Interactable = null;
            if (!IsOscarDead) //interaction animation 
            {
                InteractPrompt.SetActive(false);
                if (IsSitting()) { Anim.Play("OscarAnim_SittingIdle"); }
                else { Anim.Play("OscarAnim_StandingIdle"); }
            }
        }
    }

    bool IsSitting()
    {
        switch (CurrentState)
        {
            case "Null":
                return true;

            case "A":
                return true;

            case "YES":
                return true;
 
            case "NO":
                return true;

            case "C":
                return true;

            case "H":
                return true;

            default:
                return false;

        }
    }




    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        CurrentCharacterData.OscarState = CurrentState;
        CurrentCharacterData.MoveInteractionOnLoad = MoveInteractionOnLoad;
        CurrentCharacterData.IsOscarDead = IsOscarDead;
        CurrentCharacterData.CurrentTextLine = CurrentTextLine;
    }
    public void LoadGameFromDataToCurrentCharacterData(ref CharacterSaveData CurrentCharacterData)
    {
        CurrentState = CurrentCharacterData.OscarState;
        MoveInteractionOnLoad = CurrentCharacterData.MoveInteractionOnLoad;
        IsOscarDead = CurrentCharacterData.IsOscarDead;
        CurrentTextLine = CurrentCharacterData.CurrentTextLine;
    }
}