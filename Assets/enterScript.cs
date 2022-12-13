using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterScript : MonoBehaviour
{
    public Transform teleportPos;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportPos.transform.position + new Vector3(0f, 0f, -1f);
    }
}
