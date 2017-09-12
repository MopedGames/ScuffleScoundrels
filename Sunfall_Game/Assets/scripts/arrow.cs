using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour
{

    public Ship targetShip;
    public Vector4 limits;
    public SpriteRenderer renderer;

    public AnimationCurve popupAnimation;

    private float size = 0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targetShip != null)
        {

            Vector3 position = new Vector3(targetShip.transform.position.x, transform.position.y, targetShip.transform.position.z);

            transform.LookAt(position);
            transform.position = new Vector3(Mathf.Clamp(position.x, limits.y, limits.x), position.y, Mathf.Clamp(position.z, limits.w, limits.z));
            if (!targetShip.alive)
            {
                renderer.enabled = false;
            }
            else
            {

                if (!renderer.enabled && (position.x > limits.x || position.x < limits.y || position.z > limits.z || position.z < limits.w))
                {
                    renderer.enabled = true;

                }
                else if (renderer.enabled && (position.x < limits.x && position.x > limits.y && position.z < limits.z && position.z > limits.w))
                {
                    renderer.enabled = false;

                }
            }



            Vector2 overlap = new Vector2(Mathf.Abs(position.x) - limits.x, Mathf.Abs(position.z) - limits.z) * 2f;
            overlap = new Vector2(Mathf.Clamp01(overlap.x), Mathf.Clamp01(overlap.y));
            //Debug.Log (overlap);
            size = popupAnimation.Evaluate(overlap.x + overlap.y) * 10f; // Mathf.Lerp(0f,10f,overlap.x+overlap.y);
            transform.localScale = new Vector3(size, size, size);

        }
        else
        {
            renderer.enabled = false;
        }
    }
}
