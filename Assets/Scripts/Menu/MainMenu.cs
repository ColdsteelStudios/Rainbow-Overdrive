// ---------------------------------------------------------------------------
// MainMenu.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    enum CurrentButton
    {
        Null = 0,
        Split = 1,
        Quit = 2
    }

    public GameObject SplitScreenButton;
    public GameObject QuitButton;
    private CurrentButton m_currentButton;

    void Start()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        m_currentButton = CurrentButton.Split;
        SplitScreenButton.GetComponent<ButtonImageChange>().SetHighlight();
		QuitButton.SendMessage ("DisableHighlight");
    }

    public void A()
    {
        switch(m_currentButton)
        {
		case(CurrentButton.Split):
			transform.root.SendMessage("GoToSplitScreen");
			break;
		case(CurrentButton.Quit):
            if (!Application.isEditor)
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
			break;
		default:
			break;
        }
    }

    public void Up()
    {
		switch(m_currentButton)
		{
		case(CurrentButton.Split):
			m_currentButton = CurrentButton.Quit;
			QuitButton.SendMessage ("SetHighlight");
			SplitScreenButton.SendMessage ("DisableHighlight");
			break;
		case(CurrentButton.Quit):
			m_currentButton = CurrentButton.Split;
			SplitScreenButton.SendMessage("SetHighlight");
			QuitButton.SendMessage ("DisableHighlight");
			break;
		default:
			break;
		}
    }

    public void Down()
    {
		switch(m_currentButton)
		{
		case(CurrentButton.Split):
			m_currentButton = CurrentButton.Quit;
			QuitButton.SendMessage ("SetHighlight");
			SplitScreenButton.SendMessage ("DisableHighlight");
			break;
		case(CurrentButton.Quit):
			m_currentButton = CurrentButton.Split;
			SplitScreenButton.SendMessage("SetHighlight");
			QuitButton.SendMessage ("DisableHighlight");
			break;
		default:
			break;
		}
    }
}