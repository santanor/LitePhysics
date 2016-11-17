using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UniversalGravitation : MonoBehaviour {
    
    private PhysicsEngine[] physicsEngineArray;
    private const float bigG = 6.673e-11f;

    // Use this for initialization
    void Start () {
        physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine>();
    }

    void FixedUpdate()
    {
        CalculateGravity();
    }

    void CalculateGravity()
    {
        foreach (var pEngineA in physicsEngineArray)
        {
            foreach (var pEngineB in physicsEngineArray)
            {
                if (pEngineA != pEngineB)
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

}
