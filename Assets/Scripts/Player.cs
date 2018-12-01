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
  float fireCooldown = 1f;

  [SerializeField]
  float fireThreshold = 0.5f;

  [SerializeField]
  float tongueThreshold = 0.5f;

  [SerializeField]
  float tongueCooldown = 1f;

  [SerializeField]
  AudioClip shotSfx;

  private bool _canFire = true;
  private bool _canTongue = true;

//   [SerializeField]
//   GameObject tonguePrefab;

  [SerializeField]
  private GameObject _tongueObject;

  private Tongue _tongue;

  private Rigidbody _rigidbody;
  private AudioSource _audioSource;

  // Use this for initialization
  void Start() {
    // _tongueObject = Instantiate(tonguePrefab, transform);
    _tongue = _tongueObject.GetComponent<Tongue>();
    _rigidbody = GetComponent<Rigidbody>();
    _audioSource = GetComponent<AudioSource>();
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

    float triggers = Input.GetAxis("Triggers");
    float absTriggers = Mathf.Abs(triggers);
    bool isRight = triggers < 0;
    if (absTriggers > float.Epsilon) {
      if (isRight && absTriggers > fireThreshold && _canFire) {
        // RIGHT TRIGGER
        _audioSource.PlayOneShot(shotSfx);
        _canFire = false;
        // Vector3 trajectory = (crosshair.transform.position - transform.position).normalized;
        Vector3 trajectory = transform.forward.normalized;
        Vector3 bulletOrigin = transform.position + trajectory * 2;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletOrigin, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null) {
          bulletComponent.SetTrajectory(trajectory);
        }

        Invoke("EnableFire", fireCooldown);
      } else if (!isRight && absTriggers > tongueThreshold && _canTongue) {
        // LEFT TRIGGER
        _canTongue = false;
        // Vector3 trajectory = (crosshair.transform.position - transform.position).normalized;
        Vector3 trajectory = transform.forward.normalized;
        _tongue.Fire(crosshair.transform.position - transform.position);
        Invoke("EnableTongue", tongueCooldown);
      }
    }
  }

  private void EnableFire() {
    _canFire = true;
  }

  private void EnableTongue() {
    _canTongue = true;
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
