using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using static RoomType;

public class Room : MonoBehaviour
{
    public GameObject[] doorObjects; // 0:위 1:아래 2:왼쪽 3:오른쪽
    public GameObject[] spanwPos;    // 적 스폰 위치들
    public MonsterType[] enemyType;          // 적 타입 정보

    private bool isSpawn = false;    // 적 스폰 여부
    public bool isInPlayer = false;

    // 방에 연결된 문 열기 설정
    public void Setup(bool[] doorStates, RoomTypeEnum roomType)
    {
        for (int i = 0; i < doorObjects.Length; i++)
        {
            doorObjects[i].GetComponent<Door>().DoorTypeSelect(roomType);
            doorObjects[i].SetActive(doorStates[i]);
        }
    }

    public void DoorChange(int doorDire, RoomTypeEnum doorType)
    {
        doorObjects[doorDire].GetComponent<Door>().DoorTypeSelect(doorType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 방에 들어오면 적 스폰
        if (collision.gameObject.CompareTag("Player"))
        {
            SpanwEnemy();
            isInPlayer = true;
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            isInPlayer = false;
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
                    case MonsterType.WORM:
                        SpawnManager.Instance.SpawnWorm(spanwPos[i].transform.position);
                        break;
                    case MonsterType.GUT:
                        SpawnManager.Instance.SpawnGut(spanwPos[i].transform.position);
                        break;
                    case MonsterType.Boss:
                        SpawnManager.Instance.SpawnBoss(spanwPos[i].transform.position);
                        break;
                }
            }

            isSpawn = true;
        }
    }
}
