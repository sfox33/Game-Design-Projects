using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    float accelerationFactor;
    float initialSpeed;

    public GameObject explosionEffect;
    public AudioClip explosionSound;
    public float rotationSpeed = 10f;

    public float currentSpeed = 0;
    int direction;

    public Vector3 initialPosition;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(currentSpeed * direction * Time.deltaTime, 0, 0), Space.World);
        currentSpeed *= accelerationFactor;

        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
	}

    public void setup(float speed, float accel)
    {
        initialSpeed = speed;
        accelerationFactor = accel;
    }

    public void startBomb()
    {
        currentSpeed = initialSpeed;
    }

    public void explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        GetComponent<AudioSource>().PlayOneShot(explosionSound);
		StartCoroutine(stop());
    }

	public IEnumerator stop()
    {
		currentSpeed = 0;
		direction = 0;
		transform.position = new Vector3 (initialPosition.x, initialPosition.y - 5.0f, initialPosition.z);
		yield return StartCoroutine (Delay(Random.Range(0.0f,3.0f)));
		transform.position = new Vector3(initialPosition.x + Random.Range(-1.0f, 1.0f), initialPosition.y, initialPosition.z);
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
        set { currentSpeed = value; }
    }

    public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public float Position
    {
        get { return transform.position.x; }
    }

	IEnumerator Delay(float time) {
		yield return new WaitForSeconds (time);
	}
}
