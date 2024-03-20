using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class SpeedRunManager : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public PlayerManager playerManager;
    public float roundTime;
    public float demonTime;
    public float pursuerTime;
    public float DemonKillTime;
    public float PursuerKillTime;

    public TextMeshProUGUI DemonKillText;
    public TextMeshProUGUI PursuerKillText;

    public bool inDemon;
    public bool inPursuer;


    // Update is called once per frame
    void Update()
    {

        float timer = playerManager.TimePlayedSeconds;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        timerText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");


        if (inDemon)
        {
            demonTime += Time.deltaTime;


            float dtimer = demonTime;
            float dmili = TimeSpan.FromSeconds(dtimer).Milliseconds;
            float dseconds = TimeSpan.FromSeconds(dtimer).Seconds;
            float dminutes = TimeSpan.FromSeconds(dtimer).Minutes;
            float dhours = TimeSpan.FromSeconds(dtimer).Hours;



            DemonKillText.text = dhours.ToString("00") + ":" + dminutes.ToString("00") + ":" + dseconds.ToString("00") + ":" + dmili.ToString("00");
        }

        if (inPursuer)
        {
            pursuerTime += Time.deltaTime;


            float ptimer = pursuerTime;
            float pmili = TimeSpan.FromSeconds(ptimer).Milliseconds;
            float pseconds = TimeSpan.FromSeconds(ptimer).Seconds;
            float pminutes = TimeSpan.FromSeconds(ptimer).Minutes;
            float phours = TimeSpan.FromSeconds(ptimer).Hours;



            PursuerKillText.text = phours.ToString("00") + ":" + pminutes.ToString("00") + ":" + pseconds.ToString("00") + ":" + pmili.ToString("00");
        }

    }
    public void DemonReset()
    {
        demonTime = 0;
        inDemon = false;
        DemonKillText.text = "";
    }
    public void PersuerReset()
    {
        pursuerTime = 0;
        inPursuer = false;
        PursuerKillText.text = "";
    }
    public void DemonKilled()
    {
        inDemon = false;
        float timer = demonTime;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        DemonKillText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");
    }
    public void PursuerKilled()
    {
        inPursuer = false;
        float timer = playerManager.TimePlayedSeconds;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        PursuerKillText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");
    }
}
