using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level1Boss3 : MonoBehaviour {
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
    public GameObject targetPhase3;
    public GameObject targetBadPhase3;
    private GameObject targetFallingClone;
    private GameObject targetFallingBadClone;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;
    private GameObject[] hit;
    private GameObject[] hit2;
    private GameObject[] hit3;
    private GameObject[] hit4;
    private GameObject[] attackPattern3Targets;
    private GameObject[] attackPattern3Targets2;
    private GameObject[] attackPattern3Targets3;
    private GameObject[] attackPattern3TargetsLocked;
    private GameObject[] attackPattern3TargetsLocked2;
    private GameObject[] attackPattern3TargetsLocked3;
    private GameObject[] targetListBad;
    private GameObject[] targetList;

    private int numberOfHitMarkers;
    public int badTargetNumber;
    public int badTargetNumber2;
    public int badTargetNumber3;
    public int badTargetNumber4;
    private int numberOfAttackPattern3TargetsPhase1;
    private int numberOfAttackPattern3TargetsPhase2;
    private int numberOfAttackPattern3TargetsPhase3;
    private int targetMovingBadNumber;
    private int attackPattern;
    //private int phase3RandomNumber;
    private int phase;
    private int twoThirdsHealth;
    private int oneThirdHealth;
    private int[] phase3SpawnOrder;
    private int[] phase3SpawnOrder2;
    private int[] phase3SpawnOrder3;

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
    private Vector3[] position = { new Vector3 { x = -6, y = 4.5f, z = 0 },
                                  new Vector3 { x = -2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 2f, y = 4.5f, z = 0 },
                                  new Vector3 { x = 6, y = 4.5f, z = 0 } };
    private Vector3[] position2 = { new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2f, z = 0 } };
    private Vector3[] position3 = { new Vector3 { x = -6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 6, y = -0.5f, z = 0 } };
    private Vector3[] position4 = { new Vector3 { x = -6, y = -3f, z = 0 },
                                  new Vector3 { x = -2f, y = -3f, z = 0 },
                                  new Vector3 { x = 2f, y = -3f, z = 0 },
                                  new Vector3 { x = 6, y = -3f, z = 0 } };

    private Vector3[] attackPattern3Positions = { new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2f, z = 0 } };
    private Vector3[] attackPattern3Positions2 = { new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2, z = 0 },
                                  new Vector3 { x = -6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -2f, y = -0.5f, z = 0 } };
    private Vector3[] attackPattern3Positions3 = { new Vector3 { x = -6, y = 2f, z = 0 },
                                  new Vector3 { x = -2f, y = 2f, z = 0 },
                                  new Vector3 { x = 2f, y = 2f, z = 0 },
                                  new Vector3 { x = 6, y = 2f, z = 0 },
                                  new Vector3 { x = -6, y = -0.5f, z = 0 },
                                  new Vector3 { x = -2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 2f, y = -0.5f, z = 0 },
                                  new Vector3 { x = 6f, y = -0.5f, z = 0 }};

    private bool phaseOn;
    //private bool noMoreHitTargets;
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
    private bool audioAlreadyPlayed;
    //private bool targetExists;
    //private bool targetBadExists;
    public bool youWinBool;

    public AudioSource youWinAudioSource;
    public AudioSource gameOverAudioSource;

    public AudioClip youWinAudioClip;
    public AudioClip gameOverAudioClip;

    private Quaternion spawnRotation;

    // Use this for initialization
    void Start() {
        if (cam == null) {
            cam = Camera.main;
        }
        numberOfHitMarkers = 4;
        //noMoreHitTargets = false;
        arrayIsEmpty = true;
        arrayIsEmpty2 = true;
        arrayIsEmpty3 = true;
        arrayIsEmpty4 = true;
        //targetExists = false;
        //targetBadExists = false;
        strikePositionTarget = new Vector3(2.1f, -1f, 0);
        strikePositionPlayer = new Vector3(-4.64f, -4.24f, 0);
        playerAnimation = player.GetComponent<Animator>();
        bossAnimation = boss.GetComponent<Animator>();
        hit = new GameObject[numberOfHitMarkers];
        hit2 = new GameObject[numberOfHitMarkers];
        hit3 = new GameObject[numberOfHitMarkers];
        hit4 = new GameObject[numberOfHitMarkers];
        numberOfHitMarkers = 4;
        numberOfAttackPattern3TargetsPhase1 = 4;
        numberOfAttackPattern3TargetsPhase2 = 6;
        numberOfAttackPattern3TargetsPhase3 = 8;
        phaseTimeLeft = 12;
        hit = new GameObject[numberOfHitMarkers];
        attackPattern3Targets = new GameObject[numberOfAttackPattern3TargetsPhase1];
        attackPattern3Targets2 = new GameObject[numberOfAttackPattern3TargetsPhase2];
        attackPattern3Targets3 = new GameObject[numberOfAttackPattern3TargetsPhase3];
        attackPattern3TargetsLocked = new GameObject[numberOfAttackPattern3TargetsPhase1];
        attackPattern3TargetsLocked2 = new GameObject[numberOfAttackPattern3TargetsPhase2];
        attackPattern3TargetsLocked3 = new GameObject[numberOfAttackPattern3TargetsPhase3];
        phase3SpawnOrder = new int[] { 0, 1, 2, 3 };
        phase3SpawnOrder2 = new int[] { 0, 1, 2, 3, 4, 5 };
        phase3SpawnOrder3 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        spawnRotation = Quaternion.identity;
        attackPattern = Random.Range(0,3);
        //attackPattern = 2;
        phase = 0;
        phaseOn = false;
        oneThirdHealth = boss.GetComponent<EnemyHealthManager>().bossHealth / 3;
        twoThirdsHealth = oneThirdHealth * 2;
        // Set screen boundaries
        upperCornerScreen = new Vector3(Screen.width, Screen.height, 0.0f);
        targetWidth = cam.ScreenToWorldPoint(upperCornerScreen);
        skullWidth = target.GetComponent<Renderer>().bounds.extents.x;
        maxWidthScreen = targetWidth.x - skullWidth; ;
        audioAlreadyPlayed = false;
        youWinBool = false;
        attack3Phase1 = false;
        attack3Phase2 = false;
        attack3Phase3 = false;

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

    // Phase 1
    IEnumerator AttackPattern1() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 0) {
            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

            // Phase 1
            if (phase == 0) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit, arrayIsEmpty);

                if (arrayIsEmpty) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_3");
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern1SpawnTargets(hit, position);
                }

                else {
                    // After 2 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(2);
                    AttackPattern1DestroyTargets(hit, badTargetNumber);
                }
            }

            // Phase 2
            else if (phase == 1) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(hit2, arrayIsEmpty2);

                if (arrayIsEmpty && arrayIsEmpty2) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_3");
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern1SpawnTargets(hit, position);
                    badTargetNumber2 = AttackPattern1SpawnTargets(hit2, position2);
                }

                else {
                    // After 3 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(2.5f);
                    AttackPattern1DestroyTargets(hit, badTargetNumber);
                    AttackPattern1DestroyTargets(hit2, badTargetNumber2);
                }
            }

            // Phase 3
            else {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(hit2, arrayIsEmpty2);
                arrayIsEmpty3 = ArrayIsEmpty(hit3, arrayIsEmpty3);
                arrayIsEmpty4 = ArrayIsEmpty(hit4, arrayIsEmpty4);

                if (arrayIsEmpty && arrayIsEmpty2 && arrayIsEmpty3 && arrayIsEmpty4) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_3");
                    yield return new WaitForSeconds(1);
                    badTargetNumber = AttackPattern1SpawnTargets(hit, position);
                    badTargetNumber2 = AttackPattern1SpawnTargets(hit2, position2);
                    badTargetNumber3 = AttackPattern1SpawnTargets(hit3, position3);
                    badTargetNumber4 = AttackPattern1SpawnTargets(hit4, position4);
                }

                else {
                    // After 4 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(4);
                    AttackPattern1DestroyTargets(hit, badTargetNumber);
                    AttackPattern1DestroyTargets(hit2, badTargetNumber2);
                    AttackPattern1DestroyTargets(hit3, badTargetNumber3);
                    AttackPattern1DestroyTargets(hit4, badTargetNumber4);
                }
            }
            phaseOn = false;
            if (phaseTimeLeft <= 0)
            {
                StopAllCoroutines();
                DestroyAllTargetsAttackPattern1(hit);
                DestroyAllTargetsAttackPattern1(hit2);
                DestroyAllTargetsAttackPattern1(hit3);
                DestroyAllTargetsAttackPattern1(hit4);
            }
        }
    }

    // Phase 2
    IEnumerator AttackPattern2() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 1) {
            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

            // Phase 1
            if (phase == 0) {
                bossAnimation.SetTrigger("skill_3");
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
                bossAnimation.SetTrigger("skill_3");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // Spawn a good and bad target
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();

            }

            // Phase 3
            else {
                bossAnimation.SetTrigger("skill_3");
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));

                targetMovingBadNumber = Random.Range(0, 4);

                // Spawn 2 good and 2 bad targets
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
                AttackPattern2SpawnBadTargets();
                AttackPattern2SpawnGoodTargets();
            }
            phaseOn = false;
            if (phaseTimeLeft <= 0)
            {
                StopAllCoroutines();
                DestroyAllTargetsAttackPattern2();
            }
        }
    }

    // Attack Pattern 3
    IEnumerator AttackPattern3() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0 
            && timeLeft > 0 
            && PlayerHealthManager.playerHealth > 0 
            && attackPattern == 2) {

            // Turn on the phase so it doesnt get interrupted
            yield return new WaitForSeconds(0.1f);
            phaseOn = true;

            // Phase 1
            if (phase == 0) {
                arrayIsEmptyAttack3 = ArrayIsEmpty(attackPattern3Targets, arrayIsEmpty);

                if (arrayIsEmptyAttack3) {
                    // Shuffle the phase3SpawnOrder to randomize the values (Fisher Yates Shuffle)
                    AttackPattern3ShuffleTargets(phase3SpawnOrder);

                    // Spawn locked targets
                    bossAnimation.SetTrigger("skill_3");
                    for (int j = 0; j < attackPattern3TargetsLocked.Length; j++) {
                        yield return new WaitForSeconds(1);
                        AttackPattern3SpawnLockedTargets(attackPattern3TargetsLocked, phase3SpawnOrder, attackPattern3Positions, j);
                    }
                    // Destroy fake targets so we can spawn real targets
                    yield return new WaitForSeconds(1);
                        AttackPattern3DestroyLockedTargets(attackPattern3TargetsLocked);

                    // Spawn real targets
                        AttackPattern3SpawnTargets(attackPattern3Targets, phase3SpawnOrder, attackPattern3Positions);
                }
                else {
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets);
                    phaseOn = false;
                    if (phaseTimeLeft <= 0)
                    {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets);
                    }
                }
            }
            // Phase 2
            else if (phase == 1) {
                arrayIsEmptyAttack3Phase2 = ArrayIsEmpty(attackPattern3Targets2, arrayIsEmpty);

                if (arrayIsEmptyAttack3Phase2) {
                    // Shuffle the phase3SpawnOrder to randomize the values (Fisher Yates Shuffle)
                    AttackPattern3ShuffleTargets(phase3SpawnOrder2);

                    // Spawn locked targets
                    bossAnimation.SetTrigger("skill_3");
                    for (int j = 0; j < attackPattern3TargetsLocked2.Length; j++) {
                        yield return new WaitForSeconds(1);
                        AttackPattern3SpawnLockedTargets(attackPattern3TargetsLocked2, phase3SpawnOrder2, attackPattern3Positions2, j);
                    }
                    // Destroy fake targets so we can spawn real targets
                    yield return new WaitForSeconds(1);
                    AttackPattern3DestroyLockedTargets(attackPattern3TargetsLocked2);

                    // Spawn real targets
                    AttackPattern3SpawnTargets(attackPattern3Targets2, phase3SpawnOrder2, attackPattern3Positions2);
                }
                else {
                    yield return new WaitForSeconds(3);
                    AttackPattern3DestroyTargets(attackPattern3Targets2);
                    phaseOn = false;
                    if (phaseTimeLeft <= 0)
                    {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
                    }
                }
            }
            // Phase 3
            else {
                arrayIsEmptyAttack3Phase3 = ArrayIsEmpty(attackPattern3Targets3, arrayIsEmpty);

                if (arrayIsEmptyAttack3Phase3) {
                    // Shuffle the phase3SpawnOrder to randomize the values (Fisher Yates Shuffle)
                    AttackPattern3ShuffleTargets(phase3SpawnOrder3);

                    // Spawn locked targets
                    bossAnimation.SetTrigger("skill_3");
                    for (int j = 0; j < attackPattern3TargetsLocked3.Length; j++) {
                        yield return new WaitForSeconds(1);
                        AttackPattern3SpawnLockedTargets(attackPattern3TargetsLocked3, phase3SpawnOrder3, attackPattern3Positions3, j);
                    }
                    // Destroy fake targets so we can spawn real targets
                    yield return new WaitForSeconds(1);
                    AttackPattern3DestroyLockedTargets(attackPattern3TargetsLocked3);

                    // Spawn real targets
                    AttackPattern3SpawnTargets(attackPattern3Targets3, phase3SpawnOrder3, attackPattern3Positions3);
                }
                else {
                    yield return new WaitForSeconds(4);
                    AttackPattern3DestroyTargets(attackPattern3Targets3);
                    phaseOn = false;
                    if (phaseTimeLeft <= 0)
                    {
                        StopAllCoroutines();
                        DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
                    }
                }
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
            DestroyAllTargetsAttackPattern1(hit);
            DestroyAllTargetsAttackPattern1(hit2);
            DestroyAllTargetsAttackPattern1(hit3);
            DestroyAllTargetsAttackPattern1(hit4);
            DestroyAllTargetsAttackPattern2();
            DestroyAllTargetsAttackPattern3(attackPattern3Targets);
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
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargetsAttackPattern1(hit);
            DestroyAllTargetsAttackPattern1(hit2);
            DestroyAllTargetsAttackPattern1(hit3);
            DestroyAllTargetsAttackPattern1(hit4);
            DestroyAllTargetsAttackPattern2();
            DestroyAllTargetsAttackPattern3(attackPattern3Targets);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
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
            DestroyAllTargetsAttackPattern1(hit);
            DestroyAllTargetsAttackPattern1(hit2);
            DestroyAllTargetsAttackPattern1(hit3);
            DestroyAllTargetsAttackPattern1(hit4);
            DestroyAllTargetsAttackPattern2();
            DestroyAllTargetsAttackPattern3(attackPattern3Targets);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets2);
            DestroyAllTargetsAttackPattern3(attackPattern3Targets3);
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
            phase = 2;
        }

        // Start phasess
        if (phase == 0) {
            if (!MainMenu.phoneMode) {
                // Mouse click for phase 1 attack 1, 2, 3
                OnClickDoSomethingAttackPattern1(hit, badTargetNumber);
                OnClickDoSomethingAttackPattern2();
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            else {
                // Mouse click for phase 1 attack 1, 2, 3
                OnTouchDoSomethingAttackPattern1(hit, badTargetNumber);
                OnTouchDoSomethingAttackPattern2();
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            attack3Phase1 = false;
            attack3Phase2 = false;
            attack3Phase3 = false;
        }
        else if (phase == 1) {
            if (!MainMenu.phoneMode) {
                // Mouse click for phase 2 attack 1,2, 3
                // Mouse click for all of phase2
                OnClickDoSomethingAttackPattern1(hit, badTargetNumber);
                OnClickDoSomethingAttackPattern1(hit2, badTargetNumber2);
                OnClickDoSomethingAttackPattern2();
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            else {
                // Mouse click for phase 2 attack 1,2, 3
                // Mouse click for all of phase2
                OnTouchDoSomethingAttackPattern1(hit, badTargetNumber);
                OnTouchDoSomethingAttackPattern1(hit2, badTargetNumber2);
                OnTouchDoSomethingAttackPattern2();
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            attack3Phase1 = false;
            attack3Phase2 = false;
            attack3Phase3 = false;
        }
        else {
            if (!MainMenu.phoneMode) {

                // Mouse click for phase 2 attack 1,2,3
                // Mouse click for all of phase2
                OnClickDoSomethingAttackPattern1(hit, badTargetNumber);
                OnClickDoSomethingAttackPattern1(hit2, badTargetNumber2);
                OnClickDoSomethingAttackPattern1(hit3, badTargetNumber3);
                OnClickDoSomethingAttackPattern1(hit4, badTargetNumber4);
                OnClickDoSomethingAttackPattern2();
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            else {
                // Mouse click for phase 2 attack 1,2,3
                // Mouse click for all of phase2
                OnTouchDoSomethingAttackPattern1(hit, badTargetNumber);
                OnTouchDoSomethingAttackPattern1(hit2, badTargetNumber2);
                OnTouchDoSomethingAttackPattern1(hit3, badTargetNumber3);
                OnTouchDoSomethingAttackPattern1(hit4, badTargetNumber4);
                OnTouchDoSomethingAttackPattern2();
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                //OnClickDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                for (int i = 0; i < attackPattern3Targets.Length; i++) {
                    if (attackPattern3Targets[i]) {
                        attack3Phase1 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets2.Length; i++) {
                    if (attackPattern3Targets2[i]) {
                        attack3Phase2 = true;
                    }
                }
                for (int i = 0; i < attackPattern3Targets3.Length; i++) {
                    if (attackPattern3Targets3[i]) {
                        attack3Phase3 = true;
                    }
                }
                if (attack3Phase1) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets, phase3SpawnOrder);
                }
                else if (attack3Phase2) {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets2, phase3SpawnOrder2);
                }
                else {
                    OnTouchDoSomethingAttackPattern3(attackPattern3Targets3, phase3SpawnOrder3);
                }
            }
            attack3Phase1 = false;
            attack3Phase2 = false;
            attack3Phase3 = false;
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

    // Spawn 4 targets in attack pattern 1
    private int AttackPattern1SpawnTargets(GameObject[] array, Vector3[] positions) {
        // Choose randomly which target is the bad target
        int random = Random.Range(0, array.Length);

        // Instantiate the hit targets
        for (int i = 0; i < array.Length; i++) {
            if (random == i) {
                array[i] = Instantiate(targetBad, positions[i], Quaternion.identity);
                array[i].transform.position = positions[i];
            }
            else {
                array[i] = Instantiate(target, positions[i], Quaternion.identity);
                array[i].transform.position = positions[i];
            }
        }

        return random;
    }

    // Destroys targets in attack pattern 1
    private void AttackPattern1DestroyTargets(GameObject[] array, int badTarget) {
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

    // Spawn attack 2 bad targets
    private void AttackPattern2SpawnBadTargets() {
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        transform.position.y,
                        0.0f
                        );
        targetFallingBadClone = Instantiate(targetFallingBad, spawnPosition, spawnRotation);
        targetFallingBadClone.transform.position = spawnPosition;
        //Physics2D.IgnoreCollision(targetFallingBadClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(targetFallingBadClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Spawn attack 2 good targets
    private void AttackPattern2SpawnGoodTargets() {
        spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        transform.position.y,
                        0.0f
                        );
        targetFallingClone = Instantiate(targetFalling, spawnPosition, spawnRotation);
        targetFallingClone.transform.position = spawnPosition;
        //Physics2D.IgnoreCollision(targetFallingClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(targetFallingClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Spawn attack 3 targets
    private void AttackPattern3ShuffleTargets(int[] spawnOrderArray) {
        // Shuffle the phase3SpawnOrder to randomize the values (Fisher Yates Shuffle)
        for (int i = 0; i < spawnOrderArray.Length; i++) {
            int temp = spawnOrderArray[i];
            int random = Random.Range(temp, spawnOrderArray.Length);
            spawnOrderArray[i] = spawnOrderArray[random];
            spawnOrderArray[random] = temp;
        }
        //return spawnOrderArray;
    }

    //
    private void AttackPattern3SpawnLockedTargets(GameObject[] targetsLocked, int[] spawnOrderArray, Vector3[] positions, int number) {
        targetsLocked[spawnOrderArray[number]] = Instantiate(targetLocked, positions[spawnOrderArray[number]], Quaternion.identity);
        targetsLocked[spawnOrderArray[number]].transform.position = positions[spawnOrderArray[number]];
        //Physics2D.IgnoreCollision(targetsLocked[spawnOrderArray[number]].GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        //Physics2D.IgnoreCollision(targetsLocked[spawnOrderArray[number]].GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    //
    private void AttackPattern3DestroyLockedTargets(GameObject[] targetsLocked) {
        for (int i = 0; i < targetsLocked.Length; i++) {
            if (targetsLocked[i]) {
                Destroy(targetsLocked[i]);
            }
        }
    }

    //
    private void AttackPattern3SpawnTargets(GameObject[] targetsArray, int[] spawnOrderArray, Vector3[] positions) {
        for (int i = 0; i < targetsArray.Length; i++) {
            targetsArray[spawnOrderArray[i]] = Instantiate(targetPhase3, positions[spawnOrderArray[i]], Quaternion.identity);
            targetsArray[spawnOrderArray[i]].transform.position = positions[spawnOrderArray[i]];
            //Physics2D.IgnoreCollision(targetsArray[spawnOrderArray[i]].GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
            //Physics2D.IgnoreCollision(targetsArray[spawnOrderArray[i]].GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
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

    // AttackPattern 3 Destroy Targets
    private void AttackPattern3DestroyTargets(GameObject[] targets) {
        for (int i = 0; i < targets.Length; i++) {
            if (targets[i]) {
                // Do boss animation on first attack only
                if (i == 0) {
                    BossAttack();
                }
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
        targetListBad = GameObject.FindGameObjectsWithTag("phase2_falling_bad");
        targetList = GameObject.FindGameObjectsWithTag("phase2_falling");

        for (int i = 0; i < targetList.Length; i++) {
            Destroy(targetList[i]);
        }
        for (int i = 0; i < targetListBad.Length; i++) {
            Destroy(targetListBad[i]);
        }
    }


    // Destroy All Targets Phase3
    private void DestroyAllTargetsAttackPattern3(GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i]) {
                Destroy(array[i]);
            }
        }
    }

    // Checks to see that there are no more GOOD targets, destroys all remaining targets
    private void DestroyLeftovers(GameObject[] array, int badTarget) {
        int temp = 0;
        for (int i = 0; i < array.Length; i++) {
            if (!array[i]) {
                if (array[i] != array[badTarget]) {
                    temp++;
                }
            }
        }
        if (temp >= array.Length - 1) {
            for (int i = 0; i < array.Length; i++) {
                Destroy(array[i]);
            }
        }
    }

    // On MOUSE click do something phase1
    private void OnClickDoSomethingAttackPattern1(GameObject[] array, int badTargetNumberRand) {
        // On click, do something
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

            // If target is clicked, destroy target
            if (hitTarget.collider != null) {
                // If bad target is clicked, destroy all other targets and damage player for # of targets destroyed
                if (hitTarget.collider.gameObject == array[badTargetNumberRand]) {
                    BossAttack();
                    for (int i = 0; i < array.Length; i++) {
                        if (array[i]) {
                            Destroy(array[i]);
                            player.GetComponent<PlayerHealthManager>().giveDamage(1);
                        }
                    }
                }

                else {
                    for (int i = 0; i < array.Length; i++) {
                        // if click matches target, destroy target, damage boss
                        if (hitTarget.collider.gameObject == array[i]) {
                            Destroy(array[i]);
                            PlayerAttack();
                            boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                        }
                    }
                }
            }
        }
    }

    // On MOUSE click do something phase2
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

    // On MOUSE click do something phase3
    private void OnClickDoSomethingAttackPattern3(GameObject[] targets, int[] spawnOrder) {
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
                        if (targets[spawnOrder[i]]) {
                            if (hitTarget.collider.gameObject != targets[spawnOrder[i]]) {
                                AttackPattern3IncorrectHit(targets);
                            // Break when target is found
                                break;
                            }
                            else {
                                Destroy(targets[spawnOrder[i]]);
                                PlayerAttack();
                                boss.GetComponent<EnemyHealthManager>().giveDamage(2);
                            // Break when target is found
                                break;
                            }
                        }
                    }
                //
            }
        }
    }
    
  
    // On PHONE TOUCH, do something
    private void OnTouchDoSomethingAttackPattern1(GameObject[] array, int badTargetNumberRand)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position),
                    Vector2.zero);

                // If target is clicked, destroy target
                if (hitTarget.collider != null)
                {
                    // If bad target is clicked, destroy all other targets and damage player for # of targets destroyed
                    if (hitTarget.collider.gameObject == array[badTargetNumberRand])
                    {
                        BossAttack();
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (array[j])
                            {
                                Destroy(array[j]);
                                player.GetComponent<PlayerHealthManager>().giveDamage(1);
                            }
                        }
                    }

                    else
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            // if click matches target, destroy target, damage boss
                            if (hitTarget.collider.gameObject == array[j])
                            {
                                Destroy(array[j]);
                                PlayerAttack();
                                boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                            }
                        }
                    }
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

    // On PHONE TOUCH do something phase3
    private void OnTouchDoSomethingAttackPattern3(GameObject[] targets, int[] spawnOrder)
    {
        // Attack pattern 3 Mouse Down
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
                    for (int j = 0; i < targets.Length; j++)
                    {
                        if (targets[spawnOrder[j]])
                        {
                            if (hitTarget.collider.gameObject != targets[spawnOrder[j]])
                            {
                                AttackPattern3IncorrectHit(targets);
                                // Break when target is found
                                break;
                            }
                            else
                            {
                                Destroy(targets[spawnOrder[j]]);
                                PlayerAttack();
                                boss.GetComponent<EnemyHealthManager>().giveDamage(2);
                                // Break when target is found
                                break;
                            }
                        }
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
