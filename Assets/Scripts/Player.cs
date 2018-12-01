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

  [SerializeField]
  GameObject tonguePrefab;

  private GameObject _tongueObject;
  private Tongue _tongue;

  // Use this for initialization
  void Start() {
    _tongueObject = Instantiate(tonguePrefab, transform);
    _tongue = _tongueObject.GetComponent<Tongue>();
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  private void HandleInput() {
    HandleDebugInput();

//     float rhorizontal = Input.GetAxis("RightHorizontal");
//     float rvertical = Input.GetAxis("RightVertical");
//     transform.Rotate(new Vector3(
//       0,
//       Time.deltaTime * rhorizontal * lookSpeed,
//       0
//     ));

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

    // todo: rotate moveVector in direction of camera
    transform.position = transform.position + moveVector;

    if (Input.GetButtonDown("Fire1")) {
      GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
      Bullet bulletComponent = bullet.GetComponent<Bullet>();
      if (bulletComponent != null) {
        bulletComponent.SetTrajectory(crosshair.transform.position - transform.position);
      }
    }

    if (Input.GetButtonDown("Fire2")) {
      _tongue.Fire();
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
