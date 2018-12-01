using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
  [SerializeField]
  float moveSpeed = 27f;
  
  [SerializeField]
  float lookSpeed = 60f;

  [SerializeField]
  GameObject crosshair;

  [SerializeField]
  GameObject bulletPrefab;

//   [SerializeField]
//   GameObject tonguePrefab;

  [SerializeField]
  private GameObject _tongueObject;

  private Tongue _tongue;

  private Rigidbody _rigidbody;

  // Use this for initialization
  void Start() {
    // _tongueObject = Instantiate(tonguePrefab, transform);
    _tongue = _tongueObject.GetComponent<Tongue>();
    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  private void HandleInput() {
    HandleDebugInput();

    transform.LookAt(crosshair.transform);
    transform.Rotate(new Vector3(0, -transform.rotation.y, 0));
    // transform.rotation

    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    Vector3 moveVector = Time.deltaTime * new Vector3(
      horizontal * moveSpeed,
      0,
      vertical * moveSpeed
    );

    // transform.position = transform.position + moveVector;

    _rigidbody.AddForce(moveVector);

    if (Input.GetButtonDown("Fire1")) {
      Vector3 trajectory = crosshair.transform.position - transform.position;
      Vector3 bulletOrigin = transform.position + trajectory.normalized * 2;
      GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletOrigin, Quaternion.identity);
      Bullet bulletComponent = bullet.GetComponent<Bullet>();
      if (bulletComponent != null) {
        bulletComponent.SetTrajectory(trajectory);
      }
    }

    if (Input.GetButtonDown("Fire2")) {
      Vector3 trajectory = crosshair.transform.position - transform.position;
      _tongue.Fire(crosshair.transform.position - transform.position);
    }
  }

  private void HandleDebugInput() {
    if (!Debug.isDebugBuild) {
      return;
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      print("RELOAD");
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }
}
