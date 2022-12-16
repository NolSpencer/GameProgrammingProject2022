using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitScript : MonoBehaviour
{
    public Transform teleportPos;
    public GameObject player;
    GameObject Spawner;

    private void Start()
    {
        Spawner = GameObject.FindGameObjectWithTag("Spawner");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Spawner.GetComponent<Spawner>().waveActive)
        {
            player.transform.position = teleportPos.transform.position + new Vector3(0f, 0f, 1f);
            Spawner.GetComponent<Spawner>().inShop = false;
        }
    }
}
