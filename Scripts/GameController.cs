using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject target;
    public GameObject targetBad;
    public GameObject targetFalling;
    public GameObject targetLocked;
    private GameObject targetMovingClone;
    private GameObject targetMovingBadClone;
    private GameObject[] hit;
    private GameObject[] phase3Targets;
    private GameObject[] phase3TargetsLocked;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;

    public int bossHealth;
    public int playerHealth;
    private int numberOfHitMarkers = 4;
    private int numberOfPhase3Targets = 4;
    private int badTargetNumber;
    private int targetMovingBadNumber;
    private int phase;
    private int comboCount;
    private int phase3RandomNumber;
    private int[] attackPattern3SpawnOrder;

    public float timeLeft;
    private float phaseTimeLeft;
    private float maxWidthScreen;
    private float skullWidth;

    public Text timerText;
    public Text bossHealthText;
    public Text playerHealthText;
    public Text gameOverText;
    public Text youWinText;

    private Animator playerAnimation;
    private Animator bossAnimation;

    private Vector3[] position = { new Vector3 { x = -4, y = 3, z = 0 },
                                  new Vector3 { x = -1.25f, y = 3, z = 0 },
                                  new Vector3 { x = 1.25f, y = 3, z = 0 },
                                  new Vector3 { x = 4, y = 3, z = 0 } };
    private Vector3[] phase3Positions = { new Vector3 { x = -4, y = 3, z = 0 },
                                  new Vector3 { x = -1.25f, y = 3, z = 0 },
                                  new Vector3 { x = 1.25f, y = 3, z = 0 },
                                  new Vector3 { x = 4, y = 3, z = 0 } };
    private Vector3 strikePositionTarget;
    private Vector3 strikePositionPlayer;
    private Vector3 upperCornerScreen;
    private Vector3 targetWidth;
    private Vector3 spawnPosition;

    private Quaternion spawnRotation;

    // Use this for initialization
    void Start () {
        if (cam == null) {
            cam = Camera.main;
        }
        strikePositionTarget = new Vector3(2.1f, -1f, 0);
        strikePositionPlayer = new Vector3(-4.64f, -4.24f, 0);
        playerAnimation = player.GetComponent<Animator>();
        bossAnimation = boss.GetComponent<Animator>();
        hit = new GameObject[numberOfHitMarkers];
        phase3Targets = new GameObject[numberOfPhase3Targets];
        phase3TargetsLocked = new GameObject[numberOfPhase3Targets];
        attackPattern3SpawnOrder = new int[]{ 0, 1, 2, 3};
        spawnRotation = Quaternion.identity;
        //phase = Random.Range(0,3);
        phase = 1;
        comboCount = 0;
        // Set screen boundaries
        upperCornerScreen = new Vector3(Screen.width, Screen.height, 0.0f);
        targetWidth = cam.ScreenToWorldPoint(upperCornerScreen);
        skullWidth = target.GetComponent<Renderer>().bounds.extents.x;
        maxWidthScreen = targetWidth.x - skullWidth;;

        if (phase == 0) {
            StartCoroutine(Phase1());
        }
        else if (phase == 1) {
            StartCoroutine(Phase2());
        }
        else {
            StartCoroutine(Phase3());
        }
    }

    // Update timers and texts
    private void FixedUpdate() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            timeLeft = 0;
        }
        UpdateText();
        phaseTimeLeft -= Time.deltaTime;
        if (phaseTimeLeft <= 0) {
            StopAllCoroutines();
            phaseTimeLeft = 12;
            //phase = Random.Range(0, 3);
            bossAnimation.SetTrigger("skill_2");
            phase = 1;
            if(phase == 0) {
                StartCoroutine(Phase1());
            }
            else if(phase == 1) {
                StartCoroutine(Phase2());
            }
            else {
                StartCoroutine(Phase3());
            }
        }
    }

    // Phase 1
    IEnumerator Phase1() {
        yield return new WaitForSeconds(1);
        while (bossHealth > 0 && timeLeft > 0 && playerHealth > 0 && phase == 0) {
            if (!hit[0] && !hit[1] && !hit[2] && !hit[3]) {
                // Wait 1 second before spawning next targets
                yield return new WaitForSeconds(1);
                Phase1SpawnTargets();
            }

            else {
                // After 3 seconds destroy all targets and damage player for # of targets destroyed
                yield return new WaitForSeconds(2);
                Phase1DestroyTargets();
            }
        }
    }

    // Phase 2
    IEnumerator Phase2() {
        yield return new WaitForSeconds(1);
        while (bossHealth > 0 && timeLeft > 0 && playerHealth > 0 && phase == 1) {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            spawnPosition = new Vector3(
                        Random.Range(-maxWidthScreen, maxWidthScreen),
                        transform.position.y,
                        0.0f
                        );
            targetMovingBadNumber = Random.Range(1, 4);

            // If random number is the last number (3) in this case, spawn bad target
            if (targetMovingBadNumber == 3) {
                Phase2SpawnBadTargets();
            }
            else {
                Phase2SpawnGoodTargets();
            }
        }
    }

    // Phase 3
    IEnumerator Phase3() {
        yield return new WaitForSeconds(1);
        while (bossHealth > 0 && timeLeft > 0 && playerHealth > 0 && phase == 2) {
            if (!phase3Targets[0] && !phase3Targets[1] && !phase3Targets[2] && !phase3Targets[3]) {
                // Shuffle the attackPattern3SpawnOrder to randomize the values (Fisher Yates Shuffle)
                for (int i = 0; i < phase3Targets.Length; i++) {
                    int temp = attackPattern3SpawnOrder[i];
                    phase3RandomNumber = Random.Range(temp, attackPattern3SpawnOrder.Length);
                    attackPattern3SpawnOrder[i] = attackPattern3SpawnOrder[phase3RandomNumber];
                    attackPattern3SpawnOrder[phase3RandomNumber] = temp;
                }
                for (int i = 0; i < phase3Targets.Length; i++) {
                    yield return new WaitForSeconds(1);
                    phase3TargetsLocked[attackPattern3SpawnOrder[i]] = Instantiate(targetLocked, phase3Positions[attackPattern3SpawnOrder[i]], Quaternion.identity);
                    phase3TargetsLocked[attackPattern3SpawnOrder[i]].transform.position = phase3Positions[attackPattern3SpawnOrder[i]];
                    Physics2D.IgnoreCollision(phase3TargetsLocked[attackPattern3SpawnOrder[i]].GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(phase3TargetsLocked[attackPattern3SpawnOrder[i]].GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
                }
                yield return new WaitForSeconds(1);
                for (int i = 0; i < phase3TargetsLocked.Length; i++) {
                    if (phase3TargetsLocked[i]) {
                        Destroy(phase3TargetsLocked[i]);
                    }
                }
                for (int i = 0; i < phase3Targets.Length; i++) {
                    phase3Targets[attackPattern3SpawnOrder[i]] = Instantiate(target, phase3Positions[attackPattern3SpawnOrder[i]], Quaternion.identity);
                    phase3Targets[attackPattern3SpawnOrder[i]].transform.position = phase3Positions[attackPattern3SpawnOrder[i]];
                    Physics2D.IgnoreCollision(phase3Targets[attackPattern3SpawnOrder[i]].GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(phase3Targets[attackPattern3SpawnOrder[i]].GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
                }
            }
            else {
                yield return new WaitForSeconds(5);
                Phase3DestroyTargets();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (bossHealth <= 0) {
            bossHealth = 0;
            bossAnimation.SetTrigger("death");
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargets();
            SceneManager.LoadScene("level3_EPB");
        }
        if (playerHealth <= 0) {
            playerHealth = 0;
            playerAnimation.SetTrigger("death");
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargets();
            gameOverText.gameObject.SetActive(true);
        }

            //Phase 1
            // On Mouse click, do something
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero);

                // If target is clicked, destroy target
                if (hitTarget.collider != null) {
                    for (int i = 0; i < numberOfHitMarkers; i++) {
                        if (hitTarget.collider.gameObject == hit[i]) {
                            Destroy(hit[i]);
                            PlayerAttack();
                            bossHealth -= 1;
                            comboCount++;
                        }
                    }

                    // If bad target is clicked, destroy all other targets and damage player for # of targets destroyed
                    if (hitTarget.collider.gameObject == hit[badTargetNumber]) {
                        BossAttack();
                        for (int i = 0; i < numberOfHitMarkers; i++) {
                            if (hit[i]) {
                                Destroy(hit[i]);
                                playerHealth -= 1;
                                comboCount = 0;
                            }
                        }
                    }
                }
            }
            // Outside of left mouse down, if bad target is last one left, destroy it
            if(comboCount >= 3) {
                for(int i = 0; i < numberOfHitMarkers; i++) {
                    Destroy(hit[i]);
                }
                comboCount = 0;
            }

            // Phase 2
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero);
                // If target is clicked, destroy target
                if (hitTarget.collider != null) {
                    if (hitTarget.collider.gameObject == targetMovingClone) {
                        Destroy(targetMovingClone);
                        PlayerAttack();
                        bossHealth -= 1;
                    }
                    else if((hitTarget.collider.gameObject == targetMovingBadClone)){
                        Destroy(targetMovingBadClone);
                        BossAttack();
                        playerHealth -= 1;
                    }
                }
            }

            // Phase 3
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hitTarget = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero);

                // If target is clicked, destroy target
                if (hitTarget.collider != null) {
                    if (phase3Targets[attackPattern3SpawnOrder[0]]) {
                        if (hitTarget.collider.gameObject != phase3Targets[attackPattern3SpawnOrder[0]]) {
                            Phase3IncorrectHit();
                        }
                        else {
                            Destroy(phase3Targets[attackPattern3SpawnOrder[0]]);
                            PlayerAttack();
                            bossHealth -= 1;
                        }
                    }
                    else if (phase3Targets[attackPattern3SpawnOrder[1]]) {
                        if (hitTarget.collider.gameObject != phase3Targets[attackPattern3SpawnOrder[1]]) {
                            Phase3IncorrectHit();
                    }
                        else {
                            Destroy(phase3Targets[attackPattern3SpawnOrder[1]]);
                            PlayerAttack();
                            bossHealth -= 1;
                        }
                    }
                    else if (phase3Targets[attackPattern3SpawnOrder[2]]) {
                        if (hitTarget.collider.gameObject != phase3Targets[attackPattern3SpawnOrder[2]]) {
                            Phase3IncorrectHit();
                    }
                        else {
                            Destroy(phase3Targets[attackPattern3SpawnOrder[2]]);
                            PlayerAttack();
                            bossHealth -= 1;
                        }
                    }
                    else {
                        if (hitTarget.collider.gameObject != phase3Targets[attackPattern3SpawnOrder[3]]) {
                            Phase3IncorrectHit();
                    }
                        else {
                            Destroy(phase3Targets[attackPattern3SpawnOrder[3]]);
                            PlayerAttack();
                            bossHealth -= 1;
                        }
                    }
                }
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

    // If target reaches bottom of stage, damage player
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == targetMovingClone) {
            playerHealth--;
            BossAttack();
        }
        Destroy(other.gameObject);
    }

    // Spawn 4 targets in phase 1
    private void Phase1SpawnTargets() {
        // Choose randomly which target is the bad target
        badTargetNumber = Random.Range(0, 4);

        // Instantiate the hit targets
        for (int i = 0; i < numberOfHitMarkers; i++) {
            if (badTargetNumber == i) {
                hit[i] = Instantiate(targetBad, position[i], Quaternion.identity);
                hit[i].transform.position = position[i];
                hit[i].GetComponent<Renderer>().material.color = Color.magenta;
            }
            else {
                hit[i] = Instantiate(target, position[i], Quaternion.identity);
                hit[i].transform.position = position[i];
            }
        }
    }

    // Destroys targets in phase 1
    private void Phase1DestroyTargets() {
        for (int i = 0; i < numberOfHitMarkers; i++) {
            if (hit[i]) {
                // Do boss animation on first attack only
                if (i == 0) {
                    BossAttack();
                }
                Destroy(hit[i]);
                playerHealth--;
                comboCount = 0;
            }
        }
    }

    // Spawn phase 2 bad targets
    private void Phase2SpawnBadTargets() {
        targetMovingBadClone = Instantiate(targetFalling, spawnPosition, spawnRotation);
        targetMovingBadClone.transform.position = spawnPosition;
        targetMovingBadClone.GetComponent<Renderer>().material.color = Color.magenta;
        Physics2D.IgnoreCollision(targetMovingBadClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(targetMovingBadClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Spawn phase 2 good targets
    private void Phase2SpawnGoodTargets() {
        targetMovingClone = Instantiate(targetFalling, spawnPosition, spawnRotation);
        targetMovingClone.transform.position = spawnPosition;
        Physics2D.IgnoreCollision(targetMovingClone.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(targetMovingClone.GetComponent<Collider2D>(), targetBad.GetComponent<Collider2D>());
    }

    // Phase 3 Incorrect Hit
    private void Phase3IncorrectHit() {
        BossAttack();
        for (int i = 0; i < phase3Targets.Length; i++) {
            if (phase3Targets[i]) {
                Destroy(phase3Targets[i]);
                playerHealth -= 1;
                comboCount = 0;
            }
        }
    }

    // Phase 3 Destroy Targets
    private void Phase3DestroyTargets() {
        for (int i = 0; i < numberOfHitMarkers; i++) {
            if (phase3Targets[i]) {
                // Do boss animation on first attack only
                if (i == 0) {
                    BossAttack();
                }
                Destroy(phase3Targets[i]);
                playerHealth--;
                comboCount = 0;
            }
        }
    }

    private void DestroyAllTargets() {
        for (int i = 0; i < numberOfHitMarkers; i++) {
            if (hit[i]) {
                Destroy(hit[i]);
            }
            if (phase3Targets[i]) {
                Destroy(phase3Targets[i]);
            }
        }
        if (targetMovingBadClone) {
            Destroy(targetMovingBadClone);
        }
        if (targetMovingClone) {
            Destroy(targetMovingClone);
        }
    }

    // Update scores and text
    void UpdateText() {
        timerText.text = "Time Left:\n" + Mathf.RoundToInt(timeLeft);
        bossHealthText.text = "Boss Health:\n" + Mathf.RoundToInt(bossHealth);
        playerHealthText.text = "Player Health: \n" + Mathf.RoundToInt(playerHealth);
    }

}
