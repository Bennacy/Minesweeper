using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Sprite[] sprites;
    public Button button;
    public Image image;
    public SavedInfo savedInfo;
    public bool pressed;
    public bool choosing;
    public bool dead;


    // Start is called before the first frame update
    void Start()
    {
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        button.onClick.AddListener(Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        if(savedInfo.animateFace){
            if(savedInfo.canPlay){
                image.sprite = sprites[0];
            }
            if(choosing){
                image.sprite = sprites[2];
            }
            if(!savedInfo.canPlay){
                image.sprite = sprites[3];
            }
            if(pressed){
                image.sprite = sprites[1];
            }
        }else{
            image.sprite = sprites[0];
        }
    }

    private void OnMouseExit() {
        pressed = false;
    }

    private void OnMouseOver() {
        if(Input.GetMouseButton(0)){
            pressed = true;
        }
    }

    public void Clicked(){
        savedInfo.StartLevel();
    }
}
