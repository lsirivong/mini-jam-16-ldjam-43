using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
  private GameObject _player;

	// Use this for initialization
	void Start () {
    _player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
    if (_player == null) {
      return;
    }

    transform.position = new Vector3(
      Mathf.Clamp(transform.position.x, _player.transform.position.x - 12f, _player.transform.position.x + 12f),
      _player.transform.position.y + 40,
      Mathf.Clamp(transform.position.z, _player.transform.position.z - 5f - 40f, _player.transform.position.z + 5f - 40f)
    );
    // Vector3 playerPos = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
    // Vector3 cameraPos = new Vector3(transforkm.position.x, 0, transform.position.z);
    // Vector3 diff = cameraPos - playerPos;
    // float minDistance = 20f;
		// if (diff.magnitude > minDistance) {
      // transform.position = transform.position - diff.normalized * minDistance;
    // }
	}
}
