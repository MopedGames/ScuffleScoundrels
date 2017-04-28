using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float timeToDestroy;
	public float fadeTime = 0f;

	// Use this for initialization
	void Awake () {
		StartCoroutine ("Timer");
	}
	

	IEnumerator Timer () {
		yield return new WaitForSeconds (timeToDestroy);
		if (fadeTime > 0.01f) {
			Renderer[] renderers = GetComponentsInChildren<Renderer> ();

			//Beep beep
			for(int i = 0; i < 3; ++i){
				yield return new WaitForSeconds (fadeTime/8f);
				foreach (Renderer r in renderers) {
					r.enabled = false;
				}
				yield return new WaitForSeconds (fadeTime/8f);
				foreach (Renderer r in renderers) {
					r.enabled = true;
				}
			}
			//Bipbipbipbip
			for(int i = 0; i < 5; ++i){
				yield return new WaitForSeconds (fadeTime/16f);
				foreach (Renderer r in renderers) {
					r.enabled = false;
				}
				yield return new WaitForSeconds (fadeTime/16f);
				foreach (Renderer r in renderers) {
					r.enabled = true;
				}
			}

		}

		Destroy (gameObject);
	}
}



