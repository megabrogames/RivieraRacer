using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CarUserControl : NetworkBehaviour
{
    private CarController m_Car; // the car controller we want to use
    [HideInInspector] public GameObject carCam;
    public GameObject carCamPrefab;
    public GameObject mainBody;

    CarController carController;
    Vector3 offset = new Vector3(0f, 2f, -5f); // for camera , had to start fucking about and putting things like this all over place because I am still using the script from the StandardAssets namespace - only way i can think to fix it is, copy-pasting everything into my own scripts.

    private Vector2 startingPoint;

    bool allowInput = true;

    private float raceTime = 0.00f;
    private bool raceEnded = false;
    bool raceTimerLive = false;

    private FrontTrigger frontTrigger;

    private GameObject timePanel;

    public override void OnStartClient()
    {
        // the gamecontroller keeps track of how many player cars have joined game
        GameController.instance.RegisterPlayer(this.gameObject);
    }
    public override void OnStartLocalPlayer()
    {
        m_Car = GetComponent<CarController>();
        carCam = GameObject.Instantiate(carCamPrefab);
        carCam.transform.SetParent(this.transform);
        carCam.transform.position = transform.position + offset;
        

        mainBody.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.05f, 0.4f), Random.Range(0.05f, 0.4f), UnityEngine.Random.Range(0.05f, 0.4f));
        tag = "LocalPlayerCar";
        carController = m_Car;

        startingPoint = transform.position;

        frontTrigger = GetComponentInChildren<FrontTrigger>();
        Debug.Log("front trigger = " + frontTrigger);

        timePanel = GameObject.FindGameObjectWithTag("TimePanel");
        timePanel.SetActive(false);
        
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
      
        
        // update will only be called if raceStarted is true
        if (GameController.instance.raceStarted)
        {
            if (raceTimerLive && !raceEnded)
            {
                raceTime += Time.deltaTime;
            }
            else 
            {
                raceTimerLive = true;
            }
           

            if (allowInput)
                HandleInput();

            CheckForFinish();
            // for now, just respawn the player if car drives off map:
            if (this.gameObject.transform.position.y < -5)
            {
                Respawn();
            }
        }
    }

    void CheckForFinish()
    {
        if (frontTrigger.isFinished)
            EndRacing();
    }


    void HandleInput()
    {
        // pass the input to the car!
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
        float handbrake = CrossPlatformInputManager.GetAxis("Jump");
        m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif

        carCam.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z);
    }

    void Respawn()
    {
        if (isServer)
        {
            RpcRespawn();
        }
        else
            CmdRespawn();
    }

    [Command]
    void CmdRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the player’s position to the chosen spawn point
            transform.position = startingPoint;
            ResetCar();
        }
    }

    void EndRacing()
    {
        if (isServer)
        {
            RpcEndRacing();
        }
        else
        {
            CmdEndRacing();
        }
    }

    [Command]
    void CmdEndRacing()
    {
        RpcEndRacing();
    }

    [ClientRpc]
    void RpcEndRacing()
    {
        if (isLocalPlayer)
        {
            raceEnded = true;
            raceTimerLive = false;
            DisallowInput();
            timePanel.SetActive(true);
            timePanel.GetComponentInChildren<Text>().text = "Time: " + raceTime;
        }
    }


    void DisallowInput()
    {
        allowInput = false;
        carController.m_Rigidbody.velocity = Vector3.zero;
    }
    void ResetCar()
    {
        carController.m_Rigidbody.transform.eulerAngles = Vector3.zero;
        carController.m_Rigidbody.velocity = Vector3.zero;
    }
}
