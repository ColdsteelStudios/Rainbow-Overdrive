// ---------------------------------------------------------------------------
// LocalPlaySetup.cs
// 
// Like the Match Manager class, but for Local Play
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocalPlaySetup : MonoBehaviour 
{
    public Text m_displayMessage;
    public GameObject m_SplitScreenCamera;

    private GameObject m_playerOneCam;
    private GameObject m_playerTwoCam;
    private GameObject m_playerThreeCam;
    private GameObject m_playerFourCam;

    private bool m_usingLocalPlay = false;
    private bool m_playerCountSelected = false;
    private int m_playerCount = 0;

    void Update()
    {
        if (m_usingLocalPlay)
        {
            if (!m_playerCountSelected)
                SelectPlayerCount();
        }
    }

    //Called from Connection Setup when player indicates that
    //they want to use local play for this session
    //Allows player to choose amount of players for this game
    private void StartLocalPlayPhaseOne()
    {
        m_usingLocalPlay = true;
        m_displayMessage.text = "F1 - 2 Players" + "\n" + "F2 - 3 Players" + "\n" + "F3 - 4 Players";
    }

    //Sets up scene cameras
    private void StartLocalPlayPhaseTwo()
    {
        //Disable the current scene camera
        GameObject.Find("Camera").SetActive(false);
        SetUpCameras();
        GameObject.Find("MatchManager").SendMessage("SetPlayerCount", m_playerCount);
        GameObject.Find("MatchManager").SendMessage("StartGamePhaseOne");
    }

    private void SelectPlayerCount()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            m_displayMessage.text = "";
            m_playerCount = 2;
            m_playerCountSelected = true;
            StartLocalPlayPhaseTwo();
            return;
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
            m_displayMessage.text = "";
            m_playerCount = 3;
            m_playerCountSelected = true;
            StartLocalPlayPhaseTwo();
            return;
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            m_displayMessage.text = "";
            m_playerCount = 4;
            m_playerCountSelected = true;
            StartLocalPlayPhaseTwo();
            return;
        }
    }

    private void SetUpCameras()
    {
        float W = Screen.width;
        float H = Screen.height;

        switch (m_playerCount)
        {
            //Create two camera which each get half of the screen, split horizontally
            case (2):
                {
                    m_playerOneCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P1 = m_playerOneCam.GetComponent<Camera>();
                    Rect P1R = new Rect(0, 0, W, H);
                    P1R.yMin = 0.5f;
                    P1.rect = P1R;
                    P1.transform.tag = "P1CAM";

                    m_playerTwoCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P2 = m_playerTwoCam.GetComponent<Camera>();
                    Rect P2R = new Rect(0, 0, W, H);
                    P2R.yMax = 0.5f;
                    P2.rect = P2R;
                    P2.transform.tag = "P2CAM";
                    m_playerTwoCam.GetComponent<AudioListener>().enabled = false;
                    return;
                }
            //Create three camera's, first player gets top half, 2 and 3 share the bottom half of the screen
            case (3):
                {
                    m_playerOneCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P1 = m_playerOneCam.GetComponent<Camera>();
                    Rect P1R = new Rect(0, 0, W, H);
                    P1R.yMin = 0.5f;
                    P1.rect = P1R;
                    P1.transform.tag = "P1CAM";

                    m_playerTwoCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P2 = m_playerTwoCam.GetComponent<Camera>();
                    Rect P2R = new Rect(0, 0, W, H);
                    P2R.yMax = 0.5f;
                    P2R.xMax = 0.5f;
                    P2.rect = P2R;
                    P2.transform.tag = "P2CAM";
                    m_playerTwoCam.GetComponent<AudioListener>().enabled = false;

                    m_playerThreeCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P3 = m_playerThreeCam.GetComponent<Camera>();
                    Rect P3R = new Rect(0, 0, W, H);
                    P3R.yMax = 0.5f;
                    P3R.xMin = 0.5f;
                    P3.rect = P3R;
                    P3.transform.tag = "P3CAM";
                    m_playerThreeCam.GetComponent<AudioListener>().enabled = false;
                    return;
                }
            //Create four camera's, they get one quarter of the screen each
            case (4):
                {
                    m_playerOneCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P1 = m_playerOneCam.GetComponent<Camera>();
                    Rect P1R = new Rect(0, 0, W, H);
                    P1R.yMin = 0.5f;
                    P1R.xMax = 0.5f;
                    P1.rect = P1R;
                    P1.transform.tag = "P1CAM";
                    
                    m_playerTwoCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P2 = m_playerTwoCam.GetComponent<Camera>();
                    Rect P2R = new Rect(0, 0, W, H);
                    P2R.yMin = 0.5f;
                    P2R.xMin = 0.5f;
                    P2.rect = P2R;
                    P2.transform.tag = "P2CAM";
                    m_playerTwoCam.GetComponent<AudioListener>().enabled = false;

                    m_playerThreeCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P3 = m_playerThreeCam.GetComponent<Camera>();
                    Rect P3R = new Rect(0, 0, W, H);
                    P3R.yMax = 0.5f;
                    P3R.xMax = 0.5f;
                    P3.rect = P3R;
                    P3.transform.tag = "P3CAM";
                    m_playerThreeCam.GetComponent<AudioListener>().enabled = false;

                    m_playerFourCam = GameObject.Instantiate(m_SplitScreenCamera, Vector3.zero, Quaternion.identity) as GameObject;
                    Camera P4 = m_playerFourCam.GetComponent<Camera>();
                    Rect P4R = new Rect(0, 0, W, H);
                    P4R.xMin = 0.5f;
                    P4R.yMax = 0.5f;
                    P4.rect = P4R;
                    P4.transform.tag = "P4CAM";
                    m_playerFourCam.GetComponent<AudioListener>().enabled = false;
                    return;
                }
            default:
                break;
        }
    }
}