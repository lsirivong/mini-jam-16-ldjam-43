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

  [SerializeField]
  private float mouseYOffset = 2f;

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
    Cursor.visible = false;
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

  private float _mouseX;
  private float _mouseY;
  private bool useMouse = false;

  private void HandleInput() {
    // print(Input.mousePosition);
    // bool useMouse = true;
    Vector3 newPos = transform.localPosition;

    float horizontal = Input.GetAxis("RightHorizontal");
    float vertical = Input.GetAxis("RightVertical");

    float newMouseX = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
    float newMouseY = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);

    if (horizontal > float.Epsilon || vertical > float.Epsilon) {
      useMouse = false;
    }

    if (newMouseX != _mouseX || newMouseY != _mouseY) {
      useMouse = true;

      _mouseX = newMouseX;
      _mouseY = newMouseY;
    }


    if (!useMouse) {
      newPos = new Vector3(
        transform.localPosition.x + Time.deltaTime * horizontal * moveSpeed,
        transform.localPosition.y,
        transform.localPosition.z + Time.deltaTime * vertical * moveSpeed
      );
    } else {
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(_mouseX, _mouseY, 0));
      RaycastHit hitInfo;
      int layerMask = 1 << 9;
      if (Physics.Raycast(ray, out hitInfo, 1000f, layerMask)) {
        Ray offset = new Ray(hitInfo.point, -ray.direction);
        Vector3 offsetPoint = offset.GetPoint(mouseYOffset);
        newPos = new Vector3(
          offsetPoint.x,
          transform.localPosition.y,
          offsetPoint.z
        );
      }
    }

    Vector3 playerDiff = newPos - _playerPos;
    if (playerDiff.magnitude > maxCrosshairDist) {
      newPos = _playerPos + playerDiff.normalized * maxCrosshairDist;
    }
    transform.localPosition = newPos;
  }
}
