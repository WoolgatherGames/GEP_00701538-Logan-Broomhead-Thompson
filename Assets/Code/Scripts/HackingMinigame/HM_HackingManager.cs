using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HM_HackingManager : MonoBehaviour
{
    public static HM_HackingManager instance;
    private void Awake()
    {
        instance = this;
    }


    HM_HackableObject _currentHackingObject;

    [Header("Keys")]
    [SerializeField] HM_KeyboardKey[] keyboardKeys;

    [Header("Pop Ups")]
    [Tooltip("What the pop ups spawn into")]
    [SerializeField] GameObject popUpParent;
    [SerializeField] Vector2 popUpBoundsX;
    [SerializeField] Vector2 popUpBoundsY;
    [Tooltip("Possible pop ups that might spawn")]
    [SerializeField] GameObject[] popUpPrefabs;

    [Header("UI")]
    [SerializeField] GameObject HackingParent;
    [Tooltip("slider that visually represents the progress of the hack")]
    [SerializeField] Image progressSlider;
    [Tooltip("Text that shows what percentage the player has completed of the current hack")]
    [SerializeField] Text progressText;
    [Tooltip("Text that shows how long remains for the player to hack the device")]
    [SerializeField] Text timeLimitText;

    [Header("Balancing Values")]
    [Tooltip("How much progress is lost per second")]
    [SerializeField] int baseDecayRate = 1;

    Vector2 timeBetweenKeys = new Vector2(0.1f, 0.3f);
    Vector2 timeKeysStayActive = new Vector2(0.5f, 0.8f);

    private Vector2Int _progressRate;
    private float _timeLimit;
    private Vector2 _timeBetweenPopUps;
    private int _popUpDecayRate;

    [SerializeField] Difficulty testDifficulty;
    private void Start()
    {
        //testing only
        //BeginHack(testDifficulty);
    }


    private int progress;
    private float currentTimeBetweenKeySpawns;
    private float keySpawnTimer;
    private float popUpSpawnTimer;
    int numberOfPopUpsActive;
    private bool hacking = false;

    float progressDecayTimer;

    public void BeginHack(Difficulty difficulty)
    {
        _progressRate = difficulty.myProgressRate;
        _timeLimit = difficulty.myTimeLimit;
        _timeBetweenPopUps = difficulty.myTimeBetweenPopUps;
        _popUpDecayRate = difficulty.myPopUpDecay;


        HackingParent.SetActive(true);


        progress = 0;
        progressDecayTimer = 1f;
        UpdateProgress(0);

        SetNextPopUpSpawnTimer();
        SetNextKeySpawnTimer();

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
            //enabling the keys on a timer
            keySpawnTimer += Time.deltaTime;
            if (keySpawnTimer > currentTimeBetweenKeySpawns)
            {
                SpawnKey();
                SetNextKeySpawnTimer();
            }

            //decrease the players progress every second based on the number of pop ups (but before a pop up is spawned this frame)
            progressDecayTimer -= Time.deltaTime;
            if (progressDecayTimer < 0f)
            {
                int amount = baseDecayRate + (_popUpDecayRate * numberOfPopUpsActive);
                progressDecayTimer += 1f;
                UpdateProgress(-amount);
            }

            //spawning pop ups
            popUpSpawnTimer -= Time.deltaTime;
            if (popUpSpawnTimer < 0f)
            {
                SpawnPopUp();
                SetNextPopUpSpawnTimer();
            }

        }
    }

    void SpawnKey()
    {
        int i = Random.Range(0, keyboardKeys.Length);
        float time = Random.Range(timeKeysStayActive.x, timeKeysStayActive.y);
        keyboardKeys[i].ActivateKey(time);
    }
    void SpawnPopUp()
    {
        float xPos = Random.Range(popUpBoundsX.x, popUpBoundsX.y);
        float yPos = Random.Range(popUpBoundsY.x, popUpBoundsY.y);

        Vector3 spawnPos = new Vector3(xPos, yPos, 0f);

        int popUpNumber = Random.Range(0, popUpPrefabs.Length);

        Instantiate(popUpPrefabs[popUpNumber], spawnPos, Quaternion.identity, popUpParent.transform);

        numberOfPopUpsActive += 1;
    }
    public void PopUpDestroyed()
    {
        numberOfPopUpsActive -= 1;
        Debug.Log("Pop ups active: " + numberOfPopUpsActive);
    }


    void SetNextKeySpawnTimer()
    {
        //decide how long till the next key spawns, and set the keyspawn timer to 0. 
        currentTimeBetweenKeySpawns = Random.Range(timeBetweenKeys.x, timeBetweenKeys.y);
        keySpawnTimer = 0f;
    }
    void SetNextPopUpSpawnTimer()
    {
        popUpSpawnTimer = Random.Range(_timeBetweenPopUps.x, _timeBetweenPopUps.y);
    }

    public void KeyPressed()
    {
        //The Keyboard Key script calls this when its button has been pressed and its active. When this happens, the keyboard key also disables itself.

        UpdateProgress(Random.Range(_progressRate.x, _progressRate.y));
    }
}
