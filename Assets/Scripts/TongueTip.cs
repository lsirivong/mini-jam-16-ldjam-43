using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


  void OnCollisionEnter(Collision collision) {
    print(gameObject.name + " : collision : " + collision.collider.name + ":" + collision.collider.tag);
    
    if (collision.collider.tag == "PrisonPanel") {
      Prison prison = collision.collider.GetComponentInParent<Prison>();
      prison.SetFree();
    }
  }
}
