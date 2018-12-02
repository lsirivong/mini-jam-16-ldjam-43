using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
  [SerializeField]
  private GameObject enemyPrefab;

  [SerializeField]
  private int minEnemies = 3;

  [SerializeField]
  private int maxEnemies = 6;


	// Use this for initialization
	void Start () {
    int numEnemies = Random.Range(minEnemies, maxEnemies);

    for (int i = 0; i < numEnemies; i++) {
      Instantiate(enemyPrefab,
        new Vector3(
          Random.Range(-15f, 15f),
          1,
          Random.Range(-12f, 12f)
        ),
        Quaternion.Euler(Vector3.up * Random.Range(45f, 300f))
      );
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
