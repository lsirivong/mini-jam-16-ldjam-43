using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
  [SerializeField]
  int scoreHostages = 0;

  [SerializeField]
  float moveSpeed = 27f;
  
  [SerializeField]
  float lookSpeed = 60f;

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

  [SerializeField]
  private GameObject crosshairPrefab;
  GameObject crosshairObj;
  private Crosshair _crosshair;

  private float _startTime;

  // This shouldn't be an instance variable, but c'est la vie
  private string exitScene;

  void Awake() {
    // if more than one, then destroy selves
    Player[] existingPlayers = FindObjectsOfType<Player>();
    if (existingPlayers.Length > 1) {
      foreach (Player player in existingPlayers) {
        player.transform.position = transform.position;
      }
      Destroy(gameObject);
    }
    else {
      DontDestroyOnLoad(gameObject);
    }
  }

  // Use this for initialization
  void Start() {
    _startTime = Time.time;
    _tongue = _tongueObject.GetComponent<Tongue>();
    _rigidbody = GetComponent<Rigidbody>();
    _audioSource = GetComponent<AudioSource>();

    _healthBar = healthBarObj.GetComponent<HealthBar>();
    crosshairObj = Instantiate(crosshairPrefab);
    _crosshair = crosshairObj.GetComponent<Crosshair>();
    UpdateHealth(_maxHealth);
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  public void Damage(int value) {
    if (!IsDead() && value > 0) {
      int newHealth = _health - value;
      UpdateHealth(Math.Max(newHealth, 0));
    }
  }

  private void UpdateHealth(int value) {
    _health = value;
    _healthBar.SetHealth((float)_health / (float)_maxHealth);

    if (IsDead()) {
      Die();
    }
  }

  private void Die() {
    crosshairObj.SetActive(false);
    SendMessage("MakeFall");
    ShowScore();
  }

  private void ShowScore() {
    // for now just log it
    print("Time (seconds): " + (Time.time - _startTime));
    print("Frogs Saved: " + scoreHostages);
    print("Kills: " + 0);
    print("Sacrifices: " + 0);
  }

  public bool IsDead() {
    return _health <= 0;
  }

  private void HandleInput() {
    HandleDebugInput();

    if (IsDead()) {
      return;
    }

    _crosshair.UpdatePlayerPos(transform.position);

    transform.LookAt(crosshairObj.transform);
    transform.Rotate(new Vector3(0, -transform.rotation.y, 0));

    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    Vector3 moveVector = Time.deltaTime * new Vector3(
      horizontal * moveSpeed,
      0,
      vertical * moveSpeed
    );

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
        _tongueObject.SetActive(true);
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

    _tongueObject.SetActive(false);
  }

  private void HandleDebugInput() {
    if (!Debug.isDebugBuild) {
      return;
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  public void AddHostageScore(int numHostages) {
    scoreHostages = scoreHostages + numHostages;
  }

  void OnTriggerEnter(Collider collider) {
    if (collider.tag == "Finish") {
      print("FINISH");
      print(collider.gameObject);
      Exit exit = collider.gameObject.GetComponent<Exit>();
      print(exit);
      exitScene = exit.nextSceneName;
      print(exitScene);
      Invoke("LoadScene", 1f);
    }
  }

  private void LoadScene() {
    SceneManager.LoadScene(exitScene);
  }

}
