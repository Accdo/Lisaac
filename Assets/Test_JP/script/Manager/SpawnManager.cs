using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    public static SpawnManager Instance { get { return instance; } }

	public GameObject wormEnemy;
	public GameObject gutEnemy;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SpawnWorm(Vector3 position)
	{
		Instantiate(wormEnemy, position, Quaternion.identity);
	}

	public void SpawnGut(Vector3 position)
	{
		Instantiate(gutEnemy, position, Quaternion.identity);;
	}
}
