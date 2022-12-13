using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicShootScript : MonoBehaviour
{
    public int[] damagePerShot = new int[] { 35, 25, 10 };
    public int[] magSize = new int[] { 12, 30, 60 };
    public float[] shotInterval = new float[] { 0.7f, 0.1f, 0.05f }; //Can be found by dividing 60 by desired rpm of weapon
    public float reloadSpeed = 3f; //Time it takes to reload
    public float range = 50f;
    private int weaponNum = 0;
    // ^^^Would like for these variables to change based on the weapon player is using, when we implement them. Maybe with new script?
    public bool debug;
    public TMP_Text bulletsInMag;
    public TMP_Text equippedWeapon;
    public GameObject enemShot;
    GameObject cam;
    public float camHeightOffset;

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
        currMag = magSize[0];
        gunAudio = GetComponent<AudioSource>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position - new Vector3(0, camHeightOffset, 0), cam.transform.forward.normalized * 10, Color.red);
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= shotInterval[weaponNum] && reloadTimer == 0f) //Use of "Fire1" instead of "LeftClick" (or whatever it is on unity) makes it so we can play this game on a console, in theory
        {
            Shoot();
        }
        if (Input.GetKeyDown("r"))
        {
            currMag = 0;
        }
        if (Input.GetKeyDown("c"))
        {
            if (debug)
            {
                if (weaponNum == 2)
                {
                    weaponNum = 0;
                }
                else { ++weaponNum; }
                currMag = magSize[weaponNum];
            }
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
            currMag = magSize[weaponNum];
            reloadTimer = 0f;
        }
        if (weaponNum == 0)
        {
            equippedWeapon.text = "Rifle";
        }
        else if (weaponNum == 1)
        {
            equippedWeapon.text = "Assault Rifle";
        }
        else if (weaponNum == 2)
        {
            equippedWeapon.text = "LMG";
        }
    }

    void Shoot() //For now, no fancy animations, just want to get this up and running [took me way too long to start, sorry (: ]
    {
        timer = 0f;
        gunAudio.Play();
        currMag--;
        gunShooty.origin = cam.transform.position - new Vector3(0, 0.5f, 0);
        gunShooty.direction = cam.transform.forward;
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
                    enmHealth.TakeDamage(damagePerShot[weaponNum], shootyHit.collider.transform.position); //make enemy take damage
                    Instantiate(enemShot, shootyHit.point, shootyHit.collider.transform.rotation);
                }
            }
            else //if it is a tree, rock, or other object that you don't want to be able to shoot through
            {
                return;  //do nothing if its not an enemy
            }
        }
    }
}
