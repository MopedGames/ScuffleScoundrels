using UnityEngine;
using System.Collections;

public class line_between : MonoBehaviour {

    public Transform[] points;
    public LineRenderer line;

	// Update is called once per frame
	void Update () {
        int i;
        for (i = 0; i < points.Length; i++) {
            line.SetPosition(i, points[i].localPosition);
        }
        
	}
}
