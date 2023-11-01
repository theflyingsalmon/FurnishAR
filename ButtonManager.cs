using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    private Button btn;
    public GameObject furniture;


    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        // if the button has entered the "Selection Point"
        if (UIManager.Instance.OnEntered(gameObject))
        {
            // scale up the button to provide visual feedback
            transform.DOScale(Vector3.one * 2, 0.2f);
            //transform.localScale = Vector3.one * 2;
        }
        else
        {
            // keep the scale the same
            transform.DOScale(Vector3.one, 0.2f);
           // transform.localScale = Vector3.one;
        }
    }

    void SelectObject()
    {
        DataHandler.Instance.furniture = furniture;
    }
}
