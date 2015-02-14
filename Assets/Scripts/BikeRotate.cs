// ---------------------------------------------------------------------------
// BikeRotate.cs
// 
// Rotates the object on the Y axis
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class BikeRotate : MonoBehaviour 
{
	public float m_rotationSpeed;

	void Update()
	{
		transform.Rotate (new Vector3 (0, 1, 0), m_rotationSpeed * Time.deltaTime);
	}
}