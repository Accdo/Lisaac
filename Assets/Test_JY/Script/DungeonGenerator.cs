using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int maxRooms = 10;
    public GameObject roomPrefab;
    public float roomSpacingX = 18f;
    public float roomSpacingY = 10f;

    private Dictionary<Vector2Int, RoomData> rooms = new Dictionary<Vector2Int, RoomData>();
    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        rooms.Clear();

        // 시작 방 (0, 0)
        Vector2Int startPos = Vector2Int.zero;
        RoomData startRoom = new RoomData(startPos);
        rooms.Add(startPos, startRoom);

        // 생성 후보 리스트
        List<Vector2Int> roomPositions = new List<Vector2Int> { startPos };

        while (rooms.Count < maxRooms)
        {
            // 랜덤으로 기존 방 하나 선택
            Vector2Int currentPos = roomPositions[Random.Range(0, roomPositions.Count)];

            // 랜덤 방향 선택 (상 하 좌 우)
            int dirRandomValue = Random.Range(0, directions.Count);
            Vector2Int randomDir = directions[dirRandomValue];
            Vector2Int newPos = currentPos + randomDir;

            // 중복 시 컨티뉴
            if (rooms.ContainsKey(newPos))
            {
                continue;
            }

            // 새로운 방 생성
            RoomData newRoom = new RoomData(newPos);

            // 리스트 추가
            rooms.Add(newPos, newRoom);
            roomPositions.Add(newPos);
        }

        int counting1 = 0;
        
        // 문 연결
        foreach (var i in rooms)
        {
            int counting2 = 0;
            foreach (var j in rooms)
            {
                /*Debug.Log(counting1 + " : " + counting2 + "    |    " + i.Key.x + " : " + i.Key.y + "    |    " + j.Key.x + " : " + j.Key.y);*/
                
                // i랑 j가 같은 룸인지 검사
                if (i.Key.x == j.Key.x && i.Key.y == j.Key.y)
                {
                    counting2++;
                    continue;
                }
                else if(i.Key.x == j.Key.x && IsDifferenceOne(i.Key.y, j.Key.y))
                {
                    // X 좌표는 같고 Y좌표가 1 차이날때 (위 아래 or 아래 위 문연결)
                    if(i.Key.y < j.Key.y)
                    {
                        i.Value.doors[0] = true;
                        j.Value.doors[1] = true;
                    }
                    else if (i.Key.y > j.Key.y)
                    {
                        i.Value.doors[1] = true;
                        j.Value.doors[0] = true;
                    }
                    else
                    {
                        Debug.LogError($"1있을수 없는 일! : {i.Key.y} | {j.Key.y}");
                    }
                }
                else if(i.Key.y == j.Key.y && IsDifferenceOne(i.Key.x, j.Key.x))
                {
                    // Y 좌표는 같고 X좌표가 1 차이날때 (오른쪽 왼쪽 or 왼쪽 오른쪽 문연결)
                    if (i.Key.x < j.Key.x)
                    {
                        i.Value.doors[3] = true;
                        j.Value.doors[2] = true;
                    }
                    else if (i.Key.x > j.Key.x)
                    {
                        i.Value.doors[2] = true;
                        j.Value.doors[3] = true;
                    }
                    else
                    {
                        Debug.LogError($"2있을수 없는 일! : {i.Key.x} | {j.Key.x}");
                    }
                }
                counting2++;

            }
            counting1++;
        }

        foreach (var roomPair in rooms)
        {
            // 룸 프리팹 게임에 생성
            SpawnRoom(roomPair.Value);
        }

    }


    void SpawnRoom(RoomData data)
    {
        Vector3 worldPos = new Vector3(data.position.x * roomSpacingX, data.position.y * roomSpacingY, 0);
        Debug.Log(worldPos.x + " : " + worldPos.y);
        GameObject roomObj = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);

        // 문 활성화 및 비 활성화
        Room room = roomObj.GetComponent<Room>();
        room.Setup(data.doors);

        data.roomObj = room;
    }
    static bool IsDifferenceOne(int num1, int num2)
    {
        // 두 수가 1 차이나는지 절대값 검사
        return Mathf.Abs(num1 - num2) == 1;
    }

    public RoomData RoomInfo(Vector2Int location)
    {

        return rooms[location];
    }


    private void OnDrawGizmos()
    {
        // 맵 생성 순서 표시

        if (rooms == null) return;

        int index = 0;
        Gizmos.color = Color.green;
        foreach(var kvp in rooms)
        {

            Vector2Int poss = kvp.Key;
            RoomData room = kvp.Value;
            float xy = ((roomSpacingX + roomSpacingY) / 2);
            Vector3 pos = new Vector3(room.position.x * roomSpacingX, room.position.y * roomSpacingY, 0);

            if (room.doors[0])
            {
                Gizmos.DrawLine(pos, pos + Vector3.up * xy);
            }
            if (room.doors[1])
            {
                Gizmos.DrawLine(pos, pos + Vector3.down * xy);
            }
            if (room.doors[2])
            {
                Gizmos.DrawLine(pos, pos + Vector3.left * xy);
            }
            if (room.doors[3])
            {
                Gizmos.DrawLine(pos, pos + Vector3.right * xy);
            }


            #if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.5f, $"{index}");
            #endif

            index++;
        }
    }
}
