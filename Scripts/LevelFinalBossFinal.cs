using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelFinalBossFinal : MonoBehaviour {
    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject blueSkull;
    public GameObject greySkull;
    private GameObject blueSkullClone;
    private GameObject greySkullClone;
    //private GameObject[] hit;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;
    private GameObject[] targetListBad;
    private GameObject[] targetList;
    private GameObject[] attackPattern1Targets;

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
    private float phaseTimeLeft;
    private float maxWidthScreen;
    private float maxHeightScreen;
    private float skullWidth;
    private float skullHeight;

    public Text timerText;
    public Text bossHealthText;
    public Text playerHealthText;
    public Text gameOverText;
    public Text youWinText;

    public Button restartButton;

    private Animator playerAnimation;
    private Animator bossAnimation;

    private Vector3 strikePositionTarget;
    private Vector3 strikePositionPlayer;
    private Vector3 upperCornerScreen;
    private Vector3 targetWidth;
    private Vector3 targetHeight;
    private Vector3 spawnPosition;

    private Vector3[] attackPattern1Positions = { new Vector3 { x = -6, y = 4.5f, z = 0 },
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
        skullWidth = blueSkull.GetComponent<Renderer>().bounds.extents.x;
        skullHeight = blueSkull.GetComponent<Renderer>().bounds.extents.y;
        maxHeightScreen = targetHeight.y - skullHeight;
        maxWidthScreen = targetWidth.x - skullWidth;
        audioAlreadyPlayed = false;
        randomPosition = 0;
        attackPattern1Targets = new GameObject[attackPattern1Positions.Length];

        StartCoroutine(AttackPattern1());
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

    // AttackPattern 1
    IEnumerator AttackPattern1() {
        yield return new WaitForSeconds(0.37037037f);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 1) {

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
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
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
            }
        }

        // Set Phase 3
        else {
            if (phase != 2) {
                phase = 2;
            }
        }

        // Mouse click for all of phase1
        OnClickDoSomethingAttackPattern1();
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
    private void AttackPattern1SpawnBadTargets() {
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
        if (!attackPattern1Targets[randomPosition]) {
            greySkullClone = Instantiate(greySkull, attackPattern1Positions[randomPosition], spawnRotation);
            greySkullClone.transform.position = attackPattern1Positions[randomPosition];
            attackPattern1Targets[randomPosition] = greySkullClone;
            Destroy(greySkullClone, 3);
        }
        else {
            randomPosition = Random.Range(0, attackPattern1Positions.Length);
        }
    }

    // Spawn phase 2 good targets
    private void AttackPattern1SpawnGoodTargets() {
        /*
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        Random.Range(-maxHeightScreen, maxHeightScreen),
                        0.0f
                        );
                        */
        randomPosition = Random.Range(0, attackPattern1Positions.Length);
        if (!attackPattern1Targets[randomPosition]) {
            blueSkullClone = Instantiate(blueSkull, attackPattern1Positions[randomPosition], spawnRotation);
            blueSkullClone.transform.position = attackPattern1Positions[randomPosition];
            attackPattern1Targets[randomPosition] = blueSkullClone;
            //Destroy(orangeSkullClone, 3);
            StartCoroutine(WaitAndDestroy(attackPattern1Targets[randomPosition]));
        }
        else {
            randomPosition = Random.Range(0, attackPattern1Positions.Length);
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
        SceneManager.LoadScene("EPB_level2_boss2");
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

    /*
    // On click do something
    private void OnClickDoSomethingAttackPattern1() {
        // On click, do something
        for (int i = 0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position),
                    Vector2.zero);
                // If target is clicked, destroy target
                if (hitTarget.collider != null) {
                    // If click good target, damage boss
                    if (hitTarget.collider.gameObject.tag == "orange_skull") {
                        Destroy(hitTarget.collider.gameObject);
                        PlayerAttack();
                        boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                    }
                    // If click bad target, damage player
                    else if ((hitTarget.collider.gameObject.tag == "grey_skull")) {
                        Destroy(hitTarget.collider.gameObject);
                        BossAttack();
                        player.GetComponent<PlayerHealthManager>().giveDamage(1);
                    }
                }
            }
        }
    }
    */

    private void OnClickDoSomethingAttackPattern1()
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
                if (hitTarget.collider.gameObject.tag == "blue_skull")
                {
                    Destroy(hitTarget.collider.gameObject);
                    PlayerAttack();
                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                }
                // If click bad target, damage player
                else if ((hitTarget.collider.gameObject.tag == "grey_skull"))
                {
                    Destroy(hitTarget.collider.gameObject);
                    BossAttack();
                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                }
            }
        }
    }


    // If target reaches bottom of stage, damage player
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "phase2_falling_bad") {
            player.GetComponent<PlayerHealthManager>().giveDamage(1);
            BossAttack();
        }
        Destroy(other.gameObject);
    }

    // Update scores and text
    void UpdateText() {
        timerText.text = "Time Left:\n" + Mathf.RoundToInt(timeLeft);
        bossHealthText.text = "Boss Health:\n" + Mathf.RoundToInt(boss.GetComponent<EnemyHealthManager>().bossHealth);
        playerHealthText.text = "Player Health: \n" + Mathf.RoundToInt(PlayerHealthManager.playerHealth);
    }
}
