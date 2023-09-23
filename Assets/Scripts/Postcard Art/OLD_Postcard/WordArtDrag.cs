using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordArtDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //This is the object that will be dragged.  It's static so only one object can be dragged at a time.
    public static GameObject itemBeingDragged;
    Vector3 startPosition;

    //The starting position of the dragging object.
    Transform startParent;
    //The canvas is used as the main area that the object can't be dragged out of.  It becomes the parent when the object is dragged out of the scroll window.
    public GameObject canvas;

    private CanvasGroup cGroup;

    private PostcardControls postControls;

    // Start is called before the first frame update
    void Start()
    {
        cGroup = GetComponent<CanvasGroup>();

        postControls = FindObjectOfType<PostcardControls>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        //Block the raycasts of the canvas group this is a part of in order to prevent it from hiding detections.
        cGroup.blocksRaycasts = false;
        postControls.PanelsOn();
        postControls.removeButton.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Item will follow the mouse
        transform.position = Input.mousePosition;
        //Parent is removed so the object can float around the canvas
        //       transform.parent = canvas.transform;
        transform.SetParent(canvas.transform);
        cGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //If we are on top of a panel, set that panel image to this item.
        if (postControls.SetPanelImage() != -1)
        {
            postControls.targetPanels[postControls.SetPanelImage()].panelArt.sprite = this.GetComponent<Image>().sprite;
            postControls.targetPanels[postControls.SetPanelImage()].panelArt.enabled = true;
            postControls.targetPanels[postControls.SetPanelImage()].hasArt = true;
        }
        postControls.PanelsOff();

        cGroup.blocksRaycasts = true;
        transform.position = startPosition;

        transform.SetParent(startParent);
        postControls.removeButton.SetActive(true);


    }
}
