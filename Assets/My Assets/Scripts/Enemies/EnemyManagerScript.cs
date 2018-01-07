using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{

    [Header("Captain Squad Properties")]
    [SerializeField] GameObject captainPrefab;
    [SerializeField] Transform[] CoverPointArray;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int maxMinions;
    private int currentMinionCount = 0;

    //Shared Variables
    [HideInInspector] public GameObject selectedEnemy = null;
    [HideInInspector] public List<Transform> activeEnemies;

    //Instance Variables
    public static EnemyManagerScript Instance;

    void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;

        activeEnemies = new List<Transform>();
    }

    private void Start()
    {
        InvokeRepeating("SpawnMinion", 0.5f, 3.0f);

    }

    //Manager Behaviours
    void SpawnMinion()
    {
        //Stop Spawning Minions
        if (currentMinionCount >= maxMinions)
        {
            CancelInvoke("SpawnMinion");
            return;
        }

        Transform spawn = GetRandomSpawn();
        GameObject enemy = Instantiate(captainPrefab, spawn.position, Quaternion.identity);
        activeEnemies.Add(enemy.GetComponent<Transform>()); //adding enemies
        currentMinionCount++;
    }

    Transform GetRandomSpawn()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }

    //Enemy Request Behaviours
    public Vector3 RequestCoverPoint()
    {
        //Get a coverpoint to work with
        Transform CoverSectionGO = GetCoverPoint();
        CoverSectionScript coverSection = CoverSectionGO.GetComponent<CoverSectionScript>();

        //Choose a random spot within the specified coverpoint
        float targetX = Random.Range(coverSection.minSizeX, coverSection.maxSizeX);
        float targetZ = Random.Range(coverSection.minSizeZ, coverSection.maxSizeZ);
        Vector3 targetPos = new Vector3(targetX, 0.0f, targetZ);

        return targetPos;
    }

    Transform GetCoverPoint()
    {
        //Choose random int 
        int index = Random.Range(0, CoverPointArray.Length);
        return CoverPointArray[index];
    }
}
