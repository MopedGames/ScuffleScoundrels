using UnityEngine;
using System.Collections;

public class setSprite : MonoBehaviour {

	public Sprite sprite;
	private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		renderer.sprite = sprite;
	}
}
