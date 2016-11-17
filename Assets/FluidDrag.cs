using UnityEngine;
using System.Collections;

public class FluidDrag : MonoBehaviour {

    [Range(1,2)]
    public float velocityExponent;
    public float dragConstant;      // 
    private PhysicsEngine physicsEngine;

	// Use this for initialization
	void Start () {
        physicsEngine = GetComponent<PhysicsEngine>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 velocityVector = physicsEngine.netForceVector;
        float speed = velocityVector.magnitude;
        float dragSize = CalculateDrag(speed);
        Vector3 dragVector = dragSize * -velocityVector.normalized;
        physicsEngine.AddForce(dragVector);
	}

    float CalculateDrag(float velocity)
    {
        return dragConstant * Mathf.Pow(velocity, velocityExponent);
    }
}
