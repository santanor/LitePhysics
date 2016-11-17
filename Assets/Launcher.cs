using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {

    public PhysicsEngine ball;
    public float maxLaunchSpeed;
    public AudioClip windupSound;
    public AudioClip launchSound;

    public float speedIncrease;
    private float currentSpeed;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = windupSound;
        speedIncrease = (maxLaunchSpeed * Time.deltaTime) / audioSource.clip.length;
    }
    
    void OnMouseDown()
    {
        currentSpeed = 0;
        audioSource.clip = windupSound;
        audioSource.Play();
        InvokeRepeating("IncreaseLaunchSpeed", 0.5f, Time.fixedDeltaTime);
    }

    void IncreaseLaunchSpeed()
    {
        if (currentSpeed + speedIncrease * Time.deltaTime < maxLaunchSpeed)
        {
            currentSpeed += speedIncrease;
        }
        else
            currentSpeed = maxLaunchSpeed;
    }

    void OnMouseUp()
    {
        CancelInvoke();
        audioSource.Stop();
        audioSource.clip = launchSound;
        audioSource.Play();
        var newBall = Instantiate(ball) as PhysicsEngine;
        newBall.transform.parent = GameObject.Find("Launched balls").transform;

        Vector3 launchVector = new Vector3(1, 1, 0).normalized * currentSpeed;
        newBall.v = launchVector;

    }
}
