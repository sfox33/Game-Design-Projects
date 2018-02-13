using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
	public float rotation = 1.0f;
	public Transform transform;
	public float speed = 1.0f;
	private bool collision = false;
	private float defaultPosY;
	// Use this for initialization
	void Start () {
		defaultPosY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0.0f, speed, 0.0f);

		if (collision) {
			if(transform.position.y <= defaultPosY+2.0f) {
				transform.position = new Vector3 (transform.position.x, transform.position.y + 0.02f, transform.position.z);
			}
			speed += 0.1f;
			if (transform.position.y > defaultPosY+2.0f) {
				transform.localScale -= new Vector3 (0.01f, 0.01f, 0.01f);
			}
			if(transform.localScale.x <= 0.1f){
				print ("Should be gone now");
				Destroy(gameObject, 0.0f);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		//print ("Collision detected");
		if (other.gameObject.tag == "Player") {
			print ("Inside the if");
			collision = true;
		}
	}
}
