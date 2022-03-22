using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    // Active instance
    public static LineDrawer active;
    private List<LineRenderer> laserObjects;
    public float laserSpeed = 2f;
    public bool debug = false;
    private Transform laserTransform;

    // Get active instance
    public void Awake()
    {
        active = this;
        laserObjects = new List<LineRenderer>();
    }

    // Make lasers smaller overtime
    public void Update()
    {
        for (int i = 0; i < laserObjects.Count; i++)
        {
            if (laserObjects[i] == null)
            {
                laserObjects.RemoveAt(i);
                i--;
            }
            else if (laserObjects[i].widthMultiplier > 0f)
            {
                laserObjects[i].widthMultiplier = laserObjects[i].widthMultiplier - (Time.deltaTime * laserSpeed);
            }
        }
    }

    // Start is called before the first frame update
    public void DrawFromParent(Transform parent, Quaternion rotation, Material material, float size, float duration, float length)
    {
        // Check if debug
        if (debug) duration = 5f;

        // Create line
        GameObject newLine = new GameObject();
        newLine.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();
        laserObjects.Add(lineRenderer);

        // Set rotation
        lineRenderer.useWorldSpace = false;
        newLine.transform.SetParent(parent);
        newLine.transform.localPosition = Vector3.zero;
        newLine.transform.rotation = rotation;
        newLine.transform.eulerAngles = new Vector3(0, 0, (newLine.transform.eulerAngles.z / 2f) - 180f);

        // Set line variables
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, newLine.transform.right * length);
        lineRenderer.material = material;
        lineRenderer.startWidth = size;
        lineRenderer.endWidth = size;
        Destroy(newLine, duration);
    }
}
