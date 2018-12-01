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
  private GameObject healthBarObj;

  private Rigidbody _rigidbody;

  private Vector3 _activeForce = Vector3.zero;
  private Vector3 _activeRotation = Vector3.zero;

	// Use this for initialization
	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
    HealthBar healthBar = healthBarObj.GetComponent<HealthBar>();
    float health = Random.Range(0f, 1f);
    print(gameObject.name + " : Health : " + health);
    healthBar.SetHealth(health);
	}
	
	// Update is called once per frame
	void Update () {
    if (!_isFighting && !_isPatrolling) {
      _isPatrolling = true;
      Invoke("StartPatrol", Random.Range(0.8f, 1.8f));
    }

    _rigidbody.AddForce(_activeForce);

    transform.Rotate(_activeRotation);

    // orient health bar to face camera
    healthBarObj.transform.rotation = Quaternion.Euler(
      new Vector3(
        0,
        360f - transform.rotation.y,
        0
      )
    );
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
