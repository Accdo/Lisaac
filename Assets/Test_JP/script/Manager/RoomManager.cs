using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static RoomManager instance;
    public static RoomManager Instance { get { return instance; } }

    // 몬스터가 없을 경우 true
    public bool nonMonster { get; private set; }

    private void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 자기 자신 제거
        }
    }

    // 현재 방에 몬스터가 있는지 확인
    public void CheckMonster()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("Boss") == null)
        {
            nonMonster = true; // 몬스터가 없음
        }
        else
        {
            nonMonster = false; // 몬스터가 존재함
        }
    }
}
