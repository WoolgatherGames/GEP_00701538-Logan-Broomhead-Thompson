using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HM_HackingManager : MonoBehaviour
{

    public static HM_HackingManager instance;//no need to bother with the singleton method that creates this into a game object, since if the values for this script arnt set in the inspector, it wont work
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There are too many copies of HM_HackingManager in the scene, my game object is called: " + this.gameObject.name);
        }
        else
        {
            instance = this;
        }
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

    private float hackTimer;

    [Header("Animation Components")]
    [SerializeField] Animator startEndHackAnimator;
    [SerializeField] Text startEndAnimatedText;
    [SerializeField] ParticleSystem hackCompleteConfetii;

    [SerializeField] Difficulty testDifficulty;
    private void Start()
    {
        startEndAnimatedText.gameObject.SetActive(false);
    }

    //i dont want progress to be updated without also updating the ui, so call UpdateProgress(int) instead
    private int progress;
    //the timer limit for when the next key spawns (prompts the player to press a specific button) currently its randomised between 0.1 and 0.3 seconds every key spawn
    private float currentTimeBetweenKeySpawns;
    private float keySpawnTimer;
    private float popUpSpawnTimer;
    //to do: consider adding a maximum number of pop ups. if a player went afk they could impact performance 
    int numberOfPopUpsActive;
    private bool hacking = false;

    float progressDecayTimer;

    Difficulty currentDifficulty;
    HM_HackableObject currentHackTarget;

    HM_HackableObject currentObjectBeingHacked;
    public void BeginHack(Difficulty difficulty, HM_HackableObject objectBeingHacked)
    {
        hacking = false;//just incase a previous hack is active, we need to reset it! this will stop the hack from continuing. 

        progress = 0;
        hackTimer = 0f;
        currentDifficulty = difficulty;
        currentHackTarget = objectBeingHacked;

        timeLimitText.text = difficulty.myTimeLimit.ToString().Substring(0, 3);

        BeginHackPartOne();
    }

    void BeginHackPartOne()
    {
        //Debug.Log("Animation begin");
        startEndAnimatedText.gameObject.SetActive(true);
        startEndAnimatedText.text = "Begin Hack";
        startEndHackAnimator.SetTrigger("TriggerTextAnim");

        //BeginHackPartTwo();
        StartCoroutine(WaitBetweenBeginHackParts());

    }
    IEnumerator WaitBetweenBeginHackParts()
    {
        yield return new WaitForSeconds(1f);//make sure this as about as long as the animation takes place or slightly 
        startEndAnimatedText.gameObject.SetActive(false);
        BeginHackPartTwo();
    }

    //void BeginHackPartTwo(Difficulty difficulty, HM_HackableObject objectBeingHacked)
    void BeginHackPartTwo()
    {
        _progressRate = currentDifficulty.myProgressRate;
        _timeLimit = currentDifficulty.myTimeLimit;
        _timeBetweenPopUps = currentDifficulty.myTimeBetweenPopUps;
        _popUpDecayRate = currentDifficulty.myPopUpDecay;


        HackingParent.SetActive(true);


        progress = 0;
        progressDecayTimer = 1f;
        UpdateProgress(0);

        SetNextPopUpSpawnTimer();
        SetNextKeySpawnTimer();

        hacking = true;
        Debug.Log("Start hack");
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
        if (hacking == false)
            return;
        
        hacking = false;

        currentHackTarget.HackCompleted();
        currentDifficulty = null;
        currentHackTarget = null;

        //delete ALL pop ups that are still active
        foreach (Transform child in popUpParent.transform)
        {
            Destroy(child.gameObject);
        }

        startEndAnimatedText.gameObject.SetActive(true);
        startEndAnimatedText.text = "Hack Succesful";
        startEndHackAnimator.SetTrigger("TriggerTextAnim");
        hackCompleteConfetii.Play();

        return;
        //when the hack is complete, do stuff. celebration screen, tell the object its hacked now. ect ect. 
    }

    void CancelHack()
    {
        //add in a button that lets the player cancel the hack. or the escape key does it
        hacking = false;
        currentDifficulty = null;
        currentHackTarget = null;
    }

    private void Update()
    {
        if (hacking)
        {
            hackTimer += Time.deltaTime;
            timeLimitText.text = (_timeLimit - hackTimer).ToString().Substring(0, 3);
            if (hackTimer > _timeLimit)
            {
                //you've run out of time. OAEF
            }


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
