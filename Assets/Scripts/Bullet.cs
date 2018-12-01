using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  [SerializeField]
  private float moveSpeed = 5f;

  [SerializeField]
  private float lifetime = 5f;

  private float birth;

  private Rigidbody _rigidbody;

  private Vector3 _initialForce;

	// Use this for initialization
	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
    print("BULLET START");
    birth = Time.time;
    if (_initialForce != null) {
      _rigidbody.AddForce(_initialForce);
    }
	}
	
	// Update is called once per frame
	void Update () {
    if (Time.time - birth > lifetime) {
      Destroy(gameObject);
    }
	}

  public void SetTrajectory(Vector3 trajectory) {
    print("BULLET SET TRAJECTORY");
    Vector3 force = moveSpeed * trajectory;
    if (_rigidbody) {
      _rigidbody.AddForce(force);
    } else {
      _initialForce = force;
    }
  }
}
