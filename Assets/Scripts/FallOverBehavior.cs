using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOverBehavior : MonoBehaviour {
  [SerializeField]
  private float distanceLowerBound = 0.3f;

  [SerializeField]
  private float distanceUpperBound = 0.7f;

  [SerializeField]
  private float radiusLowerBound = 0.3f;

  [SerializeField]
  private float radiusUpperBound = 0.7f;

  [SerializeField]
  private float lowerBound = 150f;

  [SerializeField]
  private float upperBound = 400f;

  private Rigidbody _rigidbody;

	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
	}

  void Update() {
  }

  void MakeFall() {
    _rigidbody.freezeRotation = false;
    _rigidbody.isKinematic = false;
    _rigidbody.AddExplosionForce(
      UnityEngine.Random.Range(lowerBound, upperBound),
      transform.position + new Vector3(
        (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(distanceLowerBound, distanceUpperBound), 
        1f,
        (UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1) * UnityEngine.Random.Range(distanceLowerBound, distanceUpperBound)
      ),
      UnityEngine.Random.Range(radiusLowerBound, radiusUpperBound)
    );
    return;
  }
}
