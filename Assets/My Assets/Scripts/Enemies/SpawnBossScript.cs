using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossScript : MonoBehaviour {

    [SerializeField] float spawnBossDelay = 60.0f;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform bossSpawn;

	void Start()
    {
        StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(spawnBossDelay);
        Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);

        yield break;
    }

}
