using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Upgrade {
  None,
  Revolver,
  Mace,
  Shield
}

public class Exit : MonoBehaviour {
  public string nextSceneName;

  public Upgrade upgrade;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
