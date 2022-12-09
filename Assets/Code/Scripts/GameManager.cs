using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //put the game manager in the STEALTH scene. it should spawn in the hacking scene. 
    //game manager is responsible for: toggling if the player can move or not. spawning in the hacking scene. managing the players win and lose states
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("theres already a game manager in the scene");
        }
        else
        {
            Screen.SetResolution(1920, 1080, true);
            SceneManager.LoadScene("HackingScreen", LoadSceneMode.Additive);
            instance = this;
        }

    }

    [SerializeField] PC_PlayerMovement playerController;

    //scoring
    private float playerTime;//how long it took the player to beat the level
    private float playerScore;//player should gain score for hacking objects, and for how long it took them to complete the level


    private int cardsCollected;
    [SerializeField] int totalCardsRequiredToCompleteLevel;
    
    [Tooltip("The exit door should be a collider the player cant get past. it should NOT be the object with SM_EscapeDoor attatched to it, as the player moves past the (now opened) door to reach SM_EscapeDoors collider")]
    [SerializeField] private GameObject ExitDoorSpriteAndCollider;

    [Tooltip("The UI panel that is displayed once the player completes the level!")]
    [SerializeField] private GameObject EndLevelPanel;
    [SerializeField] private Text playerTimerText;
    [SerializeField] private Text playerScoreText;

    [SerializeField] private GameObject GameOverPanel;

    [SerializeField] Vector3 playerStartingLocation;
    private Vector3 playerRespawnLocation;

    private void Start()
    {
        ExitDoorSpriteAndCollider.SetActive(true);
        EndLevelPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        EnablePlayerMovement();

        playerTime = 0f;
        playerRespawnLocation = playerStartingLocation;
    }

    private void Update()
    {
        playerTime += Time.deltaTime;
    }

    public void AddCard()
    {
        //called by the card Caches before they self destruct
        cardsCollected += 1;
        if (cardsCollected == totalCardsRequiredToCompleteLevel)
        {
            //LevelComplete(); //dont end the level here. make the player go back to the start
            ExitDoorSpriteAndCollider.SetActive(false);
        }
    }

    public void BeginHack()
    {
        //called by the hacking manager
        DisablePlayerMovement();
    }
    public void EndHack(float scoreValue)
    {
        //called by the hacking manager
        playerScore += scoreValue;
        EnablePlayerMovement();
    }

    void DisablePlayerMovement()
    {
        playerController.DisableMovement();
    }
    void EnablePlayerMovement()
    {
        playerController.EnableMovement();
    }

    public void LevelComplete()
    {
        //called by the SM_EscapeDoor script when the player touches it and has all the items. It should only be accessible when the player has all the items but just incase check
        //display the players results. show a level end screen
        if (cardsCollected >= totalCardsRequiredToCompleteLevel)
        {
            Debug.Log("LEVEL COMPLETE");
            playerController.DisableMovement();//stop the player from moving, the level is over. 


            EndLevelPanel.SetActive(true);
            string[] displayScore = playerScore.ToString().Split('.');
            playerScoreText.text = "Score: " + displayScore[0];
            string[] displayTime = playerTime.ToString().Split('.');
            playerTimerText.text = "Time: " + displayTime[0] + "s";
        }
    }

    public void GameOver()
    {
        //called by the detection field if they player gets spotted. 
        playerController.DisableMovement();//stop the player from moving about
        HM_HackingManager.instance.GameOver();//stop any hacks that are currently happening from continuing (otherwise the player could complete the hack for free, and then press respawn)
        GameOverPanel.SetActive(true);
    }
    public void Restart()
    {
        //called by the restart button in the game over panel
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void RespawnPlayer()
    {
        //respawn the player from the last save location they where at, take some points away
        //the gameplay doesnt suit short levels well. so restarting a whole level is...not great. a checkpoint system works far better. 
        playerScore -= 5f;
        playerController.transform.position = playerRespawnLocation;

        StartCoroutine(RespawnPlayerWaitFrame());//cant respawn this frame because the detection field would just call "game over" again

    }
    IEnumerator RespawnPlayerWaitFrame()
    {
        yield return null;//wait a single frame because the players position needs to be reset so the detection field script doesnt immediatly game over you again. 
        GameOverPanel.SetActive(false);
        EnablePlayerMovement();
    }

    public void SetRespawnLocation(Vector3 spawnPosition)
    {
        //place respawn locations in different parts of the map where i want the player to return to if they ever get caught. 
        playerRespawnLocation = spawnPosition;
    }
}
