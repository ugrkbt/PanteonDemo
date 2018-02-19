using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {
    private bool dragging = false,building=false;
    private float distance;
    Collider2D collider;
    HashSet<GameObject> node;
    private void Start()
    {
        node = new HashSet<GameObject>();
        collider = transform.GetComponent<Collider2D>();
    }
    //Gridi görünür yapar ve fare ile obje arasında farkı yazar
    public void OnBeginDrag()
    {

        distance = Vector3.Distance(transform.position,Camera.main.transform.position);
        dragging = true;
        Grid.Instance.squareManager.SetActive(true);
    }
    //objeyi bırakır en yakın olduğu nodelerin tam ortasına ortası gelecek şekil yerleştirir
    public void OnDrop()
    {
        dragging = false;
        float x = 0, y = 0;
        if(building)
        {
            foreach(GameObject n in node)
            {
                x += n.transform.position.x;
                y += n.transform.position.y;
            }
            transform.position = new Vector2(x / node.Count,y / node.Count);
            gameObject.layer = LayerMask.NameToLayer("Obstacle");
            Grid.Instance.UpdateGrid();
           

        }
        else
            Destroy(gameObject);
        Grid.Instance.squareManager.SetActive(false);
        ColorClear();

    }

    void Update()
    {
        if(dragging)
        {
            ObsGridRay();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }
    //Objeyi gride ayırır ve her nodeden ışın göndererek nelerin üstünde olduğunu bulur
    void ObsGridRay()
    {
        ColorClear();
        node.Clear();
        Vector2 min = collider.bounds.min;
        Vector2 max = collider.bounds.max;
        for(float y = min.y + 0.16f; y < max.y; y += 0.32f)
        { 
            for(float x = min.x+0.16f; x < max.x; x += 0.32f)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(x,y),Vector2.zero);
             
                node.Add(hit.transform.gameObject);
            }
        }
        ColorChange();

    }
    //Obje konuşmaya uygunsa yeşil değilse altında bir şey varsa nodeler kırmızı yanar
    void ColorChange()
    {
        foreach(GameObject n in node)
        {
            if(n.tag != "Node")
            {
                building = false;
                foreach(GameObject y in node)
                {
                    if(y.tag == "Node")
                        y.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,0,0);
                }
                return;
            }
            else
            {
                building = true;
                n.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,1,0);
            }
        }
    }
    //tüm node karelerin rengini gri yapar
    void ColorClear()
    {
        foreach(GameObject n in node)
        {
            if(n.tag == "Node")
            {
                n.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
            }
        }
    }
}
