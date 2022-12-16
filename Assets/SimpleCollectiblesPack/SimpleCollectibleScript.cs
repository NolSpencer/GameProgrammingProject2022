using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public float rotationSpeed;
	public AudioClip collectSound;
	public GameObject collectEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect();
			if (CompareTag("Coin")) {
				other.GetComponent<PlayerHealth>().CollectCoins(100);
				gameObject.SetActive(false);
			}
			else if (CompareTag("HealthPack"))
			{
                other.GetComponent<PlayerHealth>().AddHealth(50f);
                gameObject.SetActive(false);
            }
		}
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
