using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level2Boss3 : MonoBehaviour {
    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject orangeSkull;
    public GameObject orangeSkullMoving;
    public GameObject greySkull;
    public GameObject greySkullMoving;
    public GameObject greySilentSkull;
    public GameObject magentaSkull;
    public GameObject orangeSilentSkull;
    private GameObject orangeSkullClone;
    private GameObject greySkullClone;
    //private GameObject[] hit;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;
    private GameObject[] targetListBad;
    private GameObject[] targetList;
    private GameObject[] targetListSilent;
    private GameObject[] attackPattern1Targets;
    private GameObject[] attackPattern2Targets;
    private GameObject[] attackPattern3Targets1;
    private GameObject[] attackPattern3Targets2;
    private GameObject[] attackPattern3Targets3;
    private GameObject[] attackPattern3LockedTargets1;
    private GameObject[] attackPattern3LockedTargets2;
    private GameObject[] attackPattern3LockedTargets3;

    private List<GameObject> targets = new List<GameObject>();

    private int numberOfHitMarkers;
    public int badTargetNumber;
    public int badTargetNumber2;
    public int badTargetNumber3;
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

    private Vector3[] attackPattern2Position = { new Vector3 { x = -6, y = 4.5f, z = 0 },
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

    private Vector3[] attackPattern3Position1 = { new Vector3 { x = -6, y = 4.5f, z = 0 },
                                  new Vector3 { x = -2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 6, y = 4.5f, z = 0 } };
    private Vector3[] attackPattern3Position2 = { new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2f, z = 0 } };
    private Vector3[] attackPattern3Position3 = { new Vector3 { x = -6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 6, y = -0.5f, z = 0 } };



    private Quaternion spawnRotation;

    private bool youWinBool;
    public bool audioAlreadyPlayed;
    private bool phaseOn;
    private bool arrayIsEmpty;
    private bool arrayIsEmpty2;
    private bool arrayIsEmpty3;
    private bool arrayIsEmpty4;
    private bool arrayIsEmptyAttack3;
    private bool arrayIsEmptyAttack3Phase2;
    private bool arrayIsEmptyAttack3Phase3;
    private bool attack3Phase1;
    private bool attack3Phase2;
    private bool attack3Phase3;

    public AudioSource gameOverAudioSource;
    public AudioSource youWinAudioSource;

    public AudioClip gameOverAudioClip;
    public AudioClip youWinAudioClip;

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
        attackPattern = 0;
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
        attackPattern2Targets = new GameObject[attackPattern2Position.Length];
        bottomLeftScreen = Camera.main.ScreenToWorldPoint((new Vector3(0, 0, 0.0f)));
        topRightScreen = Camera.main.ScreenToWorldPoint((new Vector3(Screen.width, Screen.height, 0.0f)));
        skullRadius = orangeSkullMoving.GetComponent<CircleCollider2D>().radius;
        //skullRadius = orangeSkullMoving.transform.localScale.x / 2;
        youWinBool = false;
        attackPattern = Random.Range(0, 3);

        //Attackpattern 1 initialization
        attackPattern1Targets = new GameObject[attackPattern1Positions.Length];

        //AttackPattern3 initialization
        numberOfHitMarkers = 4;
        arrayIsEmpty = true;
        arrayIsEmpty2 = true;
        arrayIsEmpty3 = true;
        arrayIsEmpty4 = true;
        attackPattern3Targets1 = new GameObject[numberOfHitMarkers];
        attackPattern3Targets2 = new GameObject[numberOfHitMarkers];
        attackPattern3Targets3 = new GameObject[numberOfHitMarkers];
        attackPattern3LockedTargets1 = new GameObject[numberOfHitMarkers];
        attackPattern3LockedTargets2 = new GameObject[numberOfHitMarkers];
        attackPattern3LockedTargets3 = new GameObject[numberOfHitMarkers];
        badTargetNumber = 0;
        badTargetNumber2 = 0;
        badTargetNumber3 = 0;

        if (attackPattern == 0) {
            StartCoroutine(AttackPattern1());
        }
        else if (attackPattern == 1) {
            StartCoroutine(AttackPattern2());
        }
        else {
            StartCoroutine(AttackPattern3());
        }
    }

    // Update timers and texts
    private void FixedUpdate() {
        if (!youWinBool) {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                timeLeft = 0;
            }
            UpdateText();
            phaseTimeLeft -= Time.deltaTime;

            // Start picking attack pattern

            if (phaseTimeLeft <= 0) {
                if (phaseOn == false) {
                    //StopAllCoroutines();

                    //DestroyAllTargetsPhase1(hit);
                    //DestroyAllTargetsPhase1(hit2);
                    //DestroyAllTargetsPhase1(hit3);
                    //DestroyAllTargetsPhase1(hit4);
                    //DestroyAllTargetsPhase2();
                    //DestroyAllTargetsPhase3();

                    phaseTimeLeft = 12;
                    attackPattern = Random.Range(0, 3);
                    if (attackPattern == 0) {
                        StartCoroutine(AttackPattern1());
                    }
                    else if (attackPattern == 1) {
                        StartCoroutine(AttackPattern2());
                    }
                    else {
                        StartCoroutine(AttackPattern3());
                    }
                }
            }
        }
}

    // AttackPattern 1
    IEnumerator AttackPattern1() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 0
                && phaseOn == false) {
            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

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
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
            }

            // Phase 2
            else if (phase == 1) {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // If random number is the last number (3) in this case, spawn bad target
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();

            }

            // Phase 3
            else {
                bossAnimation.SetTrigger("skill_2");
                yield return new WaitForSeconds(Random.Range(0.25f, 5f));

                targetMovingBadNumber = Random.Range(0, 4);

                // If random number is the last number (3) in this case, spawn bad target
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
                AttackPattern1SpawnBadTargets();
                AttackPattern1SpawnGoodTargets();
            }

            phaseOn = false;
            if (phaseTimeLeft <= 0) {
                StopAllCoroutines();
                DestroyAllTargetsAttackPattern1(attackPattern1Targets);
            }
        }
    }

    // Phase 2
    IEnumerator AttackPattern2() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 1
                && phaseOn == false) {
            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

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
                // Set attackPattern3 skull speed
                skullSpeed = 7;

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
                // Set attackPattern3 skull speed
                skullSpeed = 9;

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

            phaseOn = false;
            if (phaseTimeLeft <= 0) {
                StopAllCoroutines();
                DestroyAllTargetsAttackPattern2();
            }
        }
    }

    // AttackPattern 3
    IEnumerator AttackPattern3() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 2
                && phaseOn == false) {
            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

            // Phase 1
            if (phase == 0) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(attackPattern3Targets1, arrayIsEmpty);

                if (arrayIsEmpty) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    // TEST REMOVE Instantiate(greySkull, attackPattern3Position1[0], Quaternion.identity);
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern3SpawnTargets(attackPattern3LockedTargets1, attackPattern3Position1);
                    // TEST REMOVE attackPattern3Targets1[0] = Instantiate(greySkull, attackPattern3Position1[0], spawnRotation);
                    yield return new WaitForSeconds(2);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets1);

                    yield return new WaitForSeconds(0.1f);
                    for(int i = 0; i < attackPattern3Targets1.Length; i++) {
                            attackPattern3Targets1[i] = Instantiate(magentaSkull, attackPattern3Position1[i], spawnRotation);
                            attackPattern3Targets1[i].transform.position = attackPattern3Position1[i];
                    }

                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);
                }

                else {
                    // After 2 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);

                    phaseOn = false;
                    if (phaseTimeLeft <= 0) {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets1);
                    }
                }
            }

            if (phase == 1) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(attackPattern3Targets1, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(attackPattern3Targets2, arrayIsEmpty2);

                if (arrayIsEmpty) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    // TEST REMOVE Instantiate(greySkull, attackPattern3Position1[0], Quaternion.identity);
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern3SpawnTargets(attackPattern3LockedTargets1, attackPattern3Position1);
                    badTargetNumber2 = AttackPattern3SpawnTargets(attackPattern3LockedTargets2, attackPattern3Position2);
                    // TEST REMOVE attackPattern3Targets1[0] = Instantiate(greySkull, attackPattern3Position1[0], spawnRotation);
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets1);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets2);

                    yield return new WaitForSeconds(0.1f);
                    for (int i = 0; i < attackPattern3Targets1.Length; i++) {
                        attackPattern3Targets1[i] = Instantiate(magentaSkull, attackPattern3Position1[i], spawnRotation);
                        attackPattern3Targets1[i].transform.position = attackPattern3Position1[i];
                    }
                    for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                        attackPattern3Targets2[i] = Instantiate(magentaSkull, attackPattern3Position2[i], spawnRotation);
                        attackPattern3Targets2[i].transform.position = attackPattern3Position2[i];
                    }

                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);
                    AttackPattern3DestroyTargets(attackPattern3Targets2, badTargetNumber2);
                }

                else {
                    // After 2 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);
                    AttackPattern3DestroyTargets(attackPattern3Targets2, badTargetNumber2);

                    phaseOn = false;
                    if (phaseTimeLeft <= 0) {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets1);
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
                    }
                }
            }

            // Phase 3
            else if (phase == 2){
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(attackPattern3Targets1, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(attackPattern3Targets2, arrayIsEmpty2);
                arrayIsEmpty3 = ArrayIsEmpty(attackPattern3Targets3, arrayIsEmpty3);

                if (arrayIsEmpty) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    // TEST REMOVE Instantiate(greySkull, attackPattern3Position1[0], Quaternion.identity);
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern3SpawnTargets(attackPattern3LockedTargets1, attackPattern3Position1);
                    badTargetNumber2 = AttackPattern3SpawnTargets(attackPattern3LockedTargets2, attackPattern3Position2);
                    badTargetNumber3 = AttackPattern3SpawnTargets(attackPattern3LockedTargets3, attackPattern3Position3);
                    // TEST REMOVE attackPattern3Targets1[0] = Instantiate(greySkull, attackPattern3Position1[0], spawnRotation);
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets1);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets2);
                    AttackPattern3DestroyLockedTargets(attackPattern3LockedTargets3);

                    yield return new WaitForSeconds(0.1f);
                    for (int i = 0; i < attackPattern3Targets1.Length; i++) {
                        attackPattern3Targets1[i] = Instantiate(magentaSkull, attackPattern3Position1[i], spawnRotation);
                        attackPattern3Targets1[i].transform.position = attackPattern3Position1[i];
                    }
                    for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                        attackPattern3Targets2[i] = Instantiate(magentaSkull, attackPattern3Position2[i], spawnRotation);
                        attackPattern3Targets2[i].transform.position = attackPattern3Position2[i];
                    }
                    for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                        attackPattern3Targets3[i] = Instantiate(magentaSkull, attackPattern3Position3[i], spawnRotation);
                        attackPattern3Targets3[i].transform.position = attackPattern3Position3[i];
                    }

                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);
                    AttackPattern3DestroyTargets(attackPattern3Targets2, badTargetNumber2);
                    AttackPattern3DestroyTargets(attackPattern3Targets3, badTargetNumber2);
                }

                else {
                    // After 2 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets1, badTargetNumber);
                    AttackPattern3DestroyTargets(attackPattern3Targets2, badTargetNumber2);
                    AttackPattern3DestroyTargets(attackPattern3Targets3, badTargetNumber3);

                    phaseOn = false;
                    if (phaseTimeLeft <= 0) {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets1);
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
                    }
                }
            }
            phaseOn = false;
            /*
            if (phaseTimeLeft <= 0) {
                StopAllCoroutines();
                DestroyAllTargetsAttackPattern3(attackPattern3Targets1);
                DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
                DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
            }
            */
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
            DestroyAllTargetsAttackPattern1(attackPattern1Targets);
            DestroyAllTargetsAttackPattern2();
            DestroyAllTargetsAttackPattern3(attackPattern3Targets1);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
            youWinText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            youWinBool = true;
                if (!audioAlreadyPlayed) {
                    Destroy(GameObject.FindWithTag("music"));
                    youWinAudioSource.PlayOneShot(youWinAudioClip);
                    audioAlreadyPlayed = true;
                }
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
            OnClickDoSomethingAttackPattern1();

            OnClickDoSomethingAttackPattern2();

            OnClickDoSomethingAttackPattern3(attackPattern3Targets1, badTargetNumber);
            OnClickDoSomethingAttackPattern3(attackPattern3Targets2, badTargetNumber2);
            OnClickDoSomethingAttackPattern3(attackPattern3Targets3, badTargetNumber3);
        }
        else {
            OnTouchDoSomethingAttackPattern1();
            OnTouchDoSomethingAttackPattern2();

            OnTouchDoSomethingAttackPattern3(attackPattern3Targets1, badTargetNumber);
            OnTouchDoSomethingAttackPattern3(attackPattern3Targets2, badTargetNumber);
            OnTouchDoSomethingAttackPattern3(attackPattern3Targets3, badTargetNumber);
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
            orangeSkullClone = Instantiate(orangeSkull, attackPattern1Positions[randomPosition], spawnRotation);
            orangeSkullClone.transform.position = attackPattern1Positions[randomPosition];
            attackPattern1Targets[randomPosition] = orangeSkullClone;
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
            greySkullClone = Instantiate(greySkullMoving, attackPattern2Position[randomPosition], spawnRotation);
            greySkullClone.transform.position = attackPattern2Position[randomPosition];
            attackPattern2Targets[randomPosition] = greySkullClone;
            // Create a moving target
            skullDirection[randomPosition] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);

            attackPattern2Targets[randomPosition] = greySkullClone;

            Destroy(greySkullClone, 5);
        }
        else {
            randomPosition = Random.Range(0, attackPattern2Position.Length);
        }
    }

    // Spawn phase 2 good targets
    private void AttackPattern2SpawnGoodTargets() {
        /*
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        Random.Range(-maxHeightScreen, maxHeightScreen),
                        0.0f
                        );
                        */
        randomPosition = Random.Range(0, attackPattern2Position.Length);
        if (!attackPattern2Targets[randomPosition]) {
            orangeSkullClone = Instantiate(orangeSkullMoving, attackPattern2Position[randomPosition], spawnRotation);
            orangeSkullClone.transform.position = attackPattern2Position[randomPosition];
            // Create a moving target
            skullDirection[randomPosition] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f);

            attackPattern2Targets[randomPosition] = orangeSkullClone;
            //Destroy(orangeSkullClone, 3);
            StartCoroutine(WaitAndDestroy(attackPattern2Targets[randomPosition]));
        }
        else {
            randomPosition = Random.Range(0, attackPattern2Position.Length);
        }
        /*
        orangeSkullClone = Instantiate(orangeSkull, spawnPosition, spawnRotation);
        orangeSkullClone.transform.position = spawnPosition;
        targets.Add(orangeSkullClone);
        Destroy(orangeSkullClone, 3);
        */
    }

    // Spawn 4 targets in attack pattern 3
    private int AttackPattern3SpawnTargets(GameObject[] array, Vector3[] positions) {
        // Choose randomly which target is the bad target
        int random = Random.Range(0, array.Length);

        // Instantiate the hit targets
        for (int i = 0; i < array.Length; i++) {
            if (random == i) {
                array[i] = Instantiate(greySilentSkull, positions[i], spawnRotation);
                array[i].transform.position = positions[i];

            }
            else {
                array[i] = Instantiate(orangeSilentSkull, positions[i], spawnRotation);
                array[i].transform.position = positions[i];
            }
        }

        return random;
    }


    // Destroys targets in attack pattern 3
    private void AttackPattern3DestroyTargets(GameObject[] array, int badTarget) {
        for (int i = 0; i < array.Length; i++) {
            // Destroys all normal targets, damage player
            if (array[i] && i != badTarget) {
                // Do boss animation on first attack only
                if (i == 0) {
                    BossAttack();
                }
                Destroy(array[i]);
                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
            }
        }
        Destroy(array[badTarget]);
    }

    // Destroys targets in attack pattern 3
    private void AttackPattern3DestroyLockedTargets(GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
                Destroy(array[i]);
        }
    }

    // AttackPattern 3 Incorrect Hit
    private void AttackPattern3IncorrectHit(GameObject[] targets) {
        BossAttack();
        for (int i = 0; i < targets.Length; i++) {
            if (targets[i]) {
                Destroy(targets[i]);
                player.GetComponent<PlayerHealthManager>().giveDamage(1);
            }
        }
    }

    // Destroy All Targets Phase1
    private void DestroyAllTargetsAttackPattern1(GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                Destroy(array[i]);
            }
        }
    }

    // Destroy All Targets Phase2
    private void DestroyAllTargetsAttackPattern2() {
        targetListBad = GameObject.FindGameObjectsWithTag("grey_skull_moving");
        targetList = GameObject.FindGameObjectsWithTag("orange_skull_moving");

        for (int i = 0; i < targetList.Length; i++) {
            Destroy(targetList[i]);
        }
        for (int i = 0; i < targetListBad.Length; i++) {
            Destroy(targetListBad[i]);
        }
    }

    // Destroy All Targets Phase2
    private void DestroyAllTargetsAttackPattern3(GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                Destroy(array[i]);
            }
        }
    }

    // Go to next boss
    private void nextBoss() {
        SceneManager.LoadScene("EPB_level2_boss3");
    }

    // Destroy All Targets
    private void DestroyAllTargetsPhase2() {
        targetListBad = GameObject.FindGameObjectsWithTag("grey_skull");
        targetList = GameObject.FindGameObjectsWithTag("orange_skull");
        targetListSilent = GameObject.FindGameObjectsWithTag("magenta_skull");

        for (int i = 0; i < targetList.Length; i++) {
            Destroy(targetList[i]);
        }
        for (int i = 0; i < targetListBad.Length; i++) {
            Destroy(targetListBad[i]);
        }
        for (int i = 0; i < targetListSilent.Length; i++) {
            Destroy(targetListSilent[i]);
        }
    }


    // On  PHONE click do something
    private void OnTouchDoSomethingAttackPattern1()
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
                    if (hitTarget.collider.gameObject.tag == "orange_skull")
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
    }

    // On PHONE click do something
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

                // On PHONE CLICK do something attackPattern3
    private void OnTouchDoSomethingAttackPattern3(GameObject[] targets, int badTarget)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            // Attack pattern 3 Mouse Down
                    if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                        RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position),
                    Vector2.zero);

                // If target is clicked, destroy target
                if (hitTarget.collider != null)
                {

                    //if (hitTarget.collider.gameObject == targets[spawnOrder[0]]
                    // || hitTarget.collider.gameObject == targets[spawnOrder[1]]
                    // || hitTarget.collider.gameObject == targets[spawnOrder[2]]
                    // || hitTarget.collider.gameObject == targets[spawnOrder[3]]){ 

                    for (int j = 0; j < targets.Length; j++)
                    {
                        if (targets[j])
                        {
                            if (hitTarget.collider.gameObject == targets[badTarget])
                            {
                                AttackPattern3IncorrectHit(targets);
                                // Break when target is found
                                break;
                            }
                            else if (hitTarget.collider.gameObject == targets[j])
                            {
                                Destroy(targets[j]);
                                PlayerAttack();
                                boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                                // Break when target is found
                                break;
                            }
                        }
                    }
                    //
                }
            }
        }
    }

    
    // On MOUSE CLICK do something
    private void OnClickDoSomethingAttackPattern1() {
        // On click, do something
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
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

    // On click do something
    private void OnClickDoSomethingAttackPattern2() {
        // On click, do something
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);
            // If target is clicked, destroy target
            if (hitTarget.collider != null) {
                // If click good target, damage boss
                if (hitTarget.collider.gameObject.tag == "orange_skull_moving") {
                    Destroy(hitTarget.collider.gameObject);
                    PlayerAttack();
                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                }
                // If click bad target, damage player
                else if ((hitTarget.collider.gameObject.tag == "grey_skull_moving")) {
                    Destroy(hitTarget.collider.gameObject);
                    BossAttack();
                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                }
            }
        }
    }

    // On click do something attackPattern3
    private void OnClickDoSomethingAttackPattern3(GameObject[] targets, int badTarget) {
        // Attack pattern 3 Mouse Down
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

            // If target is clicked, destroy target
            if (hitTarget.collider != null) {

                //if (hitTarget.collider.gameObject == targets[spawnOrder[0]]
                // || hitTarget.collider.gameObject == targets[spawnOrder[1]]
                // || hitTarget.collider.gameObject == targets[spawnOrder[2]]
                // || hitTarget.collider.gameObject == targets[spawnOrder[3]]){ 

                for (int i = 0; i < targets.Length; i++) {
                    if (targets[i]) {
                        if (hitTarget.collider.gameObject == targets[badTarget]) {
                            AttackPattern3IncorrectHit(targets);
                            // Break when target is found
                            break;
                        }
                        else if (hitTarget.collider.gameObject == targets[i]){
                            Destroy(targets[i]);
                            PlayerAttack();
                            boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                            // Break when target is found
                            break;
                        }
                    }
                }
                //
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

    // Check if array is empty
    private bool ArrayIsEmpty(GameObject[] array, bool arrayBool) {
        // Check if array is empty, if it is, populate the stage
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                arrayBool = false;
            }
            else {
                arrayBool = true;
            }
        }

        return arrayBool;
    }

    // Update scores and text
    void UpdateText() {
        timerText.text = "Time Left:\n" + Mathf.RoundToInt(timeLeft);
        bossHealthText.text = "Boss Health:\n" + Mathf.RoundToInt(boss.GetComponent<EnemyHealthManager>().bossHealth);
        playerHealthText.text = "Player Health: \n" + Mathf.RoundToInt(PlayerHealthManager.playerHealth);
    }
}
