using UnityEngine;
using System.Collections;

[System.Serializable]
public class UvAnimationCurves {

	public AnimationCurve uCurve;
	public AnimationCurve vCurve;

}



public class animateUv : MonoBehaviour {

	public Material material;
	public string textureName = "_MainTex";

	public Vector2 animationSpeed;
	public UvAnimationCurves animationCurves;

	private Renderer renderer;
	private float timer;

	public void Awake (){

        timer = 0f;

		if (material == null) {
			renderer = GetComponent<Renderer> ();
			material = renderer.material;
		}


	}

	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		Vector2 offset = new Vector2(animationCurves.uCurve.Evaluate(timer*animationSpeed.x),
									animationCurves.vCurve.Evaluate(timer*animationSpeed.y));
		material.SetTextureOffset (textureName, offset);
	}
}
