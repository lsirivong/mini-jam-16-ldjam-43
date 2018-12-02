using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
  [SerializeField]
  private bool _isPatrolling = false;

  [SerializeField]
  private bool _isFighting = false;

  [SerializeField]
  private float movementSpeed = 10f;

  [SerializeField]
  private float rotationSpeed = 1f;

  [SerializeField]
  private int _maxHealth = 20;
  [SerializeField]
  private int _health = 20;
// 
  // [SerializeField]
  // private float bulletDamage = 0.25f;

  [SerializeField]
  private GameObject healthBarObj;
  private HealthBar _healthBar;

  [SerializeField]
  private GameObject hitParticlesGo;
  private ParticleSystem hitParticles;

  [SerializeField]
  private GameObject bulletPrefab;

  [SerializeField]
  private AudioClip shotSfx;

  [SerializeField]
  private AudioClip[] hitSfx;

  [SerializeField]
  private float fightRange = 15f;

  private Rigidbody _rigidbody;
  private AudioSource _audioSource;

  private Vector3 _activeForce = Vector3.zero;
  private Vector3 _activeRotation = Vector3.zero;

  private GameObject _player;

  private bool _didDeathAnimation = false;

	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
    _player = GameObject.FindGameObjectWithTag("Player");
    _audioSource = GetComponent<AudioSource>();
    
    hitParticles = hitParticlesGo.GetComponent<ParticleSystem>();

    _healthBar = healthBarObj.GetComponent<HealthBar>();
    UpdateHealth(_maxHealth);
	}
	
	void Update () {
    if (_health <= 0) {
      if (!_didDeathAnimation) {
        Destroy(healthBarObj);
        _rigidbody.freezeRotation = false;
        // _rigidbody.AddForce(Vector3.up * 100f);
        _rigidbody.AddExplosionForce(
          UnityEngine.Random.Range(150f, 400f),
          transform.position + new Vector3(
            (UnityEngine.Random.Range(0, 1) == 1 ? 1 : -1) * UnityEngine.Random.Range(0.3f, 0.7f), 
            1f,
            (UnityEngine.Random.Range(0, 1) == 1 ? 1 : -1) * UnityEngine.Random.Range(0.3f, 0.7f)
          ),
          0.1f
        );
      }
      return;
    }

    // decide what to do
    if (!_isFighting) {
      if ((_player.transform.position - transform.position).magnitude < fightRange) {
        // print(gameObject.name + " : close enough to fight!");

        StartFighting();
      }
    }

    if (!_isFighting && !_isPatrolling) {
      _isPatrolling = true;
      Invoke("StartPatrol", UnityEngine.Random.Range(0.8f, 1.8f));
    }

    // apply movements
    _rigidbody.AddForce(_activeForce);
    transform.Rotate(_activeRotation);
	}

  private void StartFighting() {
    // if facing player : shoot
    float angleBetween = Vector3.SignedAngle(transform.forward, _player.transform.position - transform.position, Vector3.up);
    // print("angleBetween : " + angleBetween);
    if (Mathf.Abs(angleBetween) < 5f) {
      _isFighting = true;
      Invoke("Shoot", 0.4f);
      _activeRotation = Vector3.zero;
    } else {
      _activeRotation = (angleBetween < 0 ? -1 : 1) * rotationSpeed * Vector3.up;
      // print("activeRotation : " + _activeRotation);
    }
    // otherwise, try turning toward them first
  }

  private void Shoot() {
    _isFighting = false;
    _audioSource.PlayOneShot(shotSfx);
    Vector3 trajectory = transform.forward.normalized;
    Vector3 bulletOrigin = transform.position + trajectory * 2;
    GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletOrigin, Quaternion.identity);
    Bullet bulletComponent = bullet.GetComponent<Bullet>();
    if (bulletComponent != null) {
      bulletComponent.SetTrajectory(trajectory);
    }
  }

  public void Damage(int value) {
    if (value > 0) {
      hitParticles.Stop();
      hitParticles.Play();

      if (_health > 0) {
        int sfxIndex = UnityEngine.Random.Range(0, hitSfx.Length);
        _audioSource.PlayOneShot(hitSfx[sfxIndex]);

        int newHealth = _health - value;
        UpdateHealth(Math.Max(newHealth, 0));
      }
    }
  }

  private void UpdateHealth(int value) {
    _health = value;
    _healthBar.SetHealth((float)_health / (float)_maxHealth);

    if (_health <= 0) {
      // Invoke("SelfDestroy", 3f);
    }
  }

  private void SelfDestroy() {
    Destroy(gameObject);
  }

  private void StartPatrol() {
    float rand = UnityEngine.Random.Range(0f, 1f);

    if (rand > 0.5) {
      _activeForce = movementSpeed * transform.forward;
    } else {
      _activeRotation = rotationSpeed * Vector3.up;
    }
    
    Invoke("StopPatrol", UnityEngine.Random.Range(0.8f, 1.5f));
  }

  private void StopPatrol() {
    _isPatrolling = false;
    _activeForce = Vector3.zero;
    _activeRotation = Vector3.zero;
  }
}
