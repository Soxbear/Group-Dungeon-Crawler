using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingController : MonoBehaviour
{

	Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
	PathRequest CurrentPathRequest;

	static PathfindingController Instance;
	bool IsProcessingPath;

	public static void RequestPath(Vector2 PathStart, Vector2 PathEnd, Action<Vector2[], bool> Callback)
    {
		PathRequest NewRequest = new PathRequest(PathStart, PathEnd, Callback);
		Instance.PathRequestQueue.Enqueue(NewRequest);
		Instance.TryProcessNext();
    }

	void TryProcessNext()
    {
		if (!IsProcessingPath && PathRequestQueue.Count > 0)
        {
			CurrentPathRequest = PathRequestQueue.Dequeue();
			IsProcessingPath = true;
			
        }
    }

	void FinishedProcessingPath(Vector2[] Path, bool Success)
    {
		CurrentPathRequest.Callback(Path, Success);
		IsProcessingPath = false;
		TryProcessNext();
    }

	public void CalculatePath(Vector2 StartPos, Vector2 EndPos)
    {
		StartCoroutine(FindPath(StartPos, EndPos));
    }

	IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
	{

		Vector2[] Waypoints = new Vector2[0];
		bool PathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		if (!startNode.Blocked && !targetNode.Blocked)
		{

			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode)
				{
					PathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					if (neighbour.Blocked || closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
					{
						neighbour.GCost = newMovementCostToNeighbour;
						neighbour.HCost = GetDistance(neighbour, targetNode);
						neighbour.Parent = currentNode;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
						{
							openSet.UpdateItem(neighbour);
						}
					}
				}
			}
		}
		yield return null;
		if (PathSuccess)
			Waypoints = RetracePath(startNode, targetNode);
		FinishedProcessingPath(Waypoints, PathSuccess);
	}

	Vector2[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}

		Vector2[] Waypoints = SimplifyPath(path);

		//Waypoints.Reverse();

		return Waypoints;
	}

	Vector2[] SimplifyPath(List<Node> Path)
    {
		List<Vector2> Waypoints = new List<Vector2>();
		Vector2 OldDirection = Vector2.zero;

		Vector2[] pog = { new Vector2(0f, 0f) };

		return pog;
    }

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.GridPos.x - nodeB.GridPos.x);
		int dstY = Mathf.Abs(nodeA.GridPos.y - nodeB.GridPos.y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}

	PathfindingGrid grid;

	void Awake()
	{
		grid = GetComponent<PathfindingGrid>();
		Instance = this;
	}

	struct PathRequest
    {
		public Vector2 PathStart;
		public Vector3 PathEnd;
		public Action<Vector2[], bool> Callback;

		public PathRequest(Vector2 Start, Vector2 End, Action<Vector2[], bool> Call)
        {
			PathStart = Start;
			PathEnd = End;
			Callback = Call;
        }
    }
}
