using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyPathFindingTest : MonoBehaviour
{
	public Transform target;
	
	public float speed = 10f;
	public float nextWaypointDistance = 3f;

	Path path;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;

	Transform enemyGFX;
	Seeker seeker;
	Rigidbody2D rb;

	private void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		enemyGFX=  GetComponent<Transform>();

		InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
	}

	private void UpdatePath()
	{
		if (seeker.IsDone())
			seeker.StartPath(rb.position, target.position, OnPathComplete);
	}

	private void Update()
	{
		if (path == null) return;

		if(currentWaypoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 dircetion = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
		Vector2 force = dircetion * speed;
		
		rb.velocity = force;

		float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

		if (distance < nextWaypointDistance)
		{
			currentWaypoint = currentWaypoint + 1;
		}

		if(rb.velocity.x >= 0.01f)
		{
			enemyGFX.localScale = new Vector3(1f, 1f, 1f);
		}
		else if(rb.velocity.x <= -0.01f)
		{
			enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	private void OnPathComplete(Path pt)
	{
		if (!pt.error)
		{
			path = pt;
			currentWaypoint = 0;
		}
	}
}
