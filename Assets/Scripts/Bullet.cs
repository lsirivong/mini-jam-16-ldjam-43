using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  private Vector3 _trajectory = Vector3.forward;

  [SerializeField]
  private float moveSpeed = 5f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    transform.position = transform.position + _trajectory * moveSpeed * Time.deltaTime;
	}

  public void SetTrajectory(Vector3 trajectory) {
    _trajectory = trajectory.normalized;
  }
}
