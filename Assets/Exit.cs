using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Upgrade {
  None,
  Revolver,
  Mace,
  Speed
}

public class Exit : MonoBehaviour {
  public string nextSceneName;
  public bool resetPlayer = false;

  public Upgrade upgrade;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
