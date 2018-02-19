using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    GameObject instance;
    public GameObject spawnPoint;
 
    //Sürüklemeye başladığında verilen objeyi  fare konumunda çıkarır ve OnbeginDrag fonksiyonunu çalıştırır
    public void BeginDrag(GameObject obstacle)
    {
        
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

        instance = Instantiate(obstacle,hit.point,Quaternion.identity);
        instance.GetComponent<DragAndDrop>().OnBeginDrag();
    }
    //farenin tuşunu bıraktığında objeyini OnDrop fonksiyonun çalıştırır
    public void Drop()
    {
        instance.GetComponent<DragAndDrop>().OnDrop();
    }
    //Verilen objeyi spawn pointte çikarırır
    public void ProductSoldier(GameObject soldier)
    {
        Instantiate(soldier,spawnPoint.transform.position-new Vector3(0,0,1),Quaternion.identity);
    }
}
