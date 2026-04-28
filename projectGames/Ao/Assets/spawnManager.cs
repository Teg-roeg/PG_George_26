using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public GameObject enemyCloneTemplate;
    List<enemyScript> allEnemies = new List<enemyScript>();
    Walking Player;
    int maxEnemies = 4;
    void Start()
    {
        populateEnemies();    
        Player = FindFirstObjectByType<Walking>();
        StartCoroutine(RepeatEveryTwoSeconds());
    }

    private void populateEnemies()
    {
        while(allEnemies.Count <= maxEnemies)
        {
            GameObject newEnemyGO = Instantiate(enemyCloneTemplate, new Vector3(UnityEngine.Random.Range(-10, 15), -2.70f, UnityEngine.Random.Range(-10, 15)), Quaternion.identity);
            allEnemies.Add(newEnemyGO.GetComponent<enemyScript>());
        }
    }
    IEnumerator RepeatEveryTwoSeconds()
    {
        while (true)
        {
            checkPositionPlayer();

            yield return new WaitForSeconds(1f);
        }
    }

    private void checkPositionPlayer()
    {
        foreach (enemyScript enemy in allEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, Player.transform.position) < 5f)
            {
                enemy.ThePlayerIs(Player);
            }
        }
    }

    void Update()
    {
        
    }
}
