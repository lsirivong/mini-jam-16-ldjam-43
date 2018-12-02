using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
  [SerializeField]
  private float moveSpeed = 20f;

  [SerializeField]
  private float maxCrosshairDist = 12f;

  private bool _posSet = false;
  private Vector3 _playerPos;

  void Awake() {
    // if more than one, then destroy selves
    Crosshair[] existing = FindObjectsOfType<Crosshair>();
    if (existing.Length > 1) {
      Destroy(gameObject);
    }
    else {
      DontDestroyOnLoad(gameObject);
    }
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  // not too happy with this, but we're coupling the crosshair
  // to the player's position on update calls. this makes crosshair
  // position sync to the player's movement
  public void UpdatePlayerPos(Vector3 playerPos) {
    if (!_posSet) {
      _playerPos = playerPos;
      _posSet = true;
      return;
    }

    Vector3 playerDiff = playerPos - _playerPos;

    transform.position = transform.position + playerDiff;

    _playerPos = playerPos;
  }

  private void HandleInput() {
    float horizontal = Input.GetAxis("RightHorizontal");
    float vertical = Input.GetAxis("RightVertical");
    Vector3 newPos = new Vector3(
      transform.localPosition.x + Time.deltaTime * horizontal * moveSpeed,
      transform.localPosition.y,
      transform.localPosition.z + Time.deltaTime * vertical * moveSpeed
    );
    Vector3 playerDiff = newPos - _playerPos;
    if (playerDiff.magnitude > maxCrosshairDist) {
      newPos = _playerPos + playerDiff.normalized * maxCrosshairDist;
    }
    transform.localPosition = newPos;
  }
}
