using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prototype_HackingManager : MonoBehaviour
{
    public static Prototype_HackingManager instance;
    private void Awake()
    {
        instance = this;
    }


    Prototype_HackableObject _currentHackingObject;

    [Header("Keys")]
    [SerializeField] Prototype_KeyboardKey[] keyboardKeys;

    [Header("UI")]
    [SerializeField] GameObject HackingParent;
    [Tooltip("slider that visually represents the progress of the hack")]
    [SerializeField] Image progressSlider;
    [Tooltip("Text that shows what percentage the player has completed of the current hack")]
    [SerializeField] Text progressText;
    [Tooltip("Text that shows how long remains for the player to hack the device")]
    [SerializeField] Text timeLimitText;

    [Header("Balancing Values")]
    [Tooltip("How much progress does each key give the player on click? Amount is randomly determined between the two numbers")]
    [SerializeField] Vector2Int easyDifficultProgressRate = new Vector2Int(7, 15);
    [Tooltip("How long does the player have to solve an easy hack. Note: On average 5 keys appear per second")]
    [SerializeField] float easyDifficultyTimeLimit = 10f;
    [Space(5)]
    [SerializeField] Vector2Int mediumDifficultProgressRate = new Vector2Int(6, 12);
    [SerializeField] float mediumDifficultyTimeLimit = 5f;
    [Space(5)]
    [SerializeField] Vector2Int hardDifficultProgressRate = new Vector2Int(5, 8);
    [SerializeField] float hardDifficultyTimeLimit = 5f;
    Vector2 timeBetweenKeys = new Vector2(0.1f, 0.3f);
    Vector2 timeKeysStayActive = new Vector2(0.3f, 0.5f);

    public enum difficulty
    {
        easy,
        medium,
        hard,
    }
    private difficulty currentDifficulty = difficulty.easy;

    private void Start()
    {
        //testing only
        BeginHack(difficulty.easy);
    }


    private int progress;
    private float currentTimeBetweenKeySpawns;
    private float keySpawnTimer;
    private bool hacking = false;

    void BeginHack(difficulty hackDifficulty)
    {
        currentDifficulty = hackDifficulty;
        HackingParent.SetActive(true);


        //set the timer

        progress = 0;
        UpdateProgress(0);

        SetNextSpawnTimer();
        hacking = true;
    }

    void UpdateProgress(int progressIncrease)
    {
        //call this function every time the progres changes. It will change the progress for you, and then update the UI
        progress += progressIncrease;
        if (progress < 0)
            progress = 0;
        if (progress >= 100)
        {
            progress = 100;
            HackComplete();
        }

        progressSlider.fillAmount = progress / 100f;
        progressText.text = (progress.ToString() + "%");
    }

    void HackComplete()
    {
        return;
        //when the hack is complete, do stuff. 
    }

    private void Update()
    {
        if (hacking)
        {
            keySpawnTimer += Time.deltaTime;
            if (keySpawnTimer > currentTimeBetweenKeySpawns)
            {
                SpawnKey();
                SetNextSpawnTimer();
            }
        }
    }

    void SpawnKey()
    {
        int i = Random.Range(0, keyboardKeys.Length);
        float time = Random.Range(timeKeysStayActive.x, timeKeysStayActive.y);
        keyboardKeys[i].ActivateKey(time);
    }
    void SetNextSpawnTimer()
    {
        //decide how long till the next key spawns, and set the keyspawn timer to 0. 
        currentTimeBetweenKeySpawns = Random.Range(timeBetweenKeys.x, timeBetweenKeys.y);
        keySpawnTimer = 0f;
    }

    public void KeyPressed()
    {
        //The Keyboard Key script calls this when its button has been pressed and its active. When this happens, the keyboard key also disables itself.
        if (currentDifficulty == difficulty.easy)
        {
            UpdateProgress(Random.Range(easyDifficultProgressRate.x, easyDifficultProgressRate.y + 1));
        }
        if (currentDifficulty == difficulty.medium)
        {
            UpdateProgress(Random.Range(mediumDifficultProgressRate.x, mediumDifficultProgressRate.y + 1));
        }
        if (currentDifficulty == difficulty.hard)
        {
            UpdateProgress(Random.Range(hardDifficultProgressRate.x, hardDifficultProgressRate.y + 1));
        }
    }
}
