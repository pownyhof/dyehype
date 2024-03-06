using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{

    public GameObject WinPopup;
    public Text timeText;
    public AudioSource sound;

    public List<GameObject> live_images;

    private int livesRemaining;


    void Start()
    {
        WinPopup.SetActive(false);   
    }

    private void OnGameCompleted()
    {
        // if player didnt mute audio play winSound
        int audioMuted = PlayerPrefs.GetInt("audioMuted");
        if (audioMuted == 0)
        {
            sound.Play();
        }

        // player unlocked new level
        int currentLevel = PlayerPrefs.GetInt("selectedLevel");
        int levelReached = PlayerPrefs.GetInt("levelReached");
        if (currentLevel == levelReached)
        {
            levelReached += 1;
            PlayerPrefs.SetInt("levelReached", levelReached);
        }

        // get time player needed from Timer.cs
        timeText.text = Timer.instance.GetTimeText().text;

        // get lives left from Lives.cs
        livesRemaining = Lives.GetRemainingLives();

        // show how many lives player had left
        for(int i = 0; i < livesRemaining; i++)
        {
            live_images[i].SetActive(true);
        }
        // show GameObject winPopUp
        WinPopup.SetActive(true);
    }

    private void OnEnable()
    {
        // subscribe to event
        GameEvents.OnGameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        // unsubscribe to event
        GameEvents.OnGameCompleted -= OnGameCompleted;
    }
}
