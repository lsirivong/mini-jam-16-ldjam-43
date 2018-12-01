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
  private float _health = 1f;

  [SerializeField]
  private float bulletDamage = 0.25f;

  [SerializeField]
  private float tongueDamage = 0.1f;

  [SerializeField]
  private GameObject healthBarObj;
  private HealthBar _healthBar;

  [SerializeField]
  private GameObject hitParticlesGo;
  private ParticleSystem hitParticles;

  private Rigidbody _rigidbody;

  private Vector3 _activeForce = Vector3.zero;
  private Vector3 _activeRotation = Vector3.zero;

	// Use this for initialization
	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
    hitParticles = hitParticlesGo.GetComponent<ParticleSystem>();

    _healthBar = healthBarObj.GetComponent<HealthBar>();
    UpdateHealth(1f);
	}
	
	// Update is called once per frame
	void Update () {
    if (!_isFighting && !_isPatrolling) {
      _isPatrolling = true;
      Invoke("StartPatrol", Random.Range(0.8f, 1.8f));
    }

    _rigidbody.AddForce(_activeForce);

    transform.Rotate(_activeRotation);

    // orient _health bar to face camera
    healthBarObj.transform.rotation = Quaternion.Euler(
      new Vector3(
        0,
        360f - transform.rotation.y,
        0
      )
    );
	}

  void OnCollisionEnter(Collision collision) {
    print(gameObject.name + " : collision : " + collision.collider.gameObject.name + ":" + collision.collider.gameObject.tag);
    float damage = 0f;
    // if (collision.collider.gameObject.tag == "Bullet") {
      // damage = bulletDamage;
    // }
    if (collision.collider.gameObject.tag == "Tongue") {
      damage = tongueDamage;
    }

    if (damage > Mathf.Epsilon) {
      float newHealth = _health - damage;
      UpdateHealth(Mathf.Clamp(newHealth, 0f, 1f));
    }
  }

  void OnTriggerEnter(Collider collider) {
    // print(gameObject.name + " : collision : " + collider.gameObject.name + ":" + collider.gameObject.tag);
    float damage = 0f;
    if (collider.gameObject.tag == "Bullet") {
      damage = bulletDamage;
      Destroy(collider.gameObject);
      hitParticles.Stop();
      hitParticles.Play();
    }

    // if (collider.gameObject.tag == "Tongue") {
    // damage = tongueDamage;
    // }

    if (damage > Mathf.Epsilon) {
      float newHealth = _health - damage;
      UpdateHealth(Mathf.Clamp(newHealth, 0f, 1f));
    }
  }

  private void UpdateHealth(float value) {
    // print(gameObject.name + " : UPDATEING HALTH : " + value);
    _health = value;
    _healthBar.SetHealth(_health);

    if (_health <= 0) {
      Destroy(gameObject);
    }
  }

  private void StartPatrol() {
    float rand = Random.Range(0f, 1f);

    if (rand > 0.5) {
      // _activeForce = movementSpeed * (new Vector3(
        // UnityEngine.Random.Range(-1f, 1f),
        // 0,
        // UnityEngine.Random.Range(-1f, 1f)
      // )).normalized;

      _activeForce = movementSpeed * transform.forward;
    } else {
      _activeRotation = rotationSpeed * Vector3.up;
    }
    

    Invoke("StopPatrol", Random.Range(0.8f, 1.5f));
  }

  private void StopPatrol() {
    _isPatrolling = false;
    _activeForce = Vector3.zero;
    _activeRotation = Vector3.zero;
  }
}
