using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level2Boss2 : MonoBehaviour {
    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject orangeSkullMoving;
    public GameObject greySkullMoving;
    private GameObject orangeSkullClone;
    private GameObject greySkullClone;
    //private GameObject[] hit;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;
    private GameObject[] targetListBad;
    private GameObject[] targetList;
    private GameObject[] attackPattern2Targets;

    private List<GameObject> targets = new List<GameObject>();

    //private int numberOfHitMarkers = 4;
    private int badTargetNumber;
    private int targetMovingBadNumber;
    private int attackPattern;
    private int phase;
    private int twoThirdsHealth;
    private int oneThirdHealth;
    private int randomPosition;

    public float timeLeft;

    [SerializeField]
    private float skullSpeed;

    private float skullRadius;
    private float skullWidth;
    private float skullHeight;
    private float phaseTimeLeft;
    private float maxWidthScreen;
    private float maxHeightScreen;

    public Text timerText;
    public Text bossHealthText;
    public Text playerHealthText;
    public Text gameOverText;
    public Text youWinText;

    public Button restartButton;

    private Animator playerAnimation;
    private Animator bossAnimation;

    public static Vector3 bottomLeftScreen;
    public static Vector3 topRightScreen;
    //private Vector3 skullDirection;
    private Vector3 strikePositionTarget;
    private Vector3 strikePositionPlayer;
    private Vector3 upperCornerScreen;
    private Vector3 targetWidth;
    private Vector3 targetHeight;
    private Vector3 spawnPosition;

    private Vector3[] skullDirection = new Vector3[16];
    private Vector3[] position = { new Vector3 { x = -6, y = 4.5f, z = 0 },
                                  new Vector3 { x = -2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 6, y = 4.5f, z = 0 } ,
                                  new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2f, z = 0 } ,
                                  new Vector3 { x = -6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -6, y = -3f, z = 0 },
                                  new Vector3 { x = -2f, y = -3f, z = 0 },
                                  new Vector3 { x = 2f, y = -3f, z = 0 },
                                  new Vector3 { x = 6, y = -3f, z = 0 } };



    private Quaternion spawnRotation;

    public bool audioAlreadyPlayed;

    public AudioSource gameOverAudioSource;

    public AudioClip gameOverAudioClip;

    // Use this for initialization
    void Start() {
        if (cam == null) {
            cam = Camera.main;
        }
        strikePositionTarget = new Vector3(3.8f, -1.4f, 0);
        strikePositionPlayer = new Vector3(-4.64f, -4.24f, 0);
        playerAnimation = player.GetComponent<Animator>();
        bossAnimation = boss.GetComponent<Animator>();
        //hit = new GameObject[numberOfHitMarkers];
        spawnRotation = Quaternion.identity;
        //phase = Random.Range(0,3);
        phase = 0;
        attackPattern = 1;
        oneThirdHealth = boss.GetComponent<EnemyHealthManager>().bossHealth / 3;
        twoThirdsHealth = oneThirdHealth * 2;
        // Set screen boundaries
        upperCornerScreen = new Vector3(Screen.width, Screen.height, 0.0f);
        targetWidth = cam.ScreenToWorldPoint(upperCornerScreen);
        targetHeight = cam.ScreenToWorldPoint(upperCornerScreen);
        //skullWidth = orangeSkullMoving.GetComponent<Renderer>().bounds.extents.x;
        //skullHeight = orangeSkullMoving.GetComponent<Renderer>().bounds.extents.y;
        maxHeightScreen = targetHeight.y - skullHeight;
        maxWidthScreen = targetWidth.x - skullWidth;
        audioAlreadyPlayed = false;
        randomPosition = 0;
        attackPattern2Targets = new GameObject[position.Length];
        bottomLeftScreen = Camera.main.ScreenToWorldPoint((new Vector3(0, 0, 0.0f)));
        topRightScreen = Camera.main.ScreenToWorldPoint((new Vector3(Screen.width, Screen.height, 0.0f)));
        skullRadius = orangeSkullMoving.GetComponent<CircleCollider2D>().radius;

        StartCoroutine(AttackPattern2());
    }

    // Update timers and texts
    private void FixedUpdate() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            timeLeft = 0;
        }
        UpdateText();
        phaseTimeLeft -= Time.deltaTime;
    }

    // Phase 2
    IEnumerator AttackPattern2() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 1) {

            // Phase 1
            if (phase == 0) {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(1f, 2f));

                targetMovingBadNumber = Random.Range(0, 4);

                // If random number is the last number (3) in this case, spawn bad target
                /*
                if (targetMovingBadNumber == 3) {
                    AttackPattern2SpawnBadTargets();
                }
                else {
                    AttackPattern2SpawnGoodTargets();
                }
                */
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
            }

            // Phase 2
            else if (phase == 1) {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // If random number is the last number (3) in this case, spawn bad target
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();

            }

            // Phase 3
            else {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.25f, 5f));

                targetMovingBadNumber = Random.Range(0, 4);

                // If random number is the last number (3) in this case, spawn bad target
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
            }
        }
    }

    // Wait x amount of seconds then destroy skull and deal damage to player
    IEnumerator WaitAndDestroy(GameObject skull) {
        yield return new WaitForSeconds(3);
        if (skull) {
            Destroy(skull);
            BossAttack();
            player.GetComponent<PlayerHealthManager>().giveDamage(1);
        }
    }
    // Update is called once per frame
    void Update() {
        if (boss.GetComponent<EnemyHealthManager>().bossHealth <= 0) {
            boss.GetComponent<EnemyHealthManager>().bossHealth = 0;
            bossAnimation.SetTrigger("death");
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargetsPhase2();
            Invoke("nextBoss", 2);
        }
        if (PlayerHealthManager.playerHealth <= 0) {
            PlayerHealthManager.playerHealth = 0;
            playerAnimation.SetTrigger("death");
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargetsPhase2();
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            if (!audioAlreadyPlayed) {
                Destroy(GameObject.FindWithTag("music"));
                gameOverAudioSource.PlayOneShot(gameOverAudioClip);
                audioAlreadyPlayed = true;
            }
        }
        if (timeLeft <= 0) {
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargetsPhase2();
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            if (!audioAlreadyPlayed) {
                Destroy(GameObject.FindWithTag("music"));
                gameOverAudioSource.PlayOneShot(gameOverAudioClip);
                audioAlreadyPlayed = true;
            }
        }

        // Set Phase 1
        if (boss.GetComponent<EnemyHealthManager>().bossHealth > twoThirdsHealth) {
            phase = 0;
        }

        // Set Phase 2
        else if (boss.GetComponent<EnemyHealthManager>().bossHealth <= twoThirdsHealth
            && boss.GetComponent<EnemyHealthManager>().bossHealth >= oneThirdHealth) {
            if (phase != 1) {
                phase = 1;
                skullSpeed = 7;
            }
        }

        // Set Phase 3
        else {
            if (phase != 2) {
                phase = 2;
                skullSpeed = 9;
            }
        }

        // If targets exist, move them in random directions
        for (int i = 0; i < attackPattern2Targets.Length; i++) {
            if (attackPattern2Targets[i]) {
                attackPattern2Targets[i].transform.Translate(skullDirection[i] * skullSpeed * Time.deltaTime);
                // If skull hits bottom of camera bounce off (the 1.25 int is an offset)
                if (attackPattern2Targets[i].transform.position.y < bottomLeftScreen.y + skullRadius + 1.25 && skullDirection[i].y < 0) {
                    skullDirection[i].y = -skullDirection[i].y;
                    //hit[i].transform.Translate(skullDirection[i] * skullSpeed * Time.deltaTime);
                }
                // If skull hits top of camera bounce off
                if (attackPattern2Targets[i].transform.position.y > topRightScreen.y - skullRadius && skullDirection[i].y > 0) {
                    skullDirection[i].y = -skullDirection[i].y;
                    //hit[i].transform.Translate(skullDirection[i] * skullSpeed * Time.deltaTime);
                }
                // If skull hits left of camera bounce off (.5 int is offset)
                if (attackPattern2Targets[i].transform.position.x < bottomLeftScreen.x + skullRadius + .5 && skullDirection[i].x < 0) {
                    skullDirection[i].x = -skullDirection[i].x;
                    //hit[i].transform.Translate(skullDirection[i] * skullSpeed * Time.deltaTime);
                }
                // If skull hits right of camera bounce off (-.75 is offset)
                if (attackPattern2Targets[i].transform.position.x > topRightScreen.x - skullRadius - .5 && skullDirection[i].x > 0) {
                    skullDirection[i].x = -skullDirection[i].x;
                    //hit[i].transform.Translate(skullDirection[i] * skullSpeed * Time.deltaTime);
                }
            }
        }

        if (!MainMenu.phoneMode) {
            // Mouse click for all of phase2
            OnClickDoSomethingAttackPattern2();
        }
        else {
            OnTouchDoSomethingAttackPattern2();
        }
    }

    // Animate Boss Attack
    void BossAttack() {
        bossAnimation.SetTrigger("skill_1");
        playerAnimation.SetTrigger("hit_1");
        strikePlayerClone = Instantiate(strikePlayer, strikePositionPlayer, Quaternion.identity);
        strikePlayerClone.transform.position = strikePositionPlayer;
        Destroy(strikePlayerClone, 1f);
    }

    // Animate Player Attack
    void PlayerAttack() {
        playerAnimation.SetTrigger("skill_1");
        bossAnimation.SetTrigger("hit_1");
        strikeTargetClone = Instantiate(strikeTarget, strikePositionTarget, Quaternion.identity);
        strikeTargetClone.transform.position = strikePositionTarget;
        Destroy(strikeTargetClone, 0.1f);
    }

    // Spawn phase 2 bad targets
    private void AttackPattern2SpawnBadTargets() {
        /*
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        Random.Range(-maxHeightScreen, maxHeightScreen),
                        0.0f
                        );
        greySkullClone = Instantiate(greySkull, spawnPosition, spawnRotation);
        greySkullClone.transform.position = spawnPosition;
        targets.Add(greySkullClone);
        Destroy(greySkullClone, 3);
        */
        if (!attackPattern2Targets[randomPosition]) {
            greySkullClone = Instantiate(greySkullMoving, position[randomPosition], spawnRotation);
            greySkullClone.transform.position = position[randomPosition];
            attackPattern2Targets[randomPosition] = greySkullClone;
            // Create a moving target
            skullDirection[randomPosition] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);

            attackPattern2Targets[randomPosition] = greySkullClone;

            Destroy(greySkullClone, 5);
        }
        else {
            randomPosition = Random.Range(0, position.Length);
        }
    }

    // Spawn attack 2 good targets
    private void AttackPattern2SpawnGoodTargets() {
        /*
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        Random.Range(-maxHeightScreen, maxHeightScreen),
                        0.0f
                        );
                        */
        randomPosition = Random.Range(0, position.Length);
        if (!attackPattern2Targets[randomPosition]) {
            orangeSkullClone = Instantiate(orangeSkullMoving, position[randomPosition], spawnRotation);
            orangeSkullClone.transform.position = position[randomPosition];
            // Create a moving target
            skullDirection[randomPosition] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);

            attackPattern2Targets[randomPosition] = orangeSkullClone;
            //Destroy(orangeSkullClone, 3);
            StartCoroutine(WaitAndDestroy(attackPattern2Targets[randomPosition]));
        }
        else {
            randomPosition = Random.Range(0, position.Length);
        }
        /*
        orangeSkullClone = Instantiate(orangeSkull, spawnPosition, spawnRotation);
        orangeSkullClone.transform.position = spawnPosition;
        targets.Add(orangeSkullClone);
        Destroy(orangeSkullClone, 3);
        */
    }

    // Go to next boss
    private void nextBoss() {
        SceneManager.LoadScene("EPB_level2_boss3");
    }

    // Destroy All Targets
    private void DestroyAllTargetsPhase2() {
        targetListBad = GameObject.FindGameObjectsWithTag("grey_skull");
        targetList = GameObject.FindGameObjectsWithTag("orange_skull");

        for (int i = 0; i < targetList.Length; i++) {
            Destroy(targetList[i]);
        }
        for (int i = 0; i < targetListBad.Length; i++) {
            Destroy(targetListBad[i]);
        }
    }

    // On PHONE TOUCH do something
    private void OnTouchDoSomethingAttackPattern2()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {
            // On click, do something
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position),
                    Vector2.zero);
                // If target is clicked, destroy target
                if (hitTarget.collider != null)
                {
                    // If click good target, damage boss
                    if (hitTarget.collider.gameObject.tag == "orange_skull_moving")
                    {
                        Destroy(hitTarget.collider.gameObject);
                        PlayerAttack();
                        boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                    }
                    // If click bad target, damage player
                    else if ((hitTarget.collider.gameObject.tag == "grey_skull_moving"))
                    {
                        Destroy(hitTarget.collider.gameObject);
                        BossAttack();
                        player.GetComponent<PlayerHealthManager>().giveDamage(1);
                    }
                }
            }
        }
    }

    
    // On MOUSE CLICK do something
    private void OnClickDoSomethingAttackPattern2()
    {
        // On click, do something
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);
            // If target is clicked, destroy target
            if (hitTarget.collider != null)
            {
                // If click good target, damage boss
                if (hitTarget.collider.gameObject.tag == "orange_skull_moving")
                {
                    Destroy(hitTarget.collider.gameObject);
                    PlayerAttack();
                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                }
                // If click bad target, damage player
                else if ((hitTarget.collider.gameObject.tag == "grey_skull_moving"))
                {
                    Destroy(hitTarget.collider.gameObject);
                    BossAttack();
                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                }
            }
        }
    }
    

    /*
    // If target reaches bottom of stage, damage player
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "phase2_falling_bad") {
            player.GetComponent<PlayerHealthManager>().giveDamage(1);
            BossAttack();
        }
        Destroy(other.gameObject);
    }
    */

    // Update scores and text
    void UpdateText() {
        timerText.text = "Time Left:\n" + Mathf.RoundToInt(timeLeft);
        bossHealthText.text = "Boss Health:\n" + Mathf.RoundToInt(boss.GetComponent<EnemyHealthManager>().bossHealth);
        playerHealthText.text = "Player Health: \n" + Mathf.RoundToInt(PlayerHealthManager.playerHealth);
    }
}
