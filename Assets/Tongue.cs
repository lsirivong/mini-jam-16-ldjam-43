using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour {
  [SerializeField]
  private float distance = 10f;

  private LineRenderer _lineRenderer;

	// Use this for initialization
	void Start () {
    _lineRenderer = GetComponent<LineRenderer>();
    _lineRenderer.SetPosition(0, Vector3.zero);
    _lineRenderer.SetPosition(1, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void Fire() {
    _lineRenderer.SetPosition(1, distance * Vector3.forward);
    Invoke("Retract", 0.1f);
  }

  public void Retract() {
    _lineRenderer.SetPosition(1, Vector3.zero);
  }
}
