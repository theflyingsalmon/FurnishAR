using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using Lean.Touch;
using DG.Tweening;
public class ARPlacement : MonoBehaviour
{

    public GameObject placementIndicator; 
    private GameObject spanewdObject;
    public Pose placementPose;
    private ARRaycastManager arRaycastManager;
    private bool placementPoseIsValid = false;


    public GameObject DeleteButton;
    public GameObject RedButton;
    public GameObject GreenButton;
    public GameObject BlueButton;
    public GameObject WhiteButton;
    public GameObject BlackButton;


    public Button delbut;

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        DeleteButton.SetActive(false);
        RedButton.SetActive(false);
        GreenButton.SetActive(false);
        BlueButton.SetActive(false);
        WhiteButton.SetActive(false);
        BlackButton.SetActive(false);

        spanewdObject.GetComponent<Outline>().enabled = false;
        spanewdObject.GetComponent<LeanDragTranslate>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (spanewdObject != null)
        {
            if (Input.touchCount > 0)
            {
                Debug.Log("Inside of the Object not null statement");
                
                Touch touch = Input.GetTouch(0);
                // if the user touch is over an UI element don't proceed
                if (isOverUI(touch)) return;
                

                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("Touch Phase Began");
                    Ray ray = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    

                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Raycast ray out");
                        if (hit.collider != null)
                        {

                        }
                        Debug.DrawLine(ray.origin, hit.point, Color.red);
                        // Check if the hit object is one of the instantiated objects
                        if (hit.collider.CompareTag("Object"))
                        {
                            // Select the object
                            
                            SelectObject();
                            
                        }

                    }
                    else
                    {
                        DeselectObject();
                    }
                }
            }
        }
        
        if (spanewdObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            if (isOverUI(touch)) return;
            ARPlaceObject();
            Debug.Log("Object has been placed");
        }

        // if the spawned object is null continue to update the placement indicator
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        
    }

    void UpdatePlacementIndicator()
    {
        if (spanewdObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spanewdObject = Instantiate(DataHandler.Instance.furniture, placementPose.position, placementPose.rotation);
        
    }

    // Method for detecting if the user touch is on a UI element
    bool isOverUI(Touch touch)
    {

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            // iterate through all raycast results
            foreach(var go in results)
            {
                Debug.Log(go.gameObject.name, go.gameObject);
                // if delete button then delete the instantiated object
                if (go.gameObject.name == "DeleteButton")
                {
                    DeleteGameObject();
                }
                // if red button then change the material color to red
                else if (go.gameObject.name =="RedButton")
                {
                    RedColorChange();
                }
                // if green button then change the material color to green
                else if (go.gameObject.name =="GreenButton")
                {
                    GreenColorChange();
                }
                else if (go.gameObject.name =="BlueButton")
                {
                    BlueColorChange();
                }
                else if (go.gameObject.name =="WhiteButton")
                {
                    WhiteColorChange();
                }
                else if (go.gameObject.name =="BlackButton")
                {
                    BlackColorChange();
                }
            }
        }


        return results.Count > 0;
    }

    void DeleteGameObject()
    {
        Destroy(spanewdObject);
        DeselectObject();
    }
    
    // method for enabling all components and UI elements upon selection
    void SelectObject()
    {
        Debug.Log("Object selected!");
                            
        spanewdObject.GetComponent<Outline>().enabled = true;
        spanewdObject.GetComponent<LeanDragTranslate>().enabled = true;
        DeleteButton.SetActive(true);
        RedButton.SetActive(true);
        GreenButton.SetActive(true);
        BlueButton.SetActive(true);
        WhiteButton.SetActive(true);
        BlackButton.SetActive(true);
    }

    // method for disabling all components and UI elements upon deselction
    void DeselectObject()
    {
        Debug.Log("Object Deselected");

        spanewdObject.GetComponent<Outline>().enabled = false;
        spanewdObject.GetComponent<LeanDragTranslate>().enabled = false;
        DeleteButton.SetActive(false);
        RedButton.SetActive(false);
        GreenButton.SetActive(false);
        BlueButton.SetActive(false);
        WhiteButton.SetActive(false);
        BlackButton.SetActive(false);
    }

    // method for changing instantiated objects material color to red
    void RedColorChange()
    {
        // get the renderer of the spawned object
        var furnitureRenderer = spanewdObject.GetComponent<Renderer>();
        furnitureRenderer.material.SetColor("_Color", Color.red);
    }
    // method for changing instantiated objects material color to green
    void GreenColorChange()
    {
        // get the renderer of the spawned object
        var furnitureRenderer = spanewdObject.GetComponent<Renderer>();
        furnitureRenderer.material.SetColor("_Color", Color.green);
    }

    // method for changing instantiated objects material color to blue
    void BlueColorChange()
    {
        // get the renderer of the spawned object
        var furnitureRenderer = spanewdObject.GetComponent<Renderer>();
        furnitureRenderer.material.SetColor("_Color", Color.blue);
    }

    void BlackColorChange()
    {
        // get the renderer of the spawned object
        var furnitureRenderer = spanewdObject.GetComponent<Renderer>();
        furnitureRenderer.material.SetColor("_Color", Color.black);
    }

    void WhiteColorChange()
    {
        // get the renderer of the spawned object
        var furnitureRenderer = spanewdObject.GetComponent<Renderer>();
        furnitureRenderer.material.SetColor("_Color", Color.white);
    }
}