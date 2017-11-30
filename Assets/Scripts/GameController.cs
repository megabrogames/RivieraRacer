using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameController : MonoBehaviour {
    public static GameController instance;
    public Text starterClockText;

    float startingClock = 4f;
    private bool starterClockCompleted = false;
    private bool playersReady = false;
    [HideInInspector] public bool raceStarted = false;


    private int requirePlayers = 2;

    List<GameObject> players = new List<GameObject>();

    AudioSource audioSource;
    public AudioClip starterSound;
    public AudioClip bgMusic;
    bool starterSoundPlaying = false;

    GameObject menuCam;





    private void Awake()
    {

        instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgMusic;
        audioSource.loop = true;
        audioSource.Play();
        menuCam = GameObject.FindGameObjectWithTag("MenuCamera");
    }

    void Start () {

	}

    public void RegisterPlayer(GameObject playerObject)
    {
        starterClockText.text = "Awaiting Players";
        menuCam.SetActive(false);
        players.Add(playerObject);
    }
	
	void Update () {
        if (players.Count == requirePlayers)
        {
            playersReady = true;
        }
    
        if (playersReady && !starterClockCompleted)
        {
            if (!starterSoundPlaying)
            {
                audioSource.PlayOneShot(starterSound);
                starterSoundPlaying = true;
            }

            startingClock -= Time.deltaTime;
            starterClockText.text = "" + (int)startingClock;

            // start race:
            if (startingClock <= 0)
            {
                starterClockCompleted = true;
                starterClockText.text = "GO!";
                raceStarted = true;
            }
        }

	}
}
