using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameController : MonoBehaviour {
    public static GameController instance;
    public Text starterClockText;

    float startingClock = 4f;
    [HideInInspector] private bool starterClockCompleted = false;
    [HideInInspector] private bool playersReady = false;
    [HideInInspector] public bool raceStarted = false;


    private int requirePlayers = 2;

    List<GameObject> players = new List<GameObject>();

    AudioSource audioSource;
    public AudioClip starterSound;
    bool starterSoundPlaying = false;



    private void Awake()
    {
        starterClockText.text = "Awaiting Players";
        instance = this;
        audioSource = GetComponent<AudioSource>();

    }

    void Start () {

	}

    public void RegisterPlayer(GameObject playerObject)
    {
        players.Add(playerObject);
    }
	
	void Update () {
        if (players.Count == requirePlayers)
        {
            playersReady = true;
        }
    
        if (playersReady && !starterClockCompleted)
        {
            audioSource.PlayOneShot(starterSound);
            startingClock -= Time.deltaTime;
            starterClockText.text = "" + (int)startingClock;
            if (startingClock <= 0)
            {
                starterClockCompleted = true;
                starterClockText.text = "GO!";
                raceStarted = true;
            }
        }

	}
}
