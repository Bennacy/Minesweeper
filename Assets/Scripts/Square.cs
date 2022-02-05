using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    private Vector2 prevMousePos;
    public Grid gridScript;
    // public Text txt;
    public bool isBomb;
    public int col;
    public int row;
    public int adjBombs;
    private bool highlighting;
    public SpriteRenderer sr;
    public enum State {blank, flagged, revealed, highlighted};
    public State state;
    public SavedInfo savedInfo;
    public GameObject bomb;
    public GameObject cross;
    public bool clickedL;
    public bool clickedR;
    public bool dragging;
    public float dragTolerance;
    private Vector2 prevGridPos;
    private bool resetPos;
    public Sprite[] sprites;
    public Sprite[] revealedNums;
    public ResetButton resetButton;
    public bool killingTile = false;

    private GameObject[] adjacent; // Starts up and moves clockwise
    
    void Start()
    {
        clickedL = false;
        gridScript = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        resetButton = GameObject.FindGameObjectWithTag("Reset").GetComponent<ResetButton>();
        // txt.enabled = false;
        GetAdjTiles();
        GetAdjBombs();
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        switch(state){
            case State.blank:
                sr.sprite = sprites[0];
                break;
            case State.flagged:
                if(savedInfo.canPlay){
                    sr.sprite = sprites[2];
                }else{
                    if(!isBomb){
                        sr.sprite = sprites[6];
                    }
                }
                break;
            case State.revealed:
                if(!isBomb){
                    sr.sprite = revealedNums[adjBombs];
                }else{
                    if(killingTile){
                        sr.sprite = sprites[5];
                    }else{
                        sr.sprite = sprites[4];
                    }
                }
                break;
            case State.highlighted:
                sr.sprite = sprites[1];
                break;
        }
        if(clickedL){
            resetButton.choosing = true;
            // Mathf.Abs(mousePos.x) > Mathf.Abs(prevMousePos.x + dragTolerance) || Mathf.Abs(mousePos.y) > Mathf.Abs(prevMousePos.y + dragTolerance)
            if(Mathf.Abs(mousePos.x - prevMousePos.x) >= dragTolerance || Mathf.Abs(mousePos.y - prevMousePos.y) >= dragTolerance){
                dragging = true;
                savedInfo.dragging = true;
            }else{
                dragging = false;
            }
            if(Input.GetMouseButtonUp(0)){
                clickedL = false;
                dragging = false;
                savedInfo.dragging = false;
            }
        }
        if(dragging && savedInfo.canDrag){
            if(Input.GetMouseButtonUp(0)){
                clickedL = false;
                dragging = false;
                savedInfo.dragging = false;
            }
            Vector2 newPos = new Vector2(prevGridPos.x + (mousePos.x - prevMousePos.x), prevGridPos.y + (mousePos.y - prevMousePos.y));
            // if(Mathf.Abs(gridScript) )
            gridScript.transform.position = newPos;
        }else if(!savedInfo.canDrag){
            dragging = false;
            savedInfo.dragging = false;
            clickedL = false;
            gridScript.transform.position = new Vector2(0, -1);
        }
    }

    private void OnMouseOver(){
            if(Input.GetMouseButtonDown(0)){
                prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                prevGridPos = gridScript.transform.position;
                clickedL = true;
                if(state == State.revealed)
                {
                    ShowAdj();
                }
            }else if(Input.GetMouseButtonDown(1)){
                clickedR = true;
            }else if(Input.GetMouseButtonUp(0)){
                resetButton.choosing = false;
                savedInfo.canDrag = true;
                HideAdj();
                if(clickedL && !dragging){
                    clickedL = false;
                    if(state == State.blank){
                        if(isBomb){
                            killingTile = true;
                        }
                        if(adjBombs == 0){
                            RevealZeroes();
                        }else{
                            Reveal();
                        }
                    }else if(state == State.revealed){
                        if(CountFlags() == adjBombs){
                            RevealAdj();
                        }
                    }
                }
            }else if(Input.GetMouseButtonUp(1)){
                if(clickedR){
                    clickedR = false;
                    if(state == State.blank && savedInfo.availableFlags > 0){
                        state = State.flagged;
                        savedInfo.availableFlags--;
                        if(isBomb){
                            savedInfo.correctFlags++;
                        }
                    }else if(state == State.flagged){
                        state = State.blank;
                        savedInfo.availableFlags++;
                        if(isBomb){
                            savedInfo.correctFlags--;
                        }
                    }
                }
            }
    }

    private void OnBecameInvisible() {
        savedInfo.outside++;
    }

    private void OnBecameVisible() {
        savedInfo.outside--;
    }

    private void OnMouseExit() {
        if(highlighting){
            HideAdj();
        }
        savedInfo.canDrag = true;
        clickedL = false;
        clickedR = false;
    }

    private void Reveal(){
        if(savedInfo.canPlay){
            state = State.revealed;
            savedInfo.hiddenTiles--;
            // txt.enabled = true;
            if(isBomb){
                killingTile = true;
                savedInfo.canPlay = false;
                gridScript.RevealGrid();
            }
        }
    }

    private void GetAdjTiles(){
        adjacent = new GameObject[8];
        bool lastX = !(col < gridScript.cols - 1);
        bool firstX = !(col > 0);
        bool lastY = !(row < gridScript.rows - 1);
        bool firstY = !(row > 0);
        Transform checkTile;

        if(!lastY){
            checkTile = gridScript.GetTile(row + 1, col + 0);
            adjacent[0] = checkTile.gameObject;
        }else{
            adjacent[0] = null;
        }

        if(!lastX && !lastY){
            checkTile = gridScript.GetTile(row + 1, col + 1);
            adjacent[1] = checkTile.gameObject;
        }else{
            adjacent[1] = null;
        }

        if(!lastX){
            checkTile = gridScript.GetTile(row + 0, col + 1);
            adjacent[2] = checkTile.gameObject;
        }else{
            adjacent[2] = null;
        }

        if(!lastX && !firstY){
            checkTile = gridScript.GetTile(row - 1, col + 1);
            adjacent[3] = checkTile.gameObject;
        }else{
            adjacent[3] = null;
        }

        if(!firstY){
            checkTile = gridScript.GetTile(row - 1, col + 0);
            adjacent[4] = checkTile.gameObject;
        }else{
            adjacent[4] = null;
        }

        if(!firstX && !firstY){
            checkTile = gridScript.GetTile(row - 1, col - 1);
            adjacent[5] = checkTile.gameObject;
        }else{
            adjacent[5] = null;
        }

        if(!firstX){
            checkTile = gridScript.GetTile(row + 0, col - 1);
            adjacent[6] = checkTile.gameObject;
        }else{
            adjacent[6] = null;
        }

        if(!firstX && !lastY){
            checkTile = gridScript.GetTile(row + 1, col - 1);
            adjacent[7] = checkTile.gameObject;
        }else{
            adjacent[7] = null;
        }
    }

    private void GetAdjBombs(){
        if(savedInfo.canPlay){
            foreach(GameObject tile in adjacent){
                if(tile != null){
                    Square tileScript = tile.GetComponent<Square>();
                    if(tileScript.isBomb){
                        adjBombs++;
                    }
                }
            }
            if(adjBombs > 0){
                // txt.text = adjBombs.ToString();
            }
            if(isBomb){
                // txt.text = "";
            }
        }
    }

    private void ShowAdj(){
        if(savedInfo.canPlay){
            highlighting = true;
            foreach(GameObject tile in adjacent){
                if(tile != null){
                    Square tileScript = tile.GetComponent<Square>();
                    if(tileScript.state == State.blank){
                        tileScript.state = State.highlighted;
                    }
                }
            }
        }
    }

    private void HideAdj(){
        if(savedInfo.canPlay){
            highlighting = false;
            foreach(GameObject tile in adjacent){
                if(tile != null){
                    Square tileScript = tile.GetComponent<Square>();
                    if(tileScript.state == State.highlighted){
                        tileScript.state = State.blank;
                    }
                }
            }
        }
    }

    private void RevealZeroes(){
        if(savedInfo.canPlay){
            Reveal();
            foreach(GameObject tile in adjacent){
                if(tile != null){
                    Square tileScript = tile.GetComponent<Square>();
                    if(tileScript.adjBombs == 0 && tileScript.state == State.blank){
                        tileScript.RevealZeroes();
                    }else if(tileScript.state == State.blank){
                        tileScript.Reveal();
                    }
                }
            }
        }
    }

    private void RevealAdj(){
        if(savedInfo.canPlay){
            foreach(GameObject tile in adjacent){
                if(tile != null){
                    Square tileScript = tile.GetComponent<Square>();
                    if(tileScript.state != State.revealed && tileScript.state != State.flagged){
                        tileScript.Reveal();
                        if(tileScript.adjBombs == 0){
                            tileScript.RevealZeroes();
                        }
                    }
                }
            }
        }
    }

    private int CountFlags(){
        int count = 0;
        foreach(GameObject tile in adjacent){
            if(tile != null){
                Square tileScript = tile.GetComponent<Square>();
                if(tileScript.state == State.flagged){
                    count++;
                }
            }
        }
        return count;
    }

    private void RevealGrid(){

        Reveal();
        foreach(GameObject tile in adjacent){
            if(tile != null){
                Square tileScript = tile.GetComponent<Square>();
                if(tileScript.state == State.blank){
                    tileScript.RevealGrid();
                    if(tileScript.state == State.flagged && !tileScript.isBomb){
                        tileScript.sr.sprite = sprites[5];
                    }
                }
            }
        }
    }
}
