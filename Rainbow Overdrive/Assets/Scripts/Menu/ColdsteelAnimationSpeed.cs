// ---------------------------------------------------------------------------
// ColdsteelAnimationSpeed.cs
// 
// Class description goes here
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ColdsteelAnimationSpeed : MonoBehaviour 
{
	public float m_animationSpeed;
	private float m_timePassed;
	private float m_desiredTime;

	void Start () 
	{
		gameObject.GetComponent<Animator> ().speed = m_animationSpeed;
		m_timePassed = 0.0f;
		m_desiredTime = 7.0f;
	}

	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Escape))
			Application.LoadLevel ("Arena");
		m_timePassed += Time.deltaTime;
		if(m_timePassed >= m_desiredTime)
			Application.LoadLevel ("Arena");
	}
}