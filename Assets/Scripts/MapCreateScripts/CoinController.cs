using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
        Debug.Log("asdf");
		if (other.tag == "Player") {
			Collect();
            Debug.Log("Player");
		}
	}

    void Collect()
    {
        Debug.Log("Coll");
		Destroy (gameObject);
		// if(collectSound)
		// 	AudioSource.PlayClipAtPoint(collectSound, transform.position);
		// if(collectEffect)
		// 	Instantiate(collectEffect, transform.position, Quaternion.identity);
    }
}
