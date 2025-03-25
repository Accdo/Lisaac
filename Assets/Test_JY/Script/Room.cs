using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;

public class Room : MonoBehaviour
{
    private const int WORM = 1; // 벌레 몬스터
    private const int GUT = 2;  // 장기 몬스터

    public GameObject[] doorObjects; // 0:위 1:아래 2:왼쪽 3:오른쪽
    public GameObject[] spanwPos;    // 적 스폰 위치들
    public int[] enemyType;          // 적 타입 정보

    private bool isSpawn = false;    // 적 스폰 여부

    // 방에 연결된 문 열기 설정
    public void Setup(bool[] doorStates)
    {
        for (int i = 0; i < doorObjects.Length; i++)
        {
            doorObjects[i].SetActive(doorStates[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 방에 들어오면 적 스폰
        if (collision.gameObject.CompareTag("Player"))
        {
            SpanwEnemy();
        }
    }

    private void SpanwEnemy()
    {
        if (!isSpawn)
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
