// ---------------------------------------------------------------------------
// MenuControl.cs
// 
// Controls menu system with controller
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour
{
    enum CurrentMenu
    {
        Void = 0,
        Main = 1,
        SplitScreen = 2
    }

    public GameObject Background;
    public GameObject MainMenu;
    public GameObject SplitScreenMenu;

    private float m_buttonInputCooldown = 0.25f;
    private float m_buttonInputCooldownRemainder = 0.0f;
    private CurrentMenu m_currentMenu;

    void Start()
    {
        m_currentMenu = CurrentMenu.Main;
        MainMenu.SetActive(true);
        Background.SetActive(true);
    }

    void Update()
    {
        GetButtonInput();
    }

    private void GetButtonInput()
    {
        //First check for dpad input for menu navigation
        float DPadVert = Input.GetAxis("DPadVertical");
        float DPadHori = Input.GetAxis("DPadHorizontal");

        //Next check for left thumbstick for menu navigation
        float LTSVert = Input.GetAxis("Vertical");
        float LTSHori = Input.GetAxis("Horizontal");

        m_buttonInputCooldownRemainder -= Time.deltaTime;

        if (m_buttonInputCooldownRemainder <= 0.0f)
        {
            if ((DPadVert == -1) || (LTSHori == -1))
                Left();
            else if ((DPadVert == 1) || (LTSHori == 1))
                Right();
            else if ((DPadHori == 1) || (LTSVert == 1))
                Up();
            else if ((DPadHori == -1) || (LTSVert == -1))
                Down();
        }

        if (Input.GetButtonDown("AButton"))
            A();
    }

    private void Up()
    {
        m_buttonInputCooldownRemainder = m_buttonInputCooldown;
        switch(m_currentMenu)
        {
            case (CurrentMenu.Main):
                MainMenu.SendMessage("Up");
                break;
            case (CurrentMenu.SplitScreen):
                SplitScreenMenu.SendMessage("Up");
                break;
            default:
                break;
        }
    }

    private void Down()
    {
        m_buttonInputCooldownRemainder = m_buttonInputCooldown;
        switch (m_currentMenu)
        {
            case (CurrentMenu.Main):
                MainMenu.SendMessage("Down");
                break;
            case (CurrentMenu.SplitScreen):
                SplitScreenMenu.SendMessage("Down");
                break;
            default:
                break;
        }
    }

    private void Left()
    {
        m_buttonInputCooldownRemainder = m_buttonInputCooldown;
        switch (m_currentMenu)
        {
            case (CurrentMenu.Main):
                MainMenu.SendMessage("Left");
                break;
            case (CurrentMenu.SplitScreen):
                SplitScreenMenu.SendMessage("Left");
                break;
            default:
                break;
        }
    }

    private void Right()
    {
        m_buttonInputCooldownRemainder = m_buttonInputCooldown;
        switch (m_currentMenu)
        {
            case (CurrentMenu.Main):
                MainMenu.SendMessage("Right");
                break;
            case (CurrentMenu.SplitScreen):
                SplitScreenMenu.SendMessage("Right");
                break;
            default:
                break;
        }
    }

    private void A()
    {
        switch (m_currentMenu)
        {
            case (CurrentMenu.Main):
                MainMenu.SendMessage("A");
                break;
            case (CurrentMenu.SplitScreen):
                SplitScreenMenu.SendMessage("A");
                break;
            default:
                break;
        }
    }

	private void GoToSplitScreen()
	{
        m_currentMenu = CurrentMenu.SplitScreen;
		MainMenu.SetActive(false);
		SplitScreenMenu.SetActive (true);
		SplitScreenMenu.SendMessage ("SetDefault");
	}

	private void GoToMainMenu()
	{
        m_currentMenu = CurrentMenu.Main;
        SplitScreenMenu.SetActive (false);
		MainMenu.SetActive(true);
		MainMenu.SendMessage("SetDefault");
	}

    private void GoToPlay()
    {
        m_currentMenu = CurrentMenu.Void;
        SplitScreenMenu.SetActive(false);
        Background.SetActive(false);
    }
}