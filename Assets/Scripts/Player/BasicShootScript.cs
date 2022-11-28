using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicShootScript : MonoBehaviour
{
    public int damagePerShot = 5;
    public int magSize = 12;
    public float shotInterval = 0.7f; //Can be found by dividing 60 by desired rpm of weapon
    public float reloadSpeed = 3f; //Time it takes to reload
    public float range = 50f;
    // ^^^Would like for these variables to change based on the weapon player is using, when we implement them. Maybe with new script?
    public bool debug;
    public TMP_Text bulletsInMag;

    float timer;
    float reloadTimer = 0f;
    int currMag;
    Ray gunShooty;
    RaycastHit shootyHit;
    AudioSource gunAudio;
    int shootableMask;

    // Start is called before the first frame update
    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        currMag = magSize;
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= shotInterval && reloadTimer == 0f) //Use of "Fire1" instead of "LeftClick" (or whatever it is on unity) makes it so we can play this game on a console, in theory
        {
            Shoot();
        }
        if (Input.GetKeyDown("r"))
        {
            currMag = 0;
        }
        if (currMag == 0)
        {
            reloadTimer += Time.deltaTime;
            bulletsInMag.text = "Reloading...";
        }
        else
        {
            bulletsInMag.text = currMag.ToString();
        }
        if (reloadTimer >= reloadSpeed)
        {
            currMag = magSize;
            reloadTimer = 0f;
        }
    }

    void Shoot() //For now, no fancy animations, just want to get this up and running [took me way too long to start, sorry (: ]
    {
        timer = 0f;
        gunAudio.Play();
        currMag--;
        gunShooty.origin = transform.position;
        gunShooty.direction = transform.forward;
        if (Physics.Raycast(gunShooty, out shootyHit, range, shootableMask))
        {
            if (debug)
            {
                Debug.Log(shootyHit.collider.gameObject.name);
            }
            if (shootyHit.collider.CompareTag("Enemy")) //checks to see if the object shot was an enemy or not
            {
                EnemyHealth enmHealth = shootyHit.collider.GetComponentInParent<EnemyHealth>();
                if (enmHealth != null)
                {
                    enmHealth.TakeDamage(damagePerShot, shootyHit.collider.transform.position); //make enemy take damage
                }
            }
            else //if it is a tree, rock, or other object that you don't want to be able to shoot through
            {
                return;  //do nothing if its not an enemy
            }
        }
    }
}
