using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFurniture : MonoBehaviour
{

    private static DataHandler[] furn;

  
    // Start is called before the first frame update
    void Start()
    {
        furn = FindObjectsOfType<DataHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject go = hit.collider.gameObject;
                // Check if the hit object is the instantiated prefab
                if (go.CompareTag("Object"))
                {
                    Debug.Log("Hit the prefab!");
                }
            }
        }
    }
}
