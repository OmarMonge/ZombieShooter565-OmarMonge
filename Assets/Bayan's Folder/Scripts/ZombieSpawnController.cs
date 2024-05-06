using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiePW = 3;
    public int currentZombiePW;

    public float spawnDelay = 3.0f; //Delay of time between zombie spawns
    public int currWave = 0;
    public float waveCool = 7.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List <ZombieController> currZombAlive;

    public GameObject zombiePrefab;
    public Transform target;

    private void Start()
    {
        currentZombiePW = initialZombiePW;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currZombAlive.Clear();
        currWave++;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i = 0; i < currentZombiePW;++i)
        {
            //Offset from spawner to give spawn variety
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            //Create the zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            zombie.gameObject.GetComponentInChildren<ZombieController>().target = target;
            //Retrieve Zombie Script 
            ZombieController zombieScript = zombie.GetComponent<ZombieController>();

            //TrackZombie
            currZombAlive.Add(zombieScript);

            yield return new WaitForSeconds(spawnDelay);

        }
    }

    private void Update()
    {
        List<ZombieController> removeZomb = new List<ZombieController> ();
        foreach (ZombieController zombie in removeZomb)
        {
            if (zombie.isDead)
            {
                removeZomb.Add(zombie);
            }
        }

        foreach (ZombieController zombie in removeZomb)
        {
            currZombAlive.Remove(zombie);
        }

        removeZomb.Clear();

        if(currZombAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCool());
        }
    }

    private IEnumerator WaveCool()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCool);

        inCooldown = false;

        currentZombiePW *= 2;
        StartNextWave();
    }
}
