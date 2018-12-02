using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  [SerializeField]
  private float moveSpeed = 5f;

  [SerializeField]
  private float lifetime = 5f;

  [SerializeField]
  private int bulletDamage = 10;

  private float birth;

  private Rigidbody _rigidbody;

  private Vector3 _initialForce;

	// Use this for initialization
	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
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
    Vector3 force = moveSpeed * trajectory;
    if (_rigidbody) {
      _rigidbody.AddForce(force);
    } else {
      _initialForce = force;
    }
  }

  void OnTriggerEnter(Collider collider) {
    // print("BULLET TRIGGER : " + collider.name + ":" + collider.tag);
    if (collider.tag == "Enemy" || collider.tag == "Player") {
      // Destroy(collider);
      // hitParticles.Stop();
      // hitParticles.Play();
      collider.SendMessage("Damage", bulletDamage);
      Destroy(gameObject);
    }
  }
}
