using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueTip : MonoBehaviour {
  public GameObject mace;

  private int _damage = 5;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void EnableMace() {
    mace.SetActive(true);
    _damage = 20;
  }


  void OnCollisionEnter(Collision collision) {
    // print(gameObject.name + " : collision : " + collision.collider.name + ":" + collision.collider.tag);

    switch (collision.collider.tag) {
      case "PrisonPanel":
        Prison prison = collision.collider.GetComponentInParent<Prison>();
        prison.SetFree();
        break;

      case "Enemy":
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        enemy.Damage(_damage);
        break;

      default:
        break;
    }
  }
}
