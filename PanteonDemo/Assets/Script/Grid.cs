using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public GameObject square,squareManager;
    public LayerMask obstacle;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    
    public delegate void AddBuild();
    public static event AddBuild OnUpdate;

    public static Grid Instance;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        Instance = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        
    }
    //En sol baş nodeden başlayarak sırasıyla node oluştur gride atar  
    void CreateGrid()
    {
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint,0,obstacle));
                grid[x,y] = new Node(walkable,worldPoint,x,y);
                
            }
        }
        GridDraw();
    }
    //Griddeki nodelerin yürünebilirliğini günceller ve eventi tetikler
    public void UpdateGrid()
    {
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                bool walkable = !(Physics2D.OverlapCircle(grid[i,j].worldPosition,0,obstacle));
                grid[i,j].walkable = walkable;
            }
        }
        if(OnUpdate != null)
        {
            OnUpdate();
        }
       
    }
    //Nodenin komşularını kontrol eder
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }

        return neighbours;
    }

    //Bir konuma en yakın nodenu bulur
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }
    //Gridin nodelerini 0.32px lik kare ile gösterir.
    void GridDraw()
    {
        if(grid != null)
        {
            foreach(Node n in grid)
            {
               GameObject obje = Instantiate(square,n.worldPosition + new Vector3(0,0,1),Quaternion.identity);
               obje.transform.parent = squareManager.transform;
               
            }
        }
    }


 
}
