using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour {
  [SerializeField]
  int numOccumpants = 0;

  [SerializeField]
  GameObject occupantPrefab;

	// Use this for initialization
	void Start () {
    for (int i = 0; i < numOccumpants; i++) {
      GameObject o = Instantiate(
        occupantPrefab,
        // could be an issue if too many objects
        transform.position + transform.forward * i * 1f,
        Quaternion.identity,
        transform
      );

      print("HOSTAGE: " + o.transform.position);
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
