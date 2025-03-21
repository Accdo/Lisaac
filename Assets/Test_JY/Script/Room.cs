using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;


public class Room : MonoBehaviour
{

    private const int WORM = 1;
    private const int GUT = 2;

    public GameObject[] doorObjects; // 상하좌우 4개의 문 프리팹
    public GameObject[] spanwPos;
    public int[] enemyType;

    private bool isSpawn = false;


    public void Setup(bool[] doorStates)
    {
        for (int i = 0; i < doorObjects.Length; i++)
        {
            doorObjects[i].SetActive(doorStates[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpanwEnemy();
        }
    }
    private void SpanwEnemy()
    {
        if(!isSpawn)
        {
            for (int i = 0; i < spanwPos.Length; i++)
            {
                switch (enemyType[i])
                {
                    case WORM:
                        SpawnManager.Instance.SpawnWorm(spanwPos[i].transform.position);
                        break;
                    case GUT:
                        SpawnManager.Instance.SpawnGut(spanwPos[i].transform.position);
                        break;
                }
            }

            isSpawn = true;
        }
    }

}
