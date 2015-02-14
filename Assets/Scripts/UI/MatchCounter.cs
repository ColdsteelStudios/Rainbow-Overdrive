// ---------------------------------------------------------------------------
// MatchCounter.cs
// 
// Displays countdown on GUI alterting how long until start of new match
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchCounter : MonoBehaviour
{
    public Texture2D[] m_texNumbers;
    private Image m_currentImageDisplay;

    void Start()
    {
        m_currentImageDisplay = GetComponent<Image>();
    }

    public void SetCounterDisplay(int a_displayNumber)
    {
        
    }
}