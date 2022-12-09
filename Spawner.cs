using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float objectRadius = 1f;
    public float tankRadius = 2f;

    float timeOfLastSpawn;

    public bool canSpawnEnemies = false;

    int attemptNumber;
    int maximumNumberOfAttempts = 100;

    Vector3 spawnPosition;

    Collider2D wallColl;
    Collider2D tankColl;

    void Update(){
        if(canSpawnEnemies)SpawnEnemies();
    }

    float SpawnCooldown(float x){
        float y = (100/x) + 1;
        if(y > 8) y = 8f;
        return y;
    }

    void SpawnEnemies(){
        if(Time.timeSinceLevelLoad - timeOfLastSpawn >= SpawnCooldown(Time.timeSinceLevelLoad) && GameManager.instance.State == GameState.Alive){
            attemptNumber = 0;
            while(attemptNumber <= maximumNumberOfAttempts){
                attemptNumber++;

                spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0,Camera.main.pixelWidth),
                                                                                 Random.Range(0,Camera.main.pixelHeight),
                                                                                 0f));
                spawnPosition.z = 0f;

                wallColl = Physics2D.OverlapCircle(spawnPosition,objectRadius);
                tankColl = Physics2D.OverlapCircle(spawnPosition,tankRadius,LayerMask.GetMask("Player","Enemy"));

                if(wallColl != null || tankColl != null){
                    continue;
                }else{
                    Instantiate(enemyPrefab,spawnPosition,enemyPrefab.transform.rotation);
                    FindObjectOfType<AudioManager>().Play("Spawn");
                    timeOfLastSpawn = Time.timeSinceLevelLoad;
                    break;
                }
            }
        }
    }

}
