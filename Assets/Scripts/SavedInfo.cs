using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SavedInfo : MonoBehaviour
{
    public static SavedInfo self;
    public int correctFlags;
    public int totalBombs;
    public int availableFlags;
    public int hiddenTiles;
    public int gridX;
    public int gridY;
    public int gridBombs;
    public int outside;
    public bool canDrag;
    public bool dragging;
    public Vector2 maxPos;
    public int board = 0;
    public bool canPlay = true;
    public string currScene;
    public int score;
    public int maxScore;
    public int time;
    public GameObject winScreen;
    public GameObject loseScreen;
    public bool animateFace;

    void Start()
    {
        if(self == null){
            self = this;
        }else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        // if(correctFlags == totalBombs && hiddenTiles <= 0){
        //     Debug.Log("You Won!");
        // }
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene("Board");
        }
        if(outside >= 0){
            canDrag = false;
        }
        if(currScene != SceneManager.GetActiveScene().name){
            NewScene();
        }
    }

    public void NewScene(){
        currScene = SceneManager.GetActiveScene().name;
    }

    public void StartLevel(){
        SceneManager.LoadScene("Board");
    }

    public void BackToMain(){
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleFace(bool value){
        animateFace = value;
    }
}
