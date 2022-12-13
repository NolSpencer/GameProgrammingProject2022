using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeScript : MonoBehaviour
{

    float lifeTime = 0.5f;
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
