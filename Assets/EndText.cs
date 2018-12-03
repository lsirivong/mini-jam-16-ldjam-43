using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour {

  GameObject _player;

	// Use this for initialization

	void Start () {
    _player = GameObject.FindGameObjectWithTag("Player");
    Player p = _player.GetComponent<Player>();
    Text text = GetComponent<Text>();
    text.text = "YOU DIED\n\n" + p.GetScore();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
