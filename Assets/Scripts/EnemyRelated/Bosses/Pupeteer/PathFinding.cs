using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    public Transform seeker, target;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void FixedUpdate()
    {
        if (seeker != null)
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> ClosedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count >= 0)
        {
            Node currentNode = openSet.RemoveFirst();
            ClosedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                sw.Stop();
                //print("Path Found: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            //if()

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || ClosedSet.Contains(neighbour)) continue;
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }

            
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY) return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
