// ---------------------------------------------------------------------------
// BikeControlLocak.cs
// 
// Control Script for moving bikes around in local splitscreen gameplay
// 
// Original Author: Harley Laurie
// ---------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class BikeControlLocal : MonoBehaviour 
{
    private float m_currentBikeSpeed;
    public float m_bikeMaxSpeed;
    public float m_turnSpeed;
    public float m_leanStrength;
    public float m_maxLean;
    public float m_acceleration;

    private GameObject m_bikeChild;
    private GameObject m_lightsChild;
    private float m_childRotation;
    public GameObject m_trailPrefab;
    private GameObject m_myTrail;
    private int m_playerNumber;

    //Set to true when match starts, allows bike to move, recieve input etc.
    private bool m_bikeReady = false;

    void Start()
    {
        m_bikeChild = transform.FindChild("Tronbike").gameObject;
        m_lightsChild = transform.FindChild("TronLights").gameObject;
    }

    void Update()
    {
        //Allow player to control once the match has started
        if(m_bikeReady)
        {
            //Accelerate to max speed
            if (m_currentBikeSpeed < m_bikeMaxSpeed)
                m_currentBikeSpeed += m_acceleration * Time.deltaTime;
            MoveBikeForward();
            SteerBike();
        }
    }

    //Sets player number, used so we know what control input to listen to
    public void SetPlayerNumber(int a_playerNumber)
    {
        m_playerNumber = a_playerNumber;
    }

    public void Cleanup()
    {
        GameObject.Destroy(m_myTrail);
        GameObject.Destroy(this.gameObject);
    }

    //Called by match manager once countdown has
    //complete and match has begun
    public void StartMatch()
    {
        //Activate box collider
        transform.GetComponent<BoxCollider>().enabled = true;
        //Turn gravity on
        transform.GetComponent<Rigidbody>().useGravity = true;
        //Unlock controls
        m_bikeReady = true;
        //Add a trail renderer
        m_myTrail = GameObject.Instantiate(m_trailPrefab, transform.position, Quaternion.identity) as GameObject;
        //Parent it
        m_myTrail.transform.parent = transform;
        //Activate collider spawner
        transform.GetComponent<ColliderSpawnerLocal>().enabled = true;
        //Spawn new collider spawner parent
        transform.GetComponent<ColliderSpawnerLocal>().CreateNewColliderParent();
    }

    public void MoveTo(Vector3 a_newPos)
    {
        transform.position = a_newPos;
        transform.LookAt(GameObject.Find("ArenaCenter").transform.position);
    }

    private void MoveBikeForward()
    {
        //Current position
        Vector3 P = transform.position;
        //Move forward
        P += transform.forward * (m_currentBikeSpeed * Time.deltaTime);
        //Update pos
        transform.position = P;
    }

    private void SteerBike()
    {
        float l_horizontalAxis = 0;

        //Split the input between the different controllers
        switch(m_playerNumber)
        {
            case (1):
                {
                    l_horizontalAxis = Input.GetAxis("Horizontal");
                    break;
                }
            case (2):
                {
                    l_horizontalAxis = Input.GetAxis("Horizontal2");
                    break;
                }
            case (3):
                {
                    l_horizontalAxis = Input.GetAxis("Horizontal3");
                    break;
                }
            case (4):
                {
                    l_horizontalAxis = Input.GetAxis("Horizontal4");
                    break;
                }
            default:
                break;
        }

        if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            TurnLeft();
        else if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            TurnRight();
        else if (l_horizontalAxis < 0.0f)
            TurnLeftJoystick(l_horizontalAxis);
        else if (l_horizontalAxis > 0.0f)
            TurnRightJoystick(l_horizontalAxis);
        else
            ResetLeaning();
    }

    private void TurnLeftJoystick(float a_fAxisValue)
    {
        a_fAxisValue = -a_fAxisValue;
        transform.RotateAround(transform.up, Time.deltaTime * -m_turnSpeed * a_fAxisValue);
        if (m_childRotation < m_maxLean)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * m_leanStrength * a_fAxisValue);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * m_leanStrength * a_fAxisValue);
            m_childRotation += Time.deltaTime * m_leanStrength * a_fAxisValue;
        }
    }
    private void TurnLeft()
    {
        transform.RotateAround(transform.up, Time.deltaTime * -m_turnSpeed);
        //As we turn left we want the bike to lean into the corner
        if (m_childRotation < m_maxLean)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * m_leanStrength);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * m_leanStrength);
            m_childRotation += Time.deltaTime * m_leanStrength;
        }
    }

    private void TurnRightJoystick(float a_fAxisValue)
    {
        transform.RotateAround(transform.up, Time.deltaTime * m_turnSpeed * a_fAxisValue);
        if (m_childRotation > -m_maxLean)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * -m_leanStrength * a_fAxisValue);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * -m_leanStrength * a_fAxisValue);
            m_childRotation -= Time.deltaTime * m_leanStrength * a_fAxisValue;
        }
    }

    private void TurnRight()
    {
        transform.RotateAround(transform.up, Time.deltaTime * m_turnSpeed);
        //As we turn left we want the bike to lean into the corner
        if (m_childRotation > -m_maxLean)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * -m_leanStrength);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * -m_leanStrength);
            m_childRotation -= Time.deltaTime * m_leanStrength;
        }
    }

    private void ResetLeaning()
    {
        if ((m_childRotation < 0.1f) && (m_childRotation > -0.1f))
            return;

        if (m_childRotation < 0.1f)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * m_leanStrength);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * m_leanStrength);
            m_childRotation += Time.deltaTime * m_leanStrength;
        }
        else if (m_childRotation > -0.1f)
        {
            m_bikeChild.transform.RotateAround(m_bikeChild.transform.right, Time.deltaTime * -m_leanStrength);
            m_lightsChild.transform.RotateAround(m_lightsChild.transform.right, Time.deltaTime * -m_leanStrength);
            m_childRotation -= Time.deltaTime * m_leanStrength;
        }
    }
}