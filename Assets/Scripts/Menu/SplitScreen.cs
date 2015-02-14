// ---------------------------------------------------------------------------
// ButtonImageChange.cs
// 
// Controls the player count selection menu for splitscreen play
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SplitScreen : MonoBehaviour 
{
	enum CurrentSplitScreen
	{
		Void = 0,
		Back = 1,
		Two = 2,
		Three = 3,
		Four = 4
	}

	public GameObject BackButton;
	public GameObject TwoPlayerButton;
	public GameObject ThreePlayerButton;
	public GameObject FourPlayerButton;
	private CurrentSplitScreen CurrentButton = CurrentSplitScreen.Void;

	void Start()
	{
		SetDefault();
	}

	private void SetDefault()
	{
		CurrentButton = CurrentSplitScreen.Two;
		BackButton.SendMessage ("DisableHighlight");
		TwoPlayerButton.SendMessage ("SetHighlight");
		ThreePlayerButton.SendMessage("DisableHighlight");
		FourPlayerButton.SendMessage("DisableHighlight");
	}

    private void Up()
    {
        switch(CurrentButton)
        {
            case (CurrentSplitScreen.Two):
                TwoPlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Three):
                ThreePlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Four):
                FourPlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Back):
                ThreePlayerButton.SendMessage("SetHighlight");
                BackButton.SendMessage("DisableHighlight");
                CurrentButton = CurrentSplitScreen.Three;
                break;
            default:
                break;
        }
    }

    private void Down()
    {
        switch (CurrentButton)
        {
            case (CurrentSplitScreen.Two):
                TwoPlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Three):
                ThreePlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Four):
                FourPlayerButton.SendMessage("DisableHighlight");
                BackButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Back;
                break;
            case (CurrentSplitScreen.Back):
                ThreePlayerButton.SendMessage("SetHighlight");
                BackButton.SendMessage("DisableHighlight");
                CurrentButton = CurrentSplitScreen.Three;
                break;
            default:
                break;
        }
    }

    private void Left()
    {
        switch (CurrentButton)
        {
            case (CurrentSplitScreen.Two):
                TwoPlayerButton.SendMessage("DisableHighlight");
                FourPlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Four;
                break;
            case (CurrentSplitScreen.Three):
                ThreePlayerButton.SendMessage("DisableHighlight");
                TwoPlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Two;
                break;
            case (CurrentSplitScreen.Four):
                FourPlayerButton.SendMessage("DisableHighlight");
                ThreePlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Three;
                break;
            default:
                break;
        }
    }

    private void Right()
    {
        switch (CurrentButton)
        {
            case (CurrentSplitScreen.Two):
                TwoPlayerButton.SendMessage("DisableHighlight");
                ThreePlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Three;
                break;
            case (CurrentSplitScreen.Three):
                ThreePlayerButton.SendMessage("DisableHighlight");
                FourPlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Four;
                break;
            case (CurrentSplitScreen.Four):
                FourPlayerButton.SendMessage("DisableHighlight");
                TwoPlayerButton.SendMessage("SetHighlight");
                CurrentButton = CurrentSplitScreen.Two;
                break;
            default:
                break;
        }
    }

    private void A()
    {
        switch (CurrentButton)
        {
            case (CurrentSplitScreen.Two):
                GameObject.Find("System").GetComponent<LocalPlaySetup>().m_playerCount = 2;
                GameObject.Find("System").GetComponent<LocalPlaySetup>().StartLocalPlay();
                transform.root.SendMessage("GoToPlay");
                break;
            case (CurrentSplitScreen.Three):
                Debug.Log("Play Three Player");
                break;
            case (CurrentSplitScreen.Four):
                Debug.Log("Play Four Player");
                break;
            case (CurrentSplitScreen.Back):
                transform.root.SendMessage("GoToMainMenu");
                break;
            default:
                break;
        }
    }
}