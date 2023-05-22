using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class OscarManager : MonoBehaviour
{
    public bool CanInput = true;


    [Header("State")]
    public string CurrentState;

    [Header("Dialog")]
    public GameObject DialogObject;
    public TextMeshProUGUI DialogTextObject;
    
    public string CurrentText;
    public int CurrentTextLine=0;
    public bool IsTalking = false;

    [Header("Question")]
    public GameObject QuestionObject;
    public RectTransform QuestionHightlightPos;
    public bool QuestionOrder;
    public bool QuestionOpen;



    [Header("Script")]
    public List<string> StateATextLines = new List<string>();
    public List<string> StateYESTextLines = new List<string>();
    public List<string> StateNOTextLines = new List<string>();

    [Header("Audio clips")]
    public List<AudioClip> StateAAudioClips = new List<AudioClip>();


    public void Start()
    {
        QuestionOpen = false;
        QuestionObject.SetActive(false);

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
    }

    public void Y(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == false)
        {
            Debug.Log("Y Button Pressed");
            CanInput = false;

            if (!IsTalking) { OpenDialog(); }

            NextLine();           
        }
    }
    public void A(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true)
        {
            Debug.Log("A Button Pressed");
            CanInput = false;
            ChooseQuestion();
        }
    }
    public void B(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true)
        {
            Debug.Log("B Button Pressed");
            CanInput = false;
            CurrentState = "NO";
            CloseQuestion();
        }
    }
    public void Left(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true)
        {
            CanInput = false;
            Debug.Log("Left Button Pressed");
            QuestionOrder = !QuestionOrder;
            MoveQuestionHighlight();
        }
    }
    public void Right(InputAction.CallbackContext context)
    {
        if (context.action.triggered && CanInput == true && QuestionOpen == true)
        {
            CanInput = false;
            Debug.Log("Right Button Pressed");
            QuestionOrder = !QuestionOrder;
            MoveQuestionHighlight();
        }
    }


    public void NextLine()
    {
        
        switch (CurrentState)
        {           
            case "A":
                if (CurrentTextLine<4) { CurrentText = StateATextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentText = StateATextLines[3]; OpenQuestion(); } //show new text
                //play audio of current line 
                //start timer based on length of audio clip of current line to change text
                break;
            case "YES":
                if (CurrentTextLine < 9) { CurrentText = StateYESTextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentText = StateYESTextLines[9]; }
                break;
            case "NO":
                if (CurrentTextLine < 2) { CurrentText = StateNOTextLines[CurrentTextLine]; CurrentTextLine++; } else { CurrentState = "A"; CurrentTextLine = 4; CurrentText = StateATextLines[3];  }
                break;
        }


       
       DialogTextObject.text = CurrentText;
       CanInput = true;
       Debug.Log("Dialog updated");
    }

    public void CloseDialog()
    {
        DialogObject.SetActive(false);
        IsTalking = false;
    }
    public void OpenDialog()
    {
        DialogObject.SetActive(true);
        IsTalking = true;
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

    IEnumerable Talking()
    {

        yield return new WaitForSeconds(1.2f);
    }

}
