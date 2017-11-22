using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetController : MonoBehaviour {

    public Renderer materialRenderer;
    public string textureName;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        materialRenderer.material.SetTextureOffset(textureName, new Vector2(transform.localPosition.x, transform.localPosition.y)*10);
	}
}
