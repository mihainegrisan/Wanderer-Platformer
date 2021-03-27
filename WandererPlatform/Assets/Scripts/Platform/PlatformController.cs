using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {
    [SerializeField] private Vector3[] localWaypoints;
    private Vector3[] globalWaypoints;

    [SerializeField] private float speed;
    [SerializeField] private bool cyclic;
    [SerializeField] private float waitTime;
    
    [Range(0, 2)]
    [SerializeField] private float easeAmount;
    
    [SerializeField] private LayerMask passengerMask;
    
    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private float nextMoveTime;
    
    protected override void Start() {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++) {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    private void Update() {
        UpdateRaycastOrigins();
        
        Vector3 velocity = CalculatePlatformMovement();
        
        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    private float Ease(float x) {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
    
    private Vector3 CalculatePlatformMovement() {

        if (Time.time < nextMoveTime) {
            return Vector3.zero;
        }
        
        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints =
            Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);
        
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex],
            easedPercentBetweenWaypoints);

        // reached next waypoint
        if (percentBetweenWaypoints >= 1) {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic) {
                if (fromWaypointIndex >= globalWaypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    Array.Reverse(globalWaypoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }
        
        return newPos - transform.position;
    }
    
    void MovePassengers(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();

		float directionX = Mathf.Sign (velocity.x);
		float directionY = Mathf.Sign (velocity.y);

		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y);
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);
		
				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = directionY == 1 ? velocity.x : 0;
						float pushY = velocity.y - hit.distance * directionY;
		
						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x);
			
			for (int i = 0; i < horizontalRayCount; i ++) {
				Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);
		
				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - hit.distance * directionX;
						float pushY = 0;
						
						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = .3f;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
				
				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = 0;
						float pushY = velocity.y;
						
						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}
	}
    

    private void OnDrawGizmos() {
        if (localWaypoints != null) {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++) {
                Vector3 globalWaypointPos = Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
