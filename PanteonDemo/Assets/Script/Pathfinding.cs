using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    public static Pathfinding Instance;
    List<Node> movePath;
    void Awake()
    {
        Instance = this;
        movePath =new List<Node>();
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        //FindPath(seeker.position,target.position);
    }
    public List<Node> ReturnPath(Vector3 solider,Vector3 target)
    {
        movePath.Clear();
        FindPath(solider,target);
        return movePath;
    }
    //En az maliyetli yollardan başlayarak hedef konumun nodesini bulana kadar tüm nodeleri dolaşır.
    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
       
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node node = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if(openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if(node == targetNode)
            {
                RetracePath(startNode,targetNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(node))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node,neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour,targetNode);
                    neighbour.parent = node;

                    if(!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }
    //En kısa yolu path listine atar
    void RetracePath(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        movePath = path;

    }
    //2 node arasında maliyet uzaklığını bulur 
    int GetDistance(Node nodeA,Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
