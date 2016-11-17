using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsEngine))]
public class RocketEngine : MonoBehaviour {

    public float fuelMass;              // [kg]
    public float maxThrust;             // kN [kg m s^-2]
    [Range(0,1f )]
    public float thrustPercent;          // [none]
    public Vector3 thrustUnitVector = Vector3.zero;  // 
    private PhysicsEngine physicsEngine;

    private float currentThrust;        //N

    // Use this for initialization
    void Start()
    {
        physicsEngine = GetComponent<PhysicsEngine>();
        physicsEngine.mass += fuelMass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(fuelMass > FuelThisUpdate())
        {
            fuelMass -= FuelThisUpdate();
            physicsEngine.mass -= FuelThisUpdate();
            ExertForce();
        }else
        {
            Debug.LogWarning("Out of fuel");
        }
    }

    float FuelThisUpdate()
    {
        float exhaustMassFlow;
        float effectiveExhaustVelocity;

        effectiveExhaustVelocity = 4462f;           // [m s^-1] 

        //thrust = massFlow * exaustVelocity
        exhaustMassFlow = currentThrust / effectiveExhaustVelocity;    

        return exhaustMassFlow * Time.deltaTime;    //[kg]
    }

    void ExertForce()
    {
        currentThrust = thrustPercent * maxThrust * 1000f;//To get it in newtons
        Vector3 thrustVector = thrustUnitVector.normalized * currentThrust;
        physicsEngine.AddForce(thrustVector);
    }
}
