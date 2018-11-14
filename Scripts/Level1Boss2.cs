using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level1Boss2 : MonoBehaviour {
    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject target;
    public GameObject targetBad;
    public GameObject targetFalling;
    public GameObject targetFallingBad;
    public GameObject targetLocked;
    private GameObject targetFallingClone;
    private GameObject targetFallingBadClone;
    //private GameObject[] hit;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;
    private GameObject[] targetListBad;
    private GameObject[] targetList;

    //private int numberOfHitMarkers = 4;
    private int badTargetNumber;
    private int targetMovingBadNumber;
    private int attackPattern;
    private int phase;
    private int twoThirdsHealth;
    private int oneThirdHealth;

    public float timeLeft;
    private float phaseTimeLeft;
    private float maxWidthScreen;
    private float skullWidth;

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
    private Vector3 spawnPosition;

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
        skullWidth = target.GetComponent<Renderer>().bounds.extents.x;
        maxWidthScreen = targetWidth.x - skullWidth; ;
        audioAlreadyPlayed = false;

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
                if (targetMovingBadNumber == 3) {
                    AttackPattern2SpawnBadTargets();
                }
                else {
                    AttackPattern2SpawnGoodTargets();
                }
            }

            // Phase 2
            else if (phase == 1) {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // Spawn a good and bad target
                    AttackPattern2SpawnBadTargets();
                    AttackPattern2SpawnGoodTargets();

            }

            // Phase 3
            else {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // Spawn 2 good and 2 bad targets
                    AttackPattern2SpawnBadTargets();
                    AttackPattern2SpawnGoodTargets();
                    AttackPattern2SpawnBadTargets();
                    AttackPattern2SpawnGoodTargets();
            }
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

        // Mouse click for all of phase2
        if (!MainMenu.phoneMode) {
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
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        transform.position.y,
                        0.0f
                        );
        targetFallingBadClone = Instantiate(targetFallingBad, spawnPosition, spawnRotation);
        targetFallingBadClone.transform.position = spawnPosition;
        Physics2D.IgnoreCollision(targetFallingBadClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(targetFallingBadClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Spawn phase 2 good targets
    private void AttackPattern2SpawnGoodTargets() {
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        transform.position.y,
                        0.0f
                        );
        targetFallingClone = Instantiate(targetFalling, spawnPosition, spawnRotation);
        targetFallingClone.transform.position = spawnPosition;
        Physics2D.IgnoreCollision(targetFallingClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(targetFallingClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Go to next boss
    private void nextBoss() {
        SceneManager.LoadScene("EPB_level1_boss3");
    }

    // Destroy All Targets
    private void DestroyAllTargetsPhase2() {
        targetListBad = GameObject.FindGameObjectsWithTag("phase2_falling_bad");
        targetList = GameObject.FindGameObjectsWithTag("phase2_falling");

        for (int i = 0; i < targetList.Length; i++) {
            Destroy(targetList[i]);
        }
        for (int i = 0; i < targetListBad.Length; i++) {
            Destroy(targetListBad[i]);
        }
    }

    
    // On MOUSE click do something
    private void OnClickDoSomethingAttackPattern2() {
        // On click, do something
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);
            // If target is clicked, destroy target
            if (hitTarget.collider != null) {
                // If click good target, damage boss
                if (hitTarget.collider.gameObject.tag == "phase2_falling") {
                    Destroy(hitTarget.collider.gameObject);
                    PlayerAttack();
                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                }
                // If click bad target, damage player
                else if ((hitTarget.collider.gameObject.tag == "phase2_falling_bad")) {
                    Destroy(hitTarget.collider.gameObject);
                    BossAttack();
                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                }
            }
        }
    }
    

     
    // On PHONE TOUCH do something
    private void OnTouchDoSomethingAttackPattern2()
    {
        // On click, do something
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position),
                    Vector2.zero);
                // If target is clicked, destroy target
                if (hitTarget.collider != null)
                {
                    // If click good target, damage boss
                    if (hitTarget.collider.gameObject.tag == "phase2_falling")
                    {
                        Destroy(hitTarget.collider.gameObject);
                        PlayerAttack();
                        boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                    }
                    // If click bad target, damage player
                    else if ((hitTarget.collider.gameObject.tag == "phase2_falling_bad"))
                    {
                        Destroy(hitTarget.collider.gameObject);
                        BossAttack();
                        player.GetComponent<PlayerHealthManager>().giveDamage(1);
                    }
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
