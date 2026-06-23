using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject[] WayPoints;

    public Vector3 GetWayPoints(int index)
    {
        //Returning waypoints position accoding to the index 
        return WayPoints[index].transform.position;
    }

    private void OnDrawGizmos()
    {
        if (WayPoints.Length > 0)
        {
            for (int i = 0; i < WayPoints.Length; i++)
            {
                //Setting gui style to see each waypoint name in the scene view
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.blue;
                style.alignment = TextAnchor.MiddleLeft;
                Handles.Label(WayPoints[i].transform.position + Vector3.up * 0.7f, WayPoints[i].name, style);

                if (i < WayPoints.Length - 1)
                {
                    //Drawing gizmos line from waypoint 0 to waypoint n position
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(WayPoints[i].transform.position, WayPoints[i + 1].transform.position);
                }
            }
        }
    }
}
