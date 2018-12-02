using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour {
  [SerializeField]
  int numOccupants = 3;

  [SerializeField]
  GameObject occupantPrefab;

  List<GameObject> _panels = new List<GameObject>();
  GameObject _player;

	// Use this for initialization
	void Start () {
    _player = GameObject.FindGameObjectWithTag("Player");

    for (int i = 0; i < numOccupants; i++) {
      GameObject o = Instantiate(
        occupantPrefab,
        // could be an issue if too many objects
        transform.position + transform.forward * i * 1f,
        Quaternion.identity,
        transform
      );
    }

    Transform[] children = GetComponentsInChildren<Transform>();

    foreach (Transform child in children) {
      if (child.gameObject.tag == "PrisonPanel") {
        _panels.Add(child.gameObject);
      }
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void SetFree() {
    foreach (GameObject panel in _panels) {
      panel.SendMessage("MakeFall");
    }

    _player.SendMessage("AddHostageScore", numOccupants);
  }
}
