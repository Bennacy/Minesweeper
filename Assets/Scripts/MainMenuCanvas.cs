using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public SavedInfo savedInfo;
    public Dropdown selection;
    public Slider xSlider;
    public Slider ySlider;
    public Slider bSlider;
    public Text colsTxt;
    public Text rowsTxt;
    public Text bombsTxt;
    private bool selectedOption;

    void Start()
    {
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        selection.onValueChanged.AddListener(delegate{OfficialBoards();});
        selection.value = savedInfo.board;
        xSlider.onValueChanged.AddListener(delegate{ValueChange();});
        ySlider.onValueChanged.AddListener(delegate{ValueChange();});
        bSlider.onValueChanged.AddListener(delegate{ValueChange();});
        OfficialBoards();
    }

    // Update is called once per frame
    void Update()
    {
        colsTxt.text = savedInfo.gridX.ToString();
        rowsTxt.text = savedInfo.gridY.ToString();
        bombsTxt.text = savedInfo.gridBombs.ToString();
    }

    public void OfficialBoards(){
        selectedOption = true;
        savedInfo.board = selection.value;
        switch(selection.value){
            case 0:
                savedInfo.gridX = 8;
                savedInfo.gridY = 8;
                savedInfo.gridBombs = 10;
                break;
            case 1:
                savedInfo.gridX = 16;
                savedInfo.gridY = 16;
                savedInfo.gridBombs = 40;
                break;
            case 2:
                savedInfo.gridX = 30;
                savedInfo.gridY = 16;
                savedInfo.gridBombs = 99;
                break;
        }
        UpdateSliders();
    }

    public void ValueChange(){
        if(!selectedOption){
            savedInfo.gridX = (int)xSlider.value;
            savedInfo.gridY = (int)ySlider.value;
            
            int x = savedInfo.gridX;
            int y = savedInfo.gridY;
            int maxBombs = (int)Mathf.Floor((x * y) / 3.3333333333333333333f);
            bSlider.maxValue = maxBombs;
            savedInfo.gridBombs = (int)bSlider.value;
            selection.value = 3;
            OfficialBoards();
        }
    }

    public void UpdateSliders(){
        int x = savedInfo.gridX;
        int y = savedInfo.gridY;
        int maxBombs = (int)Mathf.Floor((x * y) / 3.3333333333333333333f);
        bSlider.maxValue = maxBombs;

        xSlider.value = x;
        ySlider.value = y;
        bSlider.value = savedInfo.gridBombs;
        selectedOption = false;
    }
}
