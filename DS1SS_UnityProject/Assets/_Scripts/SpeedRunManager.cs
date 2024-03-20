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
    public float DemonKillTime;
    public float PursuerKillTime;

    public TextMeshProUGUI DemonKillText;
    public TextMeshProUGUI PursuerKillText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float timer = playerManager.TimePlayedSeconds;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        timerText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");

    }

    public void DemonKilled()
    {
        float timer = playerManager.TimePlayedSeconds;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        DemonKillText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");
    }
    public void PursuerKilled()
    {
        float timer = playerManager.TimePlayedSeconds;
        float mili = TimeSpan.FromSeconds(timer).Milliseconds;
        float seconds = TimeSpan.FromSeconds(timer).Seconds;
        float minutes = TimeSpan.FromSeconds(timer).Minutes;
        float hours = TimeSpan.FromSeconds(timer).Hours;



        PursuerKillText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + mili.ToString("00");
    }
}
