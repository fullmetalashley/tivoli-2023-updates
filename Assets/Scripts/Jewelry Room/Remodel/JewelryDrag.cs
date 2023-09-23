using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JewelryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //This is the object that will be dragged.  It's static so only one object can be dragged at a time.
    public static GameObject itemBeingDragged;
    Vector3 startPosition;

    //The starting position of the dragging object.
    Transform startParent;
    //The canvas is used as the main area that the object can't be dragged out of.  It becomes the parent when the object is dragged out of the scroll window.
    public GameObject canvas;


    private CanvasGroup cGroup;

    //SCRIPT REFS
    private JewelryHitboxController hitboxes;
    private JewelryDollSkin dollControl;
    private JewelryClosetController theCloset;
    private JewelrySlotManager slotManager;
    private GarmentDrag dragManager;

    //Index is set manually in the Inspector, corresponding to it's point on the scrolling grid.
    public int index;
    public string subcategory;

    // Start is called before the first frame update
    void Start()
    {
        dollControl = FindObjectOfType<JewelryDollSkin>();
        theCloset = FindObjectOfType<JewelryClosetController>();
        slotManager = FindObjectOfType<JewelrySlotManager>();
        hitboxes = FindObjectOfType<JewelryHitboxController>();
        dragManager = FindObjectOfType<GarmentDrag>();

        //A canvas group needs to be attached to each draggable object.
        cGroup = GetComponent<CanvasGroup>();
    }

    //The following are the interface implementations to allow us to use those interfaces.
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Establishing the drag
        if (!dragManager.dragActive)
        {
            itemBeingDragged = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;
            //Block the raycasts of the canvas group this is a part of in order to prevent it from hiding detections.
            cGroup.blocksRaycasts = false;


            theCloset.subcategory = this.subcategory;
            theCloset.currentCategory = this.subcategory;
            theCloset.ChangeClothingList();

            hitboxes.HitboxOn();

            slotManager.ActivateSlot();
            dragManager.OnDrag();
        }
    }

    //While the item is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        //Item will follow the mouse
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);

        transform.SetParent(canvas.transform);

        cGroup.blocksRaycasts = false;
    }

    //Once the mouse/finger/pointer has released the object
    public void OnEndDrag(PointerEventData eventData)
    {
        hitboxes.HitboxOff();

        //First: Are we in the earrings category?
        if (theCloset.currentCategory == "Earrings")
        {
            if (slotManager.currentSlot.mouseOnSlot || slotManager.otherEarSlot.mouseOnSlot)
            {
                dollControl.AddClothing(index);
            }
        }
        else {
            if (slotManager.currentSlot.mouseOnSlot)
            {
                dollControl.AddClothing(index);
            }
        }
 
        //If the pointer is not on top of the corresponding slot, it will reset back to the original position and the doll will not dress.
        cGroup.blocksRaycasts = true;
        transform.position = startPosition;

        transform.SetParent(startParent);
        dragManager.EndDrag();
    }
}
