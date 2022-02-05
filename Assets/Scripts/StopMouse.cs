using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMouse : MonoBehaviour
{
    public SavedInfo savedInfo;
    public RectTransform rect;
    public Vector2 xLimits;
    public Vector2 yLimits;
    public Vector2 mousePos;

    void Start()
    {
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        xLimits = new Vector2(-(Screen.width / 2.2f), (Screen.width / 2.2f));
        yLimits = new Vector2(-(Screen.height / 2.2f), (Screen.height / 2.2f));
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.x -= Screen.width/2;
        mousePos.y -= Screen.height/2;
        if(mousePos.x > xLimits.x && mousePos.x < xLimits.y && mousePos.y > yLimits.x && mousePos.y < yLimits.y){
            savedInfo.canDrag = true;
        }else if(savedInfo.dragging){
            savedInfo.canDrag = false;
        }
    }
    
    private void OnMouseOver() {
        // savedInfo.canDrag = true;
        // Debug.Log("Over");
    }

    private void OnMouseExit() {
        // savedInfo.canDrag = false;
        // Debug.Log("Exit");
    }
}
