using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;        //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    //Store a reference to our BoardManager which will set up the level.
    private TilemapSetup tilemapSetup;
    //Current level number, expressed in game as "Day  1".
    private int level = 1;
    public static event Action<int> beatLevel;

    public int numOfEnemy = 1;
    [SerializeField] private GameObject winScreen;

    public bool easyMode = false;
    public Toggle easyModeToggle;




    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        //boardScript = GetComponent<BoardManager>();
        //tilemapSetup = GetComponent<TilemapSetup>();


    }
    void Start()
    {
        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    public int GetCurrentLevel()
    {
        return level;
    }

    public void LevelCompleted()
    {
        level++;
        numOfEnemy = 1;
        if (level > 4)
        {
            Debug.Log("Game is over! You won!");
            winScreen.SetActive(true);
        }
        else
        {
            TilemapSetup.Instance.NewLevel(level);
            beatLevel?.Invoke(level);
        }
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        //boardScript.SetupScene(level);
        TilemapSetup.Instance.NewLevel(level);
    }


    public void ToggleEasyMode()
    {
        easyMode = easyModeToggle.isOn;
    }
}