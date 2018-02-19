using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
    GameObject selectedSolider;
    public GameObject information,image,production,name;

	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

            if(hit.collider != null&& !EventSystem.current.IsPointerOverGameObject())
            {
                if(hit.collider.tag == "Soldier")
                    selectedSolider = hit.collider.gameObject;
                else
                    selectedSolider = null;

                if(hit.collider.tag == "Obstacle")
                {
                    information.SetActive(true);
                   
                    image.transform.GetComponent<Image>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;
                    Sprite sprite = image.transform.GetComponent<Image>().sprite;
                    if(sprite.name=="Barracks")
                        production.SetActive(true); 
                    else
                        production.SetActive(false);                   
                    image.transform.GetComponent<RectTransform>().localScale = new Vector3(sprite.rect.width / 128,sprite.rect.height / 128,0);
                    name.GetComponent<Text>().text = sprite.name;
                }
                else
                {
                    information.SetActive(false);
                    
                }

            }
           
        }
        if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(selectedSolider != null)
            {
               
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
                
                selectedSolider.GetComponent<SoldierControl>().MoveControl(hit.point);
            }
        }
        

    }
}
