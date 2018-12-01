using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
  [SerializeField]
  private float moveSpeed = 20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    HandleInput();
	}

  private void HandleInput() {
    float horizontal = Input.GetAxis("RightHorizontal");
    float vertical = Input.GetAxis("RightVertical");
    transform.localPosition = new Vector3(
      transform.localPosition.x + Time.deltaTime * horizontal * moveSpeed,
      transform.localPosition.y,
      transform.localPosition.z + Time.deltaTime * vertical * moveSpeed
    );
  }
}
