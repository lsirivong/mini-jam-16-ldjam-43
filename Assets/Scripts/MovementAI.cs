using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI : MonoBehaviour {
  [SerializeField]
  private bool canJump = false;

  private bool _jumpActive = true;

  [SerializeField]
  private bool _canFight = false;

  [SerializeField]
  private float patrolDelayLower = 0.8f;

  [SerializeField]
  private float patrolDelayUpper = 1.5f;

  [SerializeField]
  private float patrolDurationLower = 0.8f;

  [SerializeField]
  private float patrolDurationUpper = 1.5f;

  [SerializeField]
  private float jumpMagnitude = 10f;

  [SerializeField]
  private float jumpDelayLower = 0.5f;

  [SerializeField]
  private float jumpDelayUpper = 4.5f;

  private bool _isPatrolling = false;
  private bool _isFighting = false;

  [SerializeField]
  private float movementSpeed = 10f;

  [SerializeField]
  private float rotationSpeed = 1f;

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

	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
    _player = GameObject.FindGameObjectWithTag("Player");
    _audioSource = GetComponent<AudioSource>();
    
    hitParticles = hitParticlesGo.GetComponent<ParticleSystem>();
	}
	
	void Update () {
    if (_player == null) {
      return;
    }

    // decide what to do
    if (_canFight && !_isFighting) {
      if ((_player.transform.position - transform.position).magnitude < fightRange) {

        StartFighting();
      }
    }

    if ((!_canFight || !_isFighting) && !_isPatrolling) {
      _isPatrolling = true;
      Invoke("StartPatrol", UnityEngine.Random.Range(patrolDelayLower, patrolDelayUpper));
    }

    if (canJump && _jumpActive) {
      _jumpActive = false;
      float delay = UnityEngine.Random.Range(jumpDelayLower, jumpDelayUpper);
      Invoke("StartJump", delay);
    }

    // apply movements
    _rigidbody.AddForce(_activeForce);
    transform.Rotate(_activeRotation);
	}

  private void StartFighting() {
    // if facing player : shoot
    float angleBetween = Vector3.SignedAngle(transform.forward, _player.transform.position - transform.position, Vector3.up);
    if (Mathf.Abs(angleBetween) < 5f) {
      _isFighting = true;
      Invoke("Shoot", 0.4f);
      _activeRotation = Vector3.zero;
    } else {
      _activeRotation = (angleBetween < 0 ? -1 : 1) * rotationSpeed * Vector3.up;
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

  private void StartPatrol() {
    float rand = UnityEngine.Random.Range(0f, 1f);

    if (rand > 0.5) {
      _activeForce = movementSpeed * transform.forward;
    } else {
      _activeRotation = rotationSpeed * Vector3.up;
    }
    
    Invoke("StopPatrol", UnityEngine.Random.Range(patrolDurationLower, patrolDurationUpper));
  }

  private void StartJump() {
    float rand = UnityEngine.Random.Range(0f, 1f);

    _rigidbody.AddForce(Vector3.up * jumpMagnitude);
    _jumpActive = true;
  }

  private void StopPatrol() {
    _isPatrolling = false;
    _activeForce = Vector3.zero;
    _activeRotation = Vector3.zero;
  }
}
