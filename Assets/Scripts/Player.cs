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
  GameObject crosshairObj;

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

  [SerializeField]
  private int _maxHealth = 100;

  [SerializeField]
  private GameObject _tongueObject;

  private bool _canFire = true;
  private bool _canTongue = true;

  private int _health;


  private Tongue _tongue;

  private Rigidbody _rigidbody;
  private AudioSource _audioSource;

  [SerializeField]
  private GameObject healthBarObj;
  private HealthBar _healthBar;

  private Crosshair _crosshair;

  // Use this for initialization
  void Start() {
    _tongue = _tongueObject.GetComponent<Tongue>();
    _rigidbody = GetComponent<Rigidbody>();
    _audioSource = GetComponent<AudioSource>();

    _healthBar = healthBarObj.GetComponent<HealthBar>();
    _crosshair = crosshairObj.GetComponent<Crosshair>();
    UpdateHealth(_maxHealth);
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  public void Damage(int value) {
    if (value > 0) {
      int newHealth = _health - value;
      UpdateHealth(Math.Max(newHealth, 0));
    }
  }

  private void UpdateHealth(int value) {
    _health = value;
    _healthBar.SetHealth((float)_health / (float)_maxHealth);

    if (_health <= 0) {
      Destroy(gameObject);
    }
  }

  private void HandleInput() {
    HandleDebugInput();

    _crosshair.UpdatePlayerPos(transform.position);

    transform.LookAt(crosshairObj.transform);
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
        // Vector3 trajectory = (crosshairObj.transform.position - transform.position).normalized;
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
        // Vector3 trajectory = (crosshairObj.transform.position - transform.position).normalized;
        Vector3 trajectory = transform.forward.normalized;
        _tongue.Fire(crosshairObj.transform.position - transform.position);
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
