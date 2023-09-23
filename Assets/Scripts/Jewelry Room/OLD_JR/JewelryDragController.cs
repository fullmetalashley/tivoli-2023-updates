using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JewelryDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject itemBeingDragged;
    Vector3 startPosition;

    //The starting position of the dragging object.
    Transform startParent;
    //The canvas is used as the main area that the object can't be dragged out of.  It becomes the parent when the object is dragged out of the scroll window.
    public GameObject canvas;

    private JewelryCloset jewelryCloset;
    private JewelryDollManager jewelryDollControls;
    private CanvasGroup cGroup;
    private JewelryLoader jewelryPopulator;
    private JewelrySpreadController theSpread;
    private JewelryRemovalSystem jewelRemoval;

    public string category;

    private JewelryHitboxController hitboxes;

    //Index is set manually in the Inspector, corresponding to it's point on the scrolling grid.
    public int index;


    // Start is called before the first frame update
    void Start()
    {
        jewelryCloset = FindObjectOfType<JewelryCloset>();
        jewelryDollControls = FindObjectOfType<JewelryDollManager>();
        jewelryPopulator = FindObjectOfType<JewelryLoader>();
        theSpread = FindObjectOfType<JewelrySpreadController>();

        cGroup = GetComponent<CanvasGroup>();
        hitboxes = FindObjectOfType<JewelryHitboxController>();
        jewelRemoval = FindObjectOfType<JewelryRemovalSystem>();

    }

    //The following are the interface implementations to allow us to use those interfaces.
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        //Block the raycasts of the canvas group this is a part of in order to prevent it from hiding detections.
        cGroup.blocksRaycasts = false;
        jewelryPopulator.SwitchLists(category);
        theSpread.itemActivation.SetActive(false);

        hitboxes.HitboxOn();

        jewelRemoval.RemovalOff();

        foreach (KeyValuePair<string, Image> img in jewelryDollControls.currentDoll.dollClothing)
        {
            if (jewelryPopulator.currentCategory == "Earrings")
            {
                jewelryDollControls.currentDoll.dollClothing["Left Ear"].raycastTarget = true;
                jewelryDollControls.currentDoll.dollClothing["Right Ear"].raycastTarget = true;

                foreach(KeyValuePair<string, Image> img2 in jewelryDollControls.currentDoll.dollClothing)
                {
                    if (img2.Key != "Left Ear" && img2.Key != "Right Ear")
                    {
                        jewelryDollControls.currentDoll.dollClothing[img2.Key].raycastTarget = false;
                    }
                }

            }
            else
            {
                if (img.Key != jewelryPopulator.currentCategory)
                {
                    jewelryDollControls.currentDoll.dollClothing[img.Key].raycastTarget = false;
                }
                else
                {
                    jewelryDollControls.currentDoll.dollClothing[img.Key].raycastTarget = true;
                }
            }
        }

    }

    //While the item is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        //Item will follow the mouse
        transform.position = Input.mousePosition;
        //Parent is removed so the object can float around the canvas

        transform.SetParent(canvas.transform);
 //       transform.parent = canvas.transform;
        cGroup.blocksRaycasts = false;

        //If the current target image of the doll doesn't have an existing sprite, the image needs to be turned on.  No color will be added.     
        if (!jewelryCloset.CheckForClothes(jewelryDollControls.currentDoll))
        {
            jewelryCloset.ActivateImage();
        }

        
    }

    //Once the mouse/finger/pointer has released the object
    public void OnEndDrag(PointerEventData eventData)
    {
        if (jewelryPopulator.currentCategory == "Earrings")
        {
           
            if (jewelryCloset.currentSlot.mouseOnSlot || jewelryCloset.otherEarSlot.mouseOnSlot)
            {
                jewelryCloset.currentSlot.hasClothing = true;
                jewelryCloset.otherEarSlot.hasClothing = true;
                jewelryDollControls.ChangeClothes(index);
                //The clothing image will turn on and become fully colored in.
                jewelryCloset.ActivateImage();
                jewelryCloset.ImageColorOn();


            }
            else
            {
                if (!jewelryCloset.currentSlot.hasClothing || !jewelryCloset.otherEarSlot.hasClothing)
                {
                    jewelryCloset.DeactivateImage();
                }
            }
        }
        else
        {
            //If the pointer is on top of the corresponding current slot, it can be released.
            if (jewelryCloset.currentSlot.mouseOnSlot)
            {
                jewelryCloset.currentSlot.hasClothing = true;

                //The doll will change closed based on the index of this drag slot.
                jewelryDollControls.ChangeClothes(index);
                //The clothing image will turn on and become fully colored in.
                jewelryCloset.ActivateImage();
                jewelryCloset.ImageColorOn();

            }
            //If the pointer is not on top of the corresponding slot, it will reset back to the original position and the doll will not dress.
            else
            {
                //If the doll didn't dress, turn the image back off.
                //Has Clothing has use if the doll currently has an outfit on, but did not successfully dress with the new clothes.  This way the previous outfit will not be immediately turned off.
                if (!jewelryCloset.currentSlot.hasClothing)
                {
                    jewelryCloset.DeactivateImage();
                }

            }
        }
        cGroup.blocksRaycasts = true;
        transform.position = startPosition;
        //transform.parent = startParent;

        transform.SetParent(startParent);
        theSpread.itemActivation.SetActive(true);
        hitboxes.HitboxOff();

    }
}
