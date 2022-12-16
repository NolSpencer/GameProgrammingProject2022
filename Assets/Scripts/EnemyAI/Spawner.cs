using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public bool inShop = false;
    public bool waveActive = true;
    int waveSize = 8;
    int maxEnemies = 3;
    int currEnemies = 2;
    int currWave = 1;
    public int enemiesDefeated = 0;
    float betweenWaveTimer = 0;
    public TMP_Text waveTMR;
    public TMP_Text wave;
    public TMP_Text enemiesLeft;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    private void Update()
    {
        if (betweenWaveTimer > 0 && !inShop)
        {
            betweenWaveTimer -= Time.deltaTime;
            waveTMR.text = "Time until next wave: " + betweenWaveTimer.ToString();
            if (waveActive) 
            {
                waveTMR.enabled = true;
                waveActive = false;
                CancelInvoke();
                waveTMR.enabled = true;
                audioSource.Play();

            }
        }
        else if (betweenWaveTimer <= 0 && !waveActive)
        {
            waveActive = true;
            ++currWave;
            ++waveSize;
            if (currWave % 5 == 0)
            {
                ++maxEnemies;
            }
            InvokeRepeating("Spawn", spawnTime, spawnTime);
            waveTMR.enabled = false;
            enemiesDefeated = 0;
        }
        wave.text = "Wave: " + currWave.ToString();
        int enemLeft = waveSize - enemiesDefeated;
        enemiesLeft.text = "Enemies Left: " + enemLeft.ToString();
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        if (currEnemies < maxEnemies && enemiesDefeated + currEnemies < waveSize)
        {
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            ++currEnemies;
        }
        else if (enemiesDefeated == waveSize)
        {
            betweenWaveTimer = 20f;
        }
    }

    public void EnemyKilled()
    {
        enemiesDefeated++;
        --currEnemies;
    }
}