using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    int currentWaypoint = 0;
    [SerializeField] private float speed = 0.1f;

    void Update()
    {
        if(Vector2.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
        {
            currentWaypoint=(currentWaypoint+1)% waypoints.Length;
            Debug.Log("currentWaypoint: " + currentWaypoint);
        }
        transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime*10);
    }
}
