using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  private Vector3 _trajectory = Vector3.forward;

  [SerializeField]
  private float moveSpeed = 5f;

  [SerializeField]
  private float lifetime = 5f;

  private float birth;

	// Use this for initialization
	void Start () {
    birth = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
    if (Time.time - birth >= lifetime) {
      Destroy(gameObject);
      return;
    }

    transform.position = transform.position + _trajectory * moveSpeed * Time.deltaTime;
	}

  public void SetTrajectory(Vector3 trajectory) {
    _trajectory = trajectory.normalized;
  }
}
