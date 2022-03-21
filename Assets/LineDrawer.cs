using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    // Active instance
    public static LineDrawer active;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // Start is called before the first frame update
    public void DrawFromParent(Transform parent, Quaternion rotation, Material material, float size, float duration, float length)
    {
        // Create line
        GameObject myLine = new GameObject();
        myLine.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = myLine.GetComponent<LineRenderer>();

        // Set rotation
        lineRenderer.useWorldSpace = false;
        myLine.transform.SetParent(parent);
        myLine.transform.localPosition = Vector3.zero;
        myLine.transform.rotation = rotation;
        myLine.transform.eulerAngles = new Vector3(0, 0, (myLine.transform.eulerAngles.z / 2f) - 180f);

        // Set line variables
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, myLine.transform.right * length);
        lineRenderer.material = material;
        lineRenderer.startWidth = size;
        lineRenderer.endWidth = size;
        Destroy(myLine, duration);
    }
}
