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
    public bool CanInput = true;
    public bool InRange = false;


    [Header("State")]
    public string CurrentState;
    public Animator OscarAnim;
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


    public void Start()
    {
        QuestionOpen = false;
        QuestionObject.SetActive(false);

        DialogObject.SetActive(false);
        IsTalking = false;

        CurrentTextLine = 0;

        SetLocation();
      //  SetAnimation();
        SetLineText();
       
    }

    public void SetLocation() 
    {
        switch (CurrentState) 
        {      
            case "A":
                transform.position = Locations[0];
                break;
            case "G":
                transform.position = Locations[1];
                break;
            case "B":
                transform.position = Locations[2];
                break;
            default:
                transform.position = Locations[3];
                break;
        }
    }

    public void SetAnimation()
    {
        switch (CurrentState)
        {
            case "A":
                OscarAnim.Play(""); //Sitting
                break;
            case "B":
                OscarAnim.Play(""); //standing
                break;
            case "C":
                OscarAnim.Play(""); //dying
                break;
            case "G":
                OscarAnim.Play(""); //standing
                break;
            case "H":
                OscarAnim.Play(""); //dying broken legs
                break;
             default:
                OscarAnim.Play(""); //dead
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

    public void Y(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == false && InRange == true)
        {
            Debug.Log("Y Button Pressed");
            CanInput = false;

            if (!IsTalking) { OpenDialog(); }
            NextLine();
            // StopAnyAudio();//stop audio and timer     
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
                    PlayAudio(StateAAudioClips[CurrentTextLine]);
                    CurrentTextLine = 3; // repeat the 4th line of dialog text
                    CurrentText = StateATextLines[3]; //tell dialogue text what to show
                    IsTalking = false; //done talking
                    OpenQuestion();
                }

                break;
            case "YES":
                if (CurrentTextLine < 10) { CurrentText = StateYESTextLines[CurrentTextLine]; CurrentTextLine++;  }
                else { CurrentText = StateYESTextLines[9]; CurrentTextLine = 9; CloseDialog(); IsTalking = false; }
                if (CurrentTextLine == 8) { GiveEstus(3); }
                if (CurrentTextLine == 9 && IsTalking) { GiveKey(); }
                break;
            case "NO":
                if (CurrentTextLine < 2) { CurrentText = StateNOTextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentState = "A"; CurrentTextLine = 3; CloseDialog();  }
                break;
            case "B":
                if (CurrentTextLine < 3) { CurrentText = StateBTextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentTextLine = 2; CloseDialog(); }
                break;
            case "C":
                if (CurrentTextLine < 2) { CurrentText = StateCTextLines[CurrentTextLine]; CurrentTextLine++; } else { CloseDialog(); }
                break;
            case "DF":
                if (CurrentTextLine < 1) { CurrentText = StateDFTextLines[CurrentTextLine]; CurrentTextLine++; } else { CloseDialog(); }
                break;
            case "G":
                if (CurrentTextLine < 4) { CurrentText = StateGTextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentTextLine = 3; CloseDialog(); }
                break;
            case "H":
                if (CurrentTextLine < 4) { CurrentText = StateHTextLines[CurrentTextLine]; CurrentTextLine++; } else { CloseDialog(); }
                break;
            case "I":
                if (CurrentTextLine < 3) { CurrentText = StateITextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentTextLine = 2; CloseDialog(); }
                break;
        }



        DialogTextObject.text = CurrentText;
        CanInput = true;

    }

    public void CloseDialog()
    {
        StopAnyAudio();
        DialogObject.SetActive(false);
        CanInput = true;
        DialogTextObject.text = "";
    }
    public void OpenDialog()
    {
        DialogObject.SetActive(true);
        DialogTextObject.text = CurrentText; 
    }
    public void OpenQuestion()
    {
        QuestionObject.SetActive(true);
        QuestionOpen = true;
        QuestionOrder = true;
        //Player cannot roll, sprint or jump
        CanInput = true;
        MoveQuestionHighlight();
    }
    public void CloseQuestion()
    {
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
        CurrentTextLine = 0;
        StopAnyAudio();
        if (QuestionOrder)
        {
            CurrentState = "YES";
            CloseQuestion();
            CurrentText = StateYESTextLines[CurrentTextLine];
            DialogTextObject.text = CurrentText;
            CurrentTextLine = 1;
        }
        else
        {
            CurrentState = "NO";
            CloseQuestion();
            IsTalking = false;
            DialogObject.SetActive(false);
        }
        CanInput = true;
    }


    public void PlayAudio(EventReference AudioReferenceToPlay)
    {


        Emitter.Stop();
        Emitter.EventReference = AudioReferenceToPlay;        // Emmiter system needs spacializer on the FMOD audio clips
        Emitter.Play();

        var AudioPos = FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position);

        instance = FMODUnity.RuntimeManager.CreateInstance(AudioReferenceToPlay.Path); //choose audio  
        instance.set3DAttributes(AudioPos);
                                                                                       // instance.start();
        instance.release(); //release audio from memory       
        Debug.Log("Audio Played");
    }
    IEnumerator WaitForAudioToEnd()
    {

        instance.getDescription(out FMOD.Studio.EventDescription Des);
        Des.getLength(out int lengthMili);
        int lenght = lengthMili / 1000;
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
        StopCoroutine(RoutineToStop); //stop current timer
        Debug.Log("Audio Stopped");
    }


    public void GiveEstus(int num)
    {
        Debug.Log("Given Estus x" + num);
    }
    public void GiveKey()
    {
        Debug.Log("Given Key");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Also check if the player is on the same layer as this
        {
            InRange = true;
            DialogTextObject.text = CurrentText;
            OpenDialog();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRange = false;
            DialogObject.SetActive(false);
            CloseQuestion();
        }
    }
}