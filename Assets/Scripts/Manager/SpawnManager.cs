using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	// 싱글톤 인스턴스
	private static SpawnManager instance;
	public static SpawnManager Instance { get { return instance; } }

	// 적 프리팹 (벌레, 장기)
	public GameObject wormEnemy;
	public GameObject gutEnemy;

	private void Awake()
	{
		// 싱글톤 초기화
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
		}
		else
		{
			Destroy(gameObject); // 이미 존재하면 자기 자신 파괴
		}
	}

	// 벌레 적 스폰
	public void SpawnWorm(Vector3 position)
	{
		Instantiate(wormEnemy, position, Quaternion.identity);
	}

	// 장기 적 스폰
	public void SpawnGut(Vector3 position)
	{
		Instantiate(gutEnemy, position, Quaternion.identity);
	}
}
