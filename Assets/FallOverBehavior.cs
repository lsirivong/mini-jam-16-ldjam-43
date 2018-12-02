using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOverBehavior : MonoBehaviour {
  [SerializeField]
  private float lowerBound = 0.3f;

  [SerializeField]
  private float upperBound = 0.7f;

  private Rigidbody _rigidbody;

	void Start () {
    _rigidbody = GetComponent<Rigidbody>();
	}

  void Update() {
  }

  void MakeFall() {
    _rigidbody.freezeRotation = false;
    _rigidbody.isKinematic = false;
    // _rigidbody.AddForce(Vector3.up * 100f);
    _rigidbody.AddExplosionForce(
      UnityEngine.Random.Range(150f, 400f),
      transform.position + new Vector3(
        (UnityEngine.Random.Range(0, 1) == 1 ? 1 : -1) * UnityEngine.Random.Range(lowerBound, upperBound), 
        1f,
        (UnityEngine.Random.Range(0, 1) == 1 ? 1 : -1) * UnityEngine.Random.Range(lowerBound, upperBound)
      ),
      0.1f
    );
    return;
  }
}
