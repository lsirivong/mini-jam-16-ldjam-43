using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour {
  public float fireForce = 50f;

  [SerializeField]
  private LineRenderer _lineRenderer;

  [SerializeField]
  private GameObject _tip;

  [SerializeField]
  GameObject tongueParticlesGo;
  ParticleSystem _tongueParticles;

  private Rigidbody _tipRigidbody;

	// Use this for initialization
	void Start () {
    _tipRigidbody = _tip.GetComponent<Rigidbody>();
    _lineRenderer.SetPosition(0, Vector3.zero);
    _lineRenderer.SetPosition(1, Vector3.zero);
    tongueParticlesGo.SetActive(false);
    _tongueParticles = tongueParticlesGo.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
    _lineRenderer.SetPosition(1, _tip.transform.localPosition);
	}

  public void Fire(Vector3 trajectory) {
    _tipRigidbody.AddForce(fireForce * trajectory.normalized);
    tongueParticlesGo.SetActive(true);
    _tongueParticles.Stop();
    _tongueParticles.Play();
  }
}
