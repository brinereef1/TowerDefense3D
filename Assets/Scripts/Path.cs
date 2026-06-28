using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    // Ordered list of waypoints enemies will follow.
    public GameObject[] WayPoints;

    public Vector3 GetWayPoints(int index)
    {
        // Return the position of the requested waypoint.
        return WayPoints[index].transform.position;
    }

    private void OnDrawGizmos()
    {
        if (WayPoints.Length > 0)
        {
            for (int i = 0; i < WayPoints.Length; i++)
            {
                // Display each waypoint's name in the Scene view.
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.blue;
                style.alignment = TextAnchor.MiddleLeft;

                Handles.Label(
                    WayPoints[i].transform.position + Vector3.up * 0.7f,
                    WayPoints[i].name,
                    style);

                // Draw a line between consecutive waypoints to visualize the path.
                if (i < WayPoints.Length - 1)
                {
                    Gizmos.color = Color.black;

                    Gizmos.DrawLine(
                        WayPoints[i].transform.position,
                        WayPoints[i + 1].transform.position);
                }
            }
        }
    }
}