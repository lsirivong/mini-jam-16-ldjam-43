using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : MonoBehaviour {
  [SerializeField]
  int numOccumpants = 0;

  [SerializeField]
  GameObject occupantPrefab;

  List<GameObject> _panels = new List<GameObject>();

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
    print("TODO : set hostages free");
    foreach (GameObject panel in _panels) {
      panel.SendMessage("MakeFall");
    }

    // gameObject.FindGameObjectsWithTag("PrisonPanel");
  }
}
