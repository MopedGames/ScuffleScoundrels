using UnityEngine;
using System.Collections;

public class IntroScreen : MonoBehaviour {

	public float loadTime;
	public int levelToLoad;

	private float loadedTime;
	private AsyncOperation asyncLoad;

	IEnumerator Start() {
		
		asyncLoad = Application.LoadLevelAsync(levelToLoad);
		asyncLoad.allowSceneActivation = false;
		yield return asyncLoad;

	}

	void Update (){
		loadedTime += Time.deltaTime;

		if(loadedTime > loadTime || Input.anyKey){
			asyncLoad.allowSceneActivation = true;
		}
	}

}
