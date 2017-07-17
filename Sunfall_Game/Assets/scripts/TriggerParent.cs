using UnityEngine;
using System.Collections;

public class TriggerParent : MonoBehaviour {

    public sargasso parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col) {
        parent.OnTriggerEnter(col);
	}

    void OnTriggerExit(Collider col)
    {
        parent.OnTriggerExit(col);
    }
}
