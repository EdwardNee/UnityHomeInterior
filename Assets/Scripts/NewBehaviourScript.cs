using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject prefab;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        var colid = prefab.GetComponent<BoxCollider>();

        x = colid.size.x;
        y = colid.size.y;
        z = colid.size.z;
       
        btn.onClick.AddListener(Cliccc);
        Debug.Log(prefab.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static int id;
    public void Cliccc() {
        prefab.name = prefab.name + id;
        Debug.Log(prefab.name);
        id++;
        Instantiate(prefab, new Vector3(), prefab.transform.rotation);
        //var colid = prefab.GetComponent<BoxCollider>();
        ////Берем размер объекта.
        //float xVal = prefab.transform.localScale.x;
        //float yVal = prefab.transform.localScale.y;
        //float zVal = prefab.transform.localScale.z;


        ////Если расстояние между пальцами стало меньше, то объект уменьшаем, иначе увеличиваем.
        ////if (delta > 0)
        ////{
        ////    prefab.transform.localScale = new Vector3(xVal + xVal * 0.05f, yVal + yVal * 0.05f, zVal + zVal * 0.05f);
        ////}
        ////else if (delta < 0)
        ////{
        //x = x - x * 0.05f;
        //y = y - y * 0.05f;
        //z = z - z * 0.05f;
        //Debug.Log($"{x} {y} {z}");

        ////colid.size = new Vector3(colid.size.x - colid.size.x * 0.005f, colid.size.y - colid.size.y * 0.005f, colid.size.z - colid.size.z * 0.005f);
        //    prefab.transform.localScale = new Vector3(xVal - xVal * 0.05f, yVal - yVal * 0.05f, zVal - zVal * 0.05f);
        ////}
    }

    float x;
    float y;
    float z;
}
