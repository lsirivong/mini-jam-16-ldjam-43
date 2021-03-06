﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
  [SerializeField]
  private LineRenderer _fill;

  private GameObject mainCamera;

	// Use this for initialization
	void Start () {
    mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    // transform.LookAt(mainCamera.transform);
	}
	
	// Update is called once per frame
	void Update () {
    // transform.LookAt(mainCamera.transform);

    // orient _health bar to face camera
    transform.rotation = Quaternion.Euler(
      new Vector3(
        0,
        360f - transform.parent.rotation.y,
        0
      )
    );
	}

  // percent: 0-1
  public void SetHealth(float percent) {
    _fill.SetPosition(1, new Vector3(percent * 1.5f, 0, 0));
  }
}
