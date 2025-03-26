using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static RoomType;

public class DungeonGenerator : MonoBehaviour
{
    // �ִ� �� ����
    public int maxRooms = 10;
    // ���� �� ������
    public GameObject startRoomPrefabs;
    // �Ϲ� �� ������
    public GameObject[] normalRoomPrefabs;
    // ������ �� ������
    public GameObject[] itemRoomPrefabs;
    // ���� �� ������
    public GameObject[] bossRoomPrefabs;
    // �� ���� ����
    public float roomSpacingX = 18f;
    public float roomSpacingY = 10f;

    private bool isMapCreateDone = false;

    private Dictionary<Vector2Int, RoomData> rooms = new Dictionary<Vector2Int, RoomData>();
    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up, // 위쪽
        Vector2Int.down, // 아래쪽
        Vector2Int.left, // 왼쪽
        Vector2Int.right // 오른쪽
    };
    private List<Vector2Int> endRoomList = new List<Vector2Int>();

    void Start()
    {
        // �� ���� ���� (����� 2�� ������ ��� �ٽ� ����)
        while(!isMapCreateDone)
        {
            GenerateDungeon();
        }
    }

    void GenerateDungeon()
    {
        rooms.Clear(); // 기존 방 데이터 초기화

        // 시작 위치 (0, 0)
        Vector2Int startPos = Vector2Int.zero;
        RoomData startRoom = new RoomData(startPos);
        startRoom.roomType = RoomType.RoomTypeEnum.Start;
        rooms.Add(startPos, startRoom);

        // 방 위치 리스트 생성
        List<Vector2Int> roomPositions = new List<Vector2Int> { startPos };

        int tryCount = 0;
        bool failTrigger = false;
        while (rooms.Count < maxRooms)
        {
            // �õ� Ƚ���� �ʹ� ������ ĵ��
            tryCount++;
            if (tryCount > maxRooms * 5)
            {
                failTrigger = true;
                break;
            }

            // �������� ���� �� �ϳ� ����
            Vector2Int currentPos = roomPositions[Random.Range(0, roomPositions.Count)];

            // ���� ���� ���� (up down left right)
            int dirRandomValue = Random.Range(0, 4);
            Vector2Int randomDir = directions[dirRandomValue];
            Vector2Int newPos = currentPos + randomDir;

            // 중복 방 생성 방지
            if (rooms.ContainsKey(newPos))
            {
                continue;
            }

            // ������ ���⿡ ������ �� ���� Ȯ��
            int nearRoomCount = 0;
            nearRoomCount = rooms.ContainsKey(newPos + Vector2Int.up) ? nearRoomCount + 1 : nearRoomCount;
            nearRoomCount = rooms.ContainsKey(newPos + Vector2Int.down) ? nearRoomCount + 1 : nearRoomCount;
            nearRoomCount = rooms.ContainsKey(newPos + Vector2Int.left) ? nearRoomCount + 1 : nearRoomCount;
            nearRoomCount = rooms.ContainsKey(newPos + Vector2Int.right) ? nearRoomCount + 1 : nearRoomCount;

            if (nearRoomCount >= 3)
            {
                continue;
            }

            // �ڳ�(����) ���� ������ ���, �밢���� ���� �ִ��� üũ�Ͽ� �� ���� ����
            bool hasLeft = rooms.ContainsKey(newPos + Vector2Int.left);
            bool hasRight = rooms.ContainsKey(newPos + Vector2Int.right);
            bool hasUp = rooms.ContainsKey(newPos + Vector2Int.up);
            bool hasDown = rooms.ContainsKey(newPos + Vector2Int.down);

            // ��-��(��) �ڳ� ���̸� �밢��(���� �밢 ��)�� ���� ������ ���� X
            if (hasLeft && hasUp && rooms.ContainsKey(newPos + Vector2Int.left + Vector2Int.up))
            {
                continue;
            }

            // ��-��(��) �ڳ� ���̸� �밢��(���� �밢 ��)�� ���� ������ ���� X
            if (hasRight && hasUp && rooms.ContainsKey(newPos + Vector2Int.right + Vector2Int.up))
            {
                continue;
            }

            // ��-�Ʒ�(��) �ڳ� ���̸� �밢��(�¾Ʒ� �밢 ��)�� ���� ������ ���� X
            if (hasLeft && hasDown && rooms.ContainsKey(newPos + Vector2Int.left + Vector2Int.down))
            {
                continue;
            }

            // ��-�Ʒ�(��) �ڳ� ���̸� �밢��(��Ʒ� �밢 ��)�� ���� ������ ���� X
            if (hasRight && hasDown && rooms.ContainsKey(newPos + Vector2Int.right + Vector2Int.down))
            {
                continue;
            }

            // ���ο� �� ����
            RoomData newRoom = new RoomData(newPos);
            newRoom.roomType = RoomType.RoomTypeEnum.Normal;

            // �� ����
            switch (dirRandomValue)
            {
                case 0:
                    newRoom.doors[1] = true;
                    rooms[currentPos].doors[0] = true;
                    break;
                case 1:
                    newRoom.doors[0] = true;
                    rooms[currentPos].doors[1] = true;
                    break;
                case 2:
                    newRoom.doors[3] = true;
                    rooms[currentPos].doors[2] = true;
                    break;
                case 3:
                    newRoom.doors[2] = true;
                    rooms[currentPos].doors[3] = true;
                    break;
            }

            // 방 리스트에 추가
            rooms.Add(newPos, newRoom);
            roomPositions.Add(newPos);
        }

        if (failTrigger)
        {
            return;
        }

        // int counting1 = 0;

        // �� ����
        foreach (var i in rooms)
        {
            // int counting2 = 0;
            foreach (var j in rooms)
            {
                // Debug.Log(counting1 + " : " + counting2 + "    |    " + i.Key.x + " : " + i.Key.y + "    |    " + j.Key.x + " : " + j.Key.y);

                // i�� j�� ���� ������ �˻�
                if (i.Key.x == j.Key.x && i.Key.y == j.Key.y)
                {
                    // counting2++;
                    continue;
                }
                else if (i.Key.x == j.Key.x && IsDifferenceOne(i.Key.y, j.Key.y))
                {
                    // X ��ǥ�� ���� Y��ǥ�� 1 ���̳��� (�� �Ʒ� or �Ʒ� �� ������)
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
                // counting2++;

            }
            // counting1++;
        }

        // ����� ����
        EndRoomSelect();

        if (endRoomList.Count < 2)
        {
            // ���ٸ� �� 2�� ������ ��� �ٽ� ����
            return;
        }

        //������ �� �����۹� ����
        BossRoomAndItemRoomSelect();

        /*foreach (var room in rooms)
        {
            Debug.Log("RoomList : " + room.Value.roomType);
        }*/

        foreach (var roomPair in rooms)
        {
            SpawnRoom(roomPair.Value);
        } 

        isMapCreateDone = true;
    }

    void SpawnRoom(RoomData data)
    {
        // �� ���� ����
        Vector3 worldPos = new Vector3(data.position.x * roomSpacingX, data.position.y * roomSpacingY, 0);

        // �� Ÿ�Կ� ���� ������ ����
        GameObject roomObj = null;
        switch(data.roomType)
        {
            case RoomTypeEnum.Start:
                roomObj = Instantiate(startRoomPrefabs, worldPos, Quaternion.identity, transform);
                break;
            case RoomTypeEnum.Normal:
                roomObj = Instantiate(normalRoomPrefabs[Random.Range(0, normalRoomPrefabs.Length)], worldPos, Quaternion.identity, transform);
                break;
            case RoomTypeEnum.Item:
                roomObj = Instantiate(normalRoomPrefabs[Random.Range(0, itemRoomPrefabs.Length)], worldPos, Quaternion.identity, transform);
                break;
            case RoomTypeEnum.Boss:
                roomObj = Instantiate(normalRoomPrefabs[Random.Range(0, bossRoomPrefabs.Length)], worldPos, Quaternion.identity, transform);
                break;
        }
        Vector2Int tempVec = Vector2Int.zero;

        // 방 활성화 및 문 설정
        Room room = roomObj.GetComponent<Room>();
        room.Setup(data.doors, data.roomType);

        // �ֺ� �� Ư�� ���� ��� �� ����
        for (int i = 0; i < data.doors.Length; i++)
        {
            if (data.doors[i])
            {
                switch (i)
                {
                    case 0:
                        tempVec = new Vector2Int(data.position.x, data.position.y + 1);
                        break;
                    case 1:
                        tempVec = new Vector2Int(data.position.x, data.position.y - 1);
                        break;
                    case 2:
                        tempVec = new Vector2Int(data.position.x - 1, data.position.y);
                        break;
                    case 3:
                        tempVec = new Vector2Int(data.position.x + 1, data.position.y);
                        break;
                }
                if (rooms[tempVec].roomType != RoomTypeEnum.Normal && rooms[tempVec].roomType != RoomTypeEnum.Start)
                {
                    room.DoorChange(i, rooms[tempVec].roomType);
                }
            }

        }

        data.roomObj = room;
    }
    bool IsDifferenceOne(int num1, int num2)
    {
        // 두 값의 차이가 1인지 확인
        return Mathf.Abs(num1 - num2) == 1;
    }

    int AddAbsoluteValue(int num1, int num2)
    {
        // �� ���� ���밪 ���ϱ�
        return Mathf.Abs(num1) + Mathf.Abs(num2);
    }

    public RoomData RoomInfo(Vector2Int location)
    {
        return rooms[location];
    }

    void EndRoomSelect()
    {
        endRoomList.Clear();
        Debug.Log("����� ����");
        foreach (var room in rooms)
        {
            int doorCount = 0;
            for(int i = 0; i < room.Value.doors.Length; i++)
            {
                if (room.Value.doors[i])
                {
                    doorCount++;
                }
            }
            if(doorCount == 1)
            {
                endRoomList.Add(room.Key);
            }
        }
        foreach(var endRoom in endRoomList)
        {
            Debug.Log("End Room : " + endRoom);
        }
    }

    void BossRoomAndItemRoomSelect()
    {
        Vector2Int farRoom = new Vector2Int(0,0);
        foreach (var endRoom in endRoomList)
        {
            Debug.Log(endRoom);
            Debug.Log(AddAbsoluteValue(endRoom.x, endRoom.y));
            if(AddAbsoluteValue(farRoom.x, farRoom.y) < AddAbsoluteValue(endRoom.x, endRoom.y))
            {
                farRoom.x = endRoom.x;
                farRoom.y = endRoom.y;
            }
            else if(AddAbsoluteValue(farRoom.x, farRoom.y) == AddAbsoluteValue(endRoom.x, endRoom.y))
            {
                if(Mathf.Abs(farRoom.x) < Mathf.Abs(endRoom.x))
                {
                    farRoom.x = endRoom.x;
                    farRoom.y = endRoom.y;
                }
            }
        }
        Debug.Log("Far Room : " + farRoom);
        rooms[farRoom].roomType = RoomType.RoomTypeEnum.Boss;
        endRoomList.Remove(farRoom);

        int randItemRoom = Random.Range(0, endRoomList.Count);
        rooms[endRoomList[randItemRoom]].roomType = RoomType.RoomTypeEnum.Item;
    }


    private void OnDrawGizmos()
    {
        // 던전 구조를 기즈모로 시각적으로 표시

        if (rooms == null) return;

        int index = 0;
        foreach(var kvp in rooms)
        {
            Vector2Int poss = kvp.Key;
            RoomData room = kvp.Value;
            float xy = ((roomSpacingX + roomSpacingY) / 2);
            Vector3 pos = new Vector3(room.position.x * roomSpacingX, room.position.y * roomSpacingY, 0);

            #if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(pos + Vector3.up * 0.5f, $"{index}");
            #endif

            index++;
        }
    }
}
