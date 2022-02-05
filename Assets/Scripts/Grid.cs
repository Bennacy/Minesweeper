using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int totalMines = 10;
    private int prevMines;
    private int placedMines;
    public int rows = 5;
    public int cols = 8;
    private int prevRows;
    private int prevCols;
    public float tileSize = 1;
    private float prevSize;
    public float zoomSpeed;
    public GameObject refTile;
    public SavedInfo savedInfo;
    public BoxCollider2D coll;


    // Start is called before the first frame update
    void Start()
    {
        savedInfo = GameObject.FindGameObjectWithTag("SavedInfo").GetComponent<SavedInfo>();
        cols = savedInfo.gridX;
        rows = savedInfo.gridY;
        totalMines = savedInfo.gridBombs;
        GenerateGrid();
        savedInfo.canPlay = true;
        savedInfo.correctFlags = 0;
        savedInfo.totalBombs = totalMines;
        savedInfo.availableFlags = totalMines;
        savedInfo.hiddenTiles = (rows * cols) - totalMines;
        float xColOffset;
        float yColOffset;
        if(cols % 2 == 1){
            xColOffset = 0;
        }else{
            xColOffset = -0.5f;
        }
        if(rows % 2 == 1){
            yColOffset = 0;
        }else{
            yColOffset = -0.5f;
        }
        coll.offset = new Vector2(xColOffset, yColOffset);
        coll.size = new Vector2(cols * tileSize, rows * tileSize);
    }

    // Update is called once per frame
    void Update()
    {
        if(prevCols != cols || prevRows != rows || prevSize != tileSize || prevMines != totalMines){
            GenerateGrid();
        }

        if(Input.mouseScrollDelta.y != 0){
            Vector2 newScale = new Vector2(transform.localScale.x + (Input.mouseScrollDelta.y * zoomSpeed), transform.localScale.y + (Input.mouseScrollDelta.y * zoomSpeed));
            if(newScale.x >= .5f && newScale.x <= 5){
                transform.localScale = newScale;
            }
        }
    }

    private void GenerateGrid(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }

        placedMines = 0;
        prevRows = rows;
        prevCols = cols;
        prevSize = tileSize;
        prevMines = totalMines;

        int halfX = (int)Mathf.Floor(cols / 2);
        int halfY = (int)Mathf.Floor(rows / 2);

        for (int row = 0; row < rows; row++)
        {
            GameObject currRow = new GameObject(row.ToString());
            currRow.transform.parent = transform;
            
            float posY = row * tileSize;
            posY = posY - (halfY * tileSize);
            currRow.transform.localPosition = new Vector3(0, posY);
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = (GameObject)Instantiate(refTile, currRow.transform);
                tile.name = col.ToString();

                float posX = col * tileSize;
                posX = posX - (halfX * tileSize);
                tile.transform.localScale = new Vector2(tileSize, tileSize);
                tile.transform.localPosition = new Vector2(posX, 0);

                Square squareScript = tile.gameObject.GetComponent<Square>();
                squareScript.col = col;
                squareScript.row = row;
            }
        }
        PlaceMines();
    }

    private void PlaceMines(){
        while(placedMines < totalMines){
            int chosenRow = Random.Range(0, rows - 1);
            int chosenCol = Random.Range(0, cols - 1);
            Transform row = transform.Find(chosenRow.ToString());
            Transform square = row.Find(chosenCol.ToString());
            Square squareScript = square.gameObject.GetComponent<Square>();
            if(!squareScript.isBomb){
                squareScript.isBomb = true;
                placedMines++;
            }
        }
    }

    public Transform GetTile(int row, int col){
        Transform tRow = transform.Find(row.ToString());
        Transform tile = tRow.Find(col.ToString());
        return tile;
    }

    public void RevealGrid(){
        for (int row = 0; row < rows; row++)
        {
            Transform rowGO = transform.Find(row.ToString());
            for (int col = 0; col < cols; col++)
            {
                Square squareScript = rowGO.Find(col.ToString()).GetComponent<Square>();
                // squareScript.txt.enabled = true;

                if(squareScript.state != Square.State.flagged){
                    squareScript.state = Square.State.revealed;
                }else{
                    if(!squareScript.isBomb){
                        squareScript.sr.sprite = squareScript.sprites[5];
                        // squareScript.txt.enabled = false;
                    }
                }
            }
        }
    }
}