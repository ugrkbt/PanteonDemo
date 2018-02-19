using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierControl : MonoBehaviour {
    public float speed = 2;
    Vector3[] path;
    int targetIndex;
    Vector3 currentWaypoint,pastTarget;
    List<Node> movePath ;


    // Use this for initialization
    void Start()
    {
        
        Grid.OnUpdate += Grid_OnUpdate;
        movePath = new List<Node>();
        currentWaypoint = transform.position;
        pastTarget = transform.position;
    }
    //grid güncellendiğinde yeni rota bul
    private void Grid_OnUpdate()
    {
        MoveControl(pastTarget);
    }
 
    //Verilen noktaya yol çıkar coruntine başlat
    public void MoveControl(Vector3 targetPos)
    {
        pastTarget = targetPos;
        movePath = Pathfinding.Instance.ReturnPath(transform.position,targetPos);
        path = new Vector3[movePath.Count];
        int index = 0;
        foreach(Node n in movePath)
        {
            path[index] = n.worldPosition;
            index++;
        }

        StopCoroutine("FollowPath");
        if(path.Length > 0)
        {
            targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }
    //yolu takip et
    IEnumerator FollowPath()
    {

        currentWaypoint = path[0];
        while(true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
            yield return null;
        }
    }

}
