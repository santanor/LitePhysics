using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsEngine : MonoBehaviour {

    public float mass;              //[kg]
    public Vector3 v;               //[m s^-1]
    public Vector3 netForceVector;  //N []
    private List<Vector3> forceVectorList = new List<Vector3>();
    public bool showTrails = true;

    private const float bigG = 6.673e-11f; 

    private PhysicsEngine[] physicsEngineArray;
    // Use this for initialization
    void Start () {
        SetupForceTrails();
        physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine>();
	}
	
	void FixedUpdate()
    {
        RenderTrails();
        CalculateGravity();
        UpdatePosition();
    }

    public void AddForce(Vector3 force)
    {
        forceVectorList.Add(force);
    }

    void CalculateGravity()
    {
        foreach(var pEngineA in physicsEngineArray)
        {
            foreach (var pEngineB in physicsEngineArray)
            {
                if(pEngineA != pEngineB)
                {
                    var offset = pEngineA.transform.position - pEngineB.transform.position;
                    float rSquared = Mathf.Pow(offset.magnitude, 2f);
                    float graviMagnitude = bigG * pEngineA.mass * pEngineB.mass / rSquared;
                    Vector3 gravityFeltVector = graviMagnitude * offset.normalized;

                    pEngineA.AddForce(-gravityFeltVector);
                }
            }
        }
    }

    void UpdatePosition()
    {
        netForceVector = Vector3.zero;

        foreach (Vector3 forceVector in forceVectorList)
        {
            netForceVector += forceVector;
        }
        forceVectorList = new List<Vector3>();
        Vector3 acceleration = netForceVector / mass;
        v += acceleration * Time.deltaTime;
        transform.position += v * Time.deltaTime;
    }
       
    
    /// <summary>
    /// Draw the trail
    /// </summary>
    private LineRenderer lineRenderer;
    private int numberOfForces;

    // Use this for initialization
    void SetupForceTrails()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.yellow, Color.yellow);
        lineRenderer.SetWidth(0.2F, 0.2F);
        lineRenderer.useWorldSpace = false;
    }

    // Update is called once per frame
    void RenderTrails()
    {
        if (showTrails)
        {
            lineRenderer.enabled = true;
            numberOfForces = forceVectorList.Count;
            lineRenderer.SetVertexCount(numberOfForces * 2);
            int i = 0;
            foreach (Vector3 forceVector in forceVectorList)
            {
                lineRenderer.SetPosition(i, Vector3.zero);
                lineRenderer.SetPosition(i + 1, -forceVector);
                i = i + 2;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
