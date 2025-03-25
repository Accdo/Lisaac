using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int maxRooms = 10; // 생성할 최대 방 개수
    public GameObject roomPrefab; // 방 프리팹
    public float roomSpacingX = 18f; // X축 간격
    public float roomSpacingY = 10f; // Y축 간격

    private Dictionary<Vector2Int, RoomData> rooms = new Dictionary<Vector2Int, RoomData>(); // 생성된 방들을 저장할 딕셔너리
    private List<Vector2Int> directions = new List<Vector2Int> // 가능한 이동 방향 목록
    {
        Vector2Int.up, // 위쪽
        Vector2Int.down, // 아래쪽
        Vector2Int.left, // 왼쪽
        Vector2Int.right // 오른쪽
    };

    void Start()
    {
        GenerateDungeon(); // 던전 생성 시작
    }

    void GenerateDungeon()
    {
        rooms.Clear(); // 기존 방 데이터 초기화

        // 시작 위치 (0, 0)
        Vector2Int startPos = Vector2Int.zero;
        RoomData startRoom = new RoomData(startPos);
        rooms.Add(startPos, startRoom);

        // 방 위치 리스트 생성
        List<Vector2Int> roomPositions = new List<Vector2Int> { startPos };

        while (rooms.Count < maxRooms)
        {
            // 랜덤한 기존 방 선택
            Vector2Int currentPos = roomPositions[Random.Range(0, roomPositions.Count)];

            // 랜덤한 방향 선택 (위, 아래, 왼쪽, 오른쪽 중 하나)
            int dirRandomValue = Random.Range(0, directions.Count);
            Vector2Int randomDir = directions[dirRandomValue];
            Vector2Int newPos = currentPos + randomDir;

            // 중복 방 생성 방지
            if (rooms.ContainsKey(newPos))
            {
                continue;
            }

            // 새로운 방 생성
            RoomData newRoom = new RoomData(newPos);

            // 방 리스트에 추가
            rooms.Add(newPos, newRoom);
            roomPositions.Add(newPos);
        }

        int counting1 = 0;

        // 각 방들의 문 연결 처리
        foreach (var i in rooms)
        {
            int counting2 = 0;
            foreach (var j in rooms)
            {
                // 동일한 방이면 스킵
                if (i.Key.x == j.Key.x && i.Key.y == j.Key.y)
                {
                    counting2++;
                    continue;
                }
                else if (i.Key.x == j.Key.x && IsDifferenceOne(i.Key.y, j.Key.y))
                {
                    // 같은 X좌표에서 Y좌표가 1 차이일 경우 (위쪽 또는 아래쪽 연결)
                    if (i.Key.y < j.Key.y)
                    {
                        i.Value.doors[0] = true; // 위쪽 문 열기
                        j.Value.doors[1] = true; // 아래쪽 문 열기
                    }
                    else if (i.Key.y > j.Key.y)
                    {
                        i.Value.doors[1] = true; // 아래쪽 문 열기
                        j.Value.doors[0] = true; // 위쪽 문 열기
                    }
                    else
                    {
                        Debug.LogError($"1칸 차이 오류! : {i.Key.y} | {j.Key.y}");
                    }
                }
                else if (i.Key.y == j.Key.y && IsDifferenceOne(i.Key.x, j.Key.x))
                {
                    // 같은 Y좌표에서 X좌표가 1 차이일 경우 (왼쪽 또는 오른쪽 연결)
                    if (i.Key.x < j.Key.x)
                    {
                        i.Value.doors[3] = true; // 오른쪽 문 열기
                        j.Value.doors[2] = true; // 왼쪽 문 열기
                    }
                    else if (i.Key.x > j.Key.x)
                    {
                        i.Value.doors[2] = true; // 왼쪽 문 열기
                        j.Value.doors[3] = true; // 오른쪽 문 열기
                    }
                    else
                    {
                        Debug.LogError($"2칸 차이 오류! : {i.Key.x} | {j.Key.x}");
                    }
                }
                counting2++;
            }
            counting1++;
        }

        // 모든 방을 스폰
        foreach (var roomPair in rooms)
        {
            SpawnRoom(roomPair.Value);
        }
    }

    void SpawnRoom(RoomData data)
    {
        // 월드 좌표 변환
        Vector3 worldPos = new Vector3(data.position.x * roomSpacingX, data.position.y * roomSpacingY, 0);
        Debug.Log(worldPos.x + " : " + worldPos.y);
        GameObject roomObj = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);

        // 방 활성화 및 문 설정
        Room room = roomObj.GetComponent<Room>();
        room.Setup(data.doors);

        data.roomObj = room;
    }

    static bool IsDifferenceOne(int num1, int num2)
    {
        // 두 값의 차이가 1인지 확인
        return Mathf.Abs(num1 - num2) == 1;
    }

    public RoomData RoomInfo(Vector2Int location)
    {
        return rooms[location];
    }

    private void OnDrawGizmos()
    {
        // 던전 구조를 기즈모로 시각적으로 표시

        if (rooms == null) return;

        int index = 0;
        Gizmos.color = Color.green;
        foreach (var kvp in rooms)
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
