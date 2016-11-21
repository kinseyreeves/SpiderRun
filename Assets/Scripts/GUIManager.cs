using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GUIManager : MonoBehaviour {

    SceneManager sceneManager;

    public Button changeMode;
    public Text staminaText;
    public Text scoreText;
    public Text healthText;
    public Text phaseText;
    public Text startPhaseText;
    public Text noEnemiesText;
    public Text gameOverText;
    public Text gameScoreText;

    public GameObject player;
    public PlayerManager playerManager;
    public EnemyManager enemyManager;
    

    public bool gameOver = false;
    public float gameOverTimer;

    //build buttons
    public List<Button> buildButtons;


    public PlayerManager playerScript;
    public TerrainEditor terrainScript;

    List<Phase> phases = new List<Phase>();
    

    private Phase currPhase;

    public int phaseTimer;
    int phaseNo;
    int maxPhase;

    public int gameScore;

    public enum gameMode {Player, Terrain, Build}
    public enum Placeable { PoisonBush, FireTree, RockTurret }
    gameMode currMode;


	// Use this for initialization
	void Start () {
        gameScore = 0;
        gameOver = false;
        gameOverText.enabled = false;

        showBuildOptions(true);
        //i want to load all this from a text file of enemy data



        phases.Add(new Phase(1, 50, new enemyData(3, 0, 0, 30)));
        phases.Add(new Phase(2, 50, new enemyData(5, 2, 0, 50)));
        phases.Add(new Phase(3, 50, new enemyData(5, 4, 0, 50)));
        phases.Add(new Phase(4, 20, new enemyData(18, 2, 0, 50)));
        phases.Add(new Phase(5, 20, new enemyData(5, 5, 2, 80)));
        phases.Add(new Phase(6, 20, new enemyData(17, 6, 3, 80)));
        phases.Add(new Phase(7, 20, new enemyData(30, 10, 2, 80)));
        phases.Add(new Phase(8, 20, new enemyData(25, 0, 2, 80)));
        phases.Add(new Phase(9, 20, new enemyData(5, 5, 5, 80)));
        phases.Add(new Phase(10, 20, new enemyData(35, 0, 10, 80)));
        phases.Add(new Phase(10, 20, new enemyData(35, 0, 10, 80)));


        //list of all the build buttons

        phaseNo = 0;
        maxPhase = phases.Count;
        currPhase = phases[0];
        changeMode.onClick.AddListener(() => { onClickChangeMode(); });
        currMode = gameMode.Build;
        changeMode.GetComponentInChildren<Text>().text = getNextMode();

       
        buildButtons[0].onClick.AddListener(() => { playerManager.buildPoisonBush(); });
        buildButtons[1].onClick.AddListener(() => { playerManager.buildFireTree(); });
        buildButtons[2].onClick.AddListener(() => { playerManager.throwRollingBall(); });
        buildButtons[3].onClick.AddListener(() => { playerManager.createASCannon(); });

    }
    // Update is called once per frame
    void Update () {
        
        scoreText.text = "Form points: " + playerScript.points;
        healthText.text = "Health : " + (int)playerScript.hp/10;
        phaseText.text = "Phase : " + currPhase.level;
        noEnemiesText.text = "Number of enemies: " + currPhase.totalEnemies;
        gameScoreText.text = "Score: " + gameScore;
        staminaText.text = "Stamina : " + ((playerScript.stamina - 0.1) / 10).ToString("F0") + "%";


        

        if(playerScript.hp <= 0 || gameOver)
        {
            endGame();
        }
        
        if(enemyManager.enemyCount == 0) {
            if (phaseNo < maxPhase)
                currPhase = phases[phaseNo++];
            else
                displayWinMessage();
            beginPhase(currPhase);
            
        }

    }

    private void displayWinMessage()
    {
        throw new NotImplementedException();
    }

    void onClickChangeMode()
    {
        changeMode.GetComponentInChildren<Text>().text = getNextMode();
    }


    //cycles between the game modes on button clicks
    private string getNextMode()
    {
        if(currMode == gameMode.Player)
        {
            currMode = gameMode.Terrain;
            playerScript.isModeActive = false;
            terrainScript.isModeActive = true;
            return "Terrain";
        }

        currMode = gameMode.Player;
        playerScript.isModeActive = true;
        terrainScript.isModeActive = false;
        return "Player";
    }

    private void showBuildOptions(bool active)
    {
        
        foreach (Button b in buildButtons)
        {
            b.gameObject.SetActive(active);
        }
        
       
    }

    public void endGame()
    {
        gameOver = true;
        gameOverText.text = "Game Over";
        gameOverText.enabled = true;
        if (gameOverTimer > 0) {
            gameOverTimer -= Time.deltaTime;
        }else {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void beginPhase(Phase phase)
    {
        displayStart(phase.level);
        enemyManager.createEnemies(phase.enemy);
        playerManager.addScore(phase.startPoints);
        gameScore += 50;
    }


    

    public void displayStart(int phase)
    {
        
        startPhaseText.text = "Phase " + phase + ".. Get ready!";
        Invoke("HideText", phaseTimer);
        
    }

    public void HideText()
    {
        startPhaseText.text = "";
    }

    public void enemyDeath()
    {
        currPhase.totalEnemies -= 1;
        enemyManager.enemyCount -= 1;

    }

    
}



public class Phase
{
    public int level;
    public int totalEnemies;
    public int startPoints;
    public enemyData enemy;



    public Phase(int level, int startPoints, enemyData enemy)
    {
        this.level = level;
        this.startPoints = startPoints;
        this.enemy = enemy;
        this.totalEnemies = enemy.total;
    }


}

public struct enemyData
{

    public int easy;
    public int medium;
    public int hard;
    public int total;
    public int spawnRange;

    public enemyData(int easy, int medium, int hard, int spawnRange)
    {
        this.easy = easy;
        this.medium = medium;
        this.hard = hard;
        this.total = easy + medium + hard;
        this.spawnRange = spawnRange;
    }
}

