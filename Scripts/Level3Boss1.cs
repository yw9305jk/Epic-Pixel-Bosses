using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Level3Boss1 : MonoBehaviour {

    public Camera cam;

    public GameObject player;
    public GameObject boss;
    public GameObject strikeTarget;
    public GameObject strikePlayer;
    public GameObject skullBlue;
    public GameObject skullGrey;
    public GameObject frozen;
    private GameObject[] hit1;
    private GameObject[] hit2;
    private GameObject[] hit3;
    private GameObject[] hit4;
    private GameObject[] frozen1;
    private GameObject[] frozen2;
    private GameObject[] frozen3;
    private GameObject[] frozen4;
    private GameObject strikeTargetClone;
    private GameObject strikePlayerClone;

    private int numberOfHitMarkers;
    //private int positionMarkers;
    private int badTargetNumber1;
    private int badTargetNumber2;
    private int badTargetNumber3;
    private int badTargetNumber4;
    private int attackPattern;
    private int phase;
    private int twoThirdsHealth;
    private int oneThirdHealth;

    public float timeLeft;
    private float phaseTimeLeft;
    //private float maxWidthScreen;
    //private float skullWidth;

    public Text timerText;
    public Text bossHealthText;
    public Text playerHealthText;
    public Text gameOverText;
    public Text youWinText;

    public Button restartButton;

    private Animator playerAnimation;
    public static Animator bossAnimation;

    private Vector3[] position1 = { new Vector3 { x = -6, y = 4.5f, z = 0 },
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

    private Vector3 strikePositionTarget;
    private Vector3 strikePositionPlayer;
    //private Vector3 upperCornerScreen;
    //private Vector3 targetWidth;
    private Vector3 spawnPosition;

    //private bool noMoreHitTargets;
    private bool arrayIsEmpty;
    private bool arrayIsEmpty2;
    private bool arrayIsEmpty3;
    private bool arrayIsEmpty4;
    //private bool targetExists;
    //private bool targetBadExists;
    public bool audioAlreadyPlayed;

    public AudioSource gameOverAudioSource;

    public AudioClip gameOverAudioClip;

    // Use this for initialization
    void Start() {
        if (cam == null) {
            cam = Camera.main;
        }
        numberOfHitMarkers = 4;
        //positionMarkers = 4;
        //noMoreHitTargets = false;
        arrayIsEmpty = true;
        arrayIsEmpty2 = true;
        arrayIsEmpty3 = true;
        arrayIsEmpty4 = true;
        //targetExists = false;
        //targetBadExists = false;
        strikePositionTarget = new Vector3(3.8f, -1.4f, 0);
        strikePositionPlayer = new Vector3(-4.64f, -4.24f, 0);
        playerAnimation = player.GetComponent<Animator>();
        bossAnimation = boss.GetComponent<Animator>();
        hit1 = new GameObject[numberOfHitMarkers];
        hit2 = new GameObject[numberOfHitMarkers];
        hit3 = new GameObject[numberOfHitMarkers];
        hit4 = new GameObject[numberOfHitMarkers];
        frozen1 = new GameObject[numberOfHitMarkers];
        frozen2 = new GameObject[numberOfHitMarkers];
        frozen3 = new GameObject[numberOfHitMarkers];
        frozen4 = new GameObject[numberOfHitMarkers];
        //attackPattern= Random.Range(0,3);
        attackPattern = 0;
        phase = 0;
        oneThirdHealth = boss.GetComponent<EnemyHealthManager>().bossHealth / 3;
        twoThirdsHealth = oneThirdHealth * 2;
        // Set screen boundaries
        //upperCornerScreen = new Vector3(Screen.width, Screen.height, 0.0f);
        //targetWidth = cam.ScreenToWorldPoint(upperCornerScreen);
        //skullWidth = target.GetComponent<Renderer>().bounds.extents.x;
        //maxWidthScreen = targetWidth.x - skullWidth; ;
        audioAlreadyPlayed = false;

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

    // Phase 1
    IEnumerator AttackPattern1() {
        yield return new WaitForSeconds(1);
        while (boss.GetComponent<EnemyHealthManager>().bossHealth > 0
                && timeLeft > 0
                && PlayerHealthManager.playerHealth > 0
                && attackPattern == 0) {

            // Phase 1
            if (phase == 0) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit1, arrayIsEmpty);

                if (arrayIsEmpty) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    yield return new WaitForSeconds(1);
                    badTargetNumber1 = AttackPattern1SpawnTargets(hit1, position1);
                    AttackPattern1SpawnFrozen(frozen1, position1);
                }

                else {
                    // After 2 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(3);
                    AttackPattern1DestroyTargets(hit1, badTargetNumber1);
                    AttackPattern1DestroyFrozen(frozen1);
                }
            }

            // Phase 2
            else if (phase == 1) {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit1, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(hit2, arrayIsEmpty2);

                if (arrayIsEmpty && arrayIsEmpty2) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    yield return new WaitForSeconds(1);
                    badTargetNumber1 = AttackPattern1SpawnTargets(hit1, position1);
                    badTargetNumber2 = AttackPattern1SpawnTargets(hit2, position2);
                    AttackPattern1SpawnFrozen(frozen1, position1);
                    AttackPattern1SpawnFrozen(frozen2, position2);
                }

                else {
                    // After 3 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(4);
                    AttackPattern1DestroyTargets(hit1, badTargetNumber1);
                    AttackPattern1DestroyTargets(hit2, badTargetNumber2);
                    AttackPattern1DestroyFrozen(frozen1);
                    AttackPattern1DestroyFrozen(frozen2);
                }
            }

            // Phase 3
            else {
                // Check if array is empty, if it is, populate the stage
                arrayIsEmpty = ArrayIsEmpty(hit1, arrayIsEmpty);
                arrayIsEmpty2 = ArrayIsEmpty(hit2, arrayIsEmpty2);
                arrayIsEmpty3 = ArrayIsEmpty(hit3, arrayIsEmpty3);
                arrayIsEmpty4 = ArrayIsEmpty(hit4, arrayIsEmpty4);

                if (arrayIsEmpty && arrayIsEmpty2 && arrayIsEmpty3 && arrayIsEmpty4) {
                    // Wait 1 second before spawning next targets
                    bossAnimation.SetTrigger("skill_2");
                    yield return new WaitForSeconds(1);
                    badTargetNumber1 = AttackPattern1SpawnTargets(hit1, position1);
                    badTargetNumber2 = AttackPattern1SpawnTargets(hit2, position2);
                    badTargetNumber3 = AttackPattern1SpawnTargets(hit3, position3);
                    badTargetNumber4 = AttackPattern1SpawnTargets(hit4, position4);
                    AttackPattern1SpawnFrozen(frozen1, position1);
                    AttackPattern1SpawnFrozen(frozen2, position2);
                    AttackPattern1SpawnFrozen(frozen3, position3);
                    AttackPattern1SpawnFrozen(frozen4, position4);
                }

                else {
                    // After 4 seconds destroy all targets and damage player for # of targets destroyed
                    yield return new WaitForSeconds(6);
                    AttackPattern1DestroyTargets(hit1, badTargetNumber1);
                    AttackPattern1DestroyTargets(hit2, badTargetNumber2);
                    AttackPattern1DestroyTargets(hit3, badTargetNumber3);
                    AttackPattern1DestroyTargets(hit4, badTargetNumber4);
                    AttackPattern1DestroyFrozen(frozen1);
                    AttackPattern1DestroyFrozen(frozen2);
                    AttackPattern1DestroyFrozen(frozen3);
                    AttackPattern1DestroyFrozen(frozen4);
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
            DestroyAllTargetsPhase1(hit1);
            DestroyAllTargetsPhase1(hit2);
            DestroyAllTargetsPhase1(hit3);
            DestroyAllTargetsPhase1(hit4);
            DestroyAllTargetsPhase1(frozen1);
            DestroyAllTargetsPhase1(frozen2);
            DestroyAllTargetsPhase1(frozen3);
            DestroyAllTargetsPhase1(frozen4);
            Invoke("nextBoss", 2);
        }
        if (PlayerHealthManager.playerHealth <= 0) {
            PlayerHealthManager.playerHealth = 0;
            playerAnimation.SetTrigger("death");
            // Destroy remaining targets
            StopAllCoroutines();
            DestroyAllTargetsPhase1(hit1);
            DestroyAllTargetsPhase1(hit2);
            DestroyAllTargetsPhase1(hit3);
            DestroyAllTargetsPhase1(hit4);
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
            DestroyAllTargetsPhase1(hit1);
            DestroyAllTargetsPhase1(hit2);
            DestroyAllTargetsPhase1(hit3);
            DestroyAllTargetsPhase1(hit4);
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

        // Start phases
        if (phase == 0) {
            if (!MainMenu.phoneMode) {
                OnClickDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);

            }
            else {
                OnTouchDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);
            }

            //DestroyLeftovers(hit, badTargetNumber);
        }
        else if (phase == 1) {
            if (!MainMenu.phoneMode) {
                OnClickDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);
                OnClickDoSomethingAttackPattern1(hit2, frozen2, badTargetNumber2);
            }
            else {
                OnTouchDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);
                OnTouchDoSomethingAttackPattern1(hit2, frozen2, badTargetNumber2);
            }
            //DestroyLeftovers(hit, badTargetNumber);
            //DestroyLeftovers(hit2, badTargetNumber2);
        }
        else {
            if (!MainMenu.phoneMode) {
                OnClickDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);
                OnClickDoSomethingAttackPattern1(hit2, frozen2, badTargetNumber2);
                OnClickDoSomethingAttackPattern1(hit3, frozen3, badTargetNumber3);
                OnClickDoSomethingAttackPattern1(hit4, frozen4, badTargetNumber4);
            }
            else {
                OnTouchDoSomethingAttackPattern1(hit1, frozen1, badTargetNumber1);
                OnTouchDoSomethingAttackPattern1(hit2, frozen2, badTargetNumber2);
                OnTouchDoSomethingAttackPattern1(hit3, frozen3, badTargetNumber3);
                OnTouchDoSomethingAttackPattern1(hit4, frozen4, badTargetNumber4);
            }
            //DestroyLeftovers(hit, badTargetNumber);
            //DestroyLeftovers(hit2, badTargetNumber2);
            //DestroyLeftovers(hit3, badTargetNumber3);
            //DestroyLeftovers(hit4, badTargetNumber4);
        }
    }

    // Animate Boss Attack
    public void BossAttack() {
        bossAnimation.SetTrigger("skill_1");
        playerAnimation.SetTrigger("hit_1");
        strikePlayerClone = Instantiate(strikePlayer, strikePositionPlayer, Quaternion.identity);
        strikePlayerClone.transform.position = strikePositionPlayer;
        Destroy(strikePlayerClone, 1f);
    }

    // Animate Player Attack
    public void PlayerAttack() {
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
                array[i] = Instantiate(skullGrey, positions[i], Quaternion.identity);
                array[i].transform.position = positions[i];
            }
            else {
                array[i] = Instantiate(skullBlue, positions[i], Quaternion.identity);
                array[i].transform.position = positions[i];
            }
        }

        return random;
    }

    // Spawn 4 frozen targets in attack pattern 1
    private int AttackPattern1SpawnFrozen(GameObject[] array, Vector3[] positions) {
        // Choose randomly which target is the bad target
        int random = Random.Range(0, array.Length);

        // Instantiate the hit targets
        for (int i = 0; i < array.Length; i++) {
            if (random == i) {
                array[i] = Instantiate(frozen, positions[i], Quaternion.identity);
                array[i].transform.position = positions[i];
            }
            else {
                array[i] = Instantiate(frozen, positions[i], Quaternion.identity);
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

    // Destroys frozen targets in attack pattern 1
    private void AttackPattern1DestroyFrozen(GameObject[] array) {
        for (int i = 0; i < array.Length; i++) {
                Destroy(array[i]);
        }
    }

    // Go to next boss
    private void nextBoss() {
        SceneManager.LoadScene("EPB_level3_boss2");
    }

    // Destroy All Targets
    private void DestroyAllTargetsPhase1(GameObject[] array) {
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

        // On MOUSE click do something
        private void OnClickDoSomethingAttackPattern1(GameObject[] hitArray, GameObject[] frozenArray, int badTargetNumberRand) {
        // On click, do something
        if (Input.GetMouseButtonDown(0)) {
             RaycastHit2D[] hitTarget = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                      Vector2.zero);

            // Check for length of hit array (Meaning there is a frozen and a skull)
            if (hitTarget.Length > 0) {
                // If there is a frozen, since length is > 1, find frozen and destroy it
                if(hitTarget.Length > 1) {
                    if (hitTarget[0].collider.gameObject.tag == "frozen") {
                        for (int j = 0; j < hitArray.Length; j++) {
                            if (hitTarget[0].collider.gameObject == frozenArray[j]) {
                                Destroy(frozenArray[j]);
                            }
                        }
                    }
                    else if (hitTarget[1].collider.gameObject.tag == "frozen") {
                        for (int j = 0; j < hitArray.Length; j++) {
                            if (hitTarget[1].collider.gameObject == frozenArray[j]) {
                                Destroy(frozenArray[j]);
                            }
                        }
                    }
                    else {
                        if (hitTarget[1].collider.gameObject == hitArray[badTargetNumberRand]) {
                            BossAttack();
                            for (int j = 0; j < hitArray.Length; j++) {
                                if (hitArray[j]) {
                                    Destroy(hitArray[j]);
                                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                                    if (frozenArray[j]) {
                                        Destroy(frozenArray[j]);
                                    }
                                }
                            }
                        }

                        else {
                            for (int j = 0; j < hitArray.Length; j++) {
                                // if click matches target, destroy target, damage boss
                                if (hitTarget[1].collider.gameObject == hitArray[j]) {
                                    Destroy(hitArray[j]);
                                    PlayerAttack();
                                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                                }
                            }
                        }
                    }
                }
                // Else there is no frozen so just destroy the skull
                else {
                    if (hitTarget[0].collider.gameObject.tag == "frozen") {
                        for (int j = 0; j < hitArray.Length; j++) {
                            if (hitTarget[0].collider.gameObject == frozenArray[j]) {
                                Destroy(frozenArray[j]);
                            }
                        }
                    }
                    else {
                        if (hitTarget[0].collider.gameObject == hitArray[badTargetNumberRand]) {
                            BossAttack();
                            for (int j = 0; j < hitArray.Length; j++) {
                                if (hitArray[j]) {
                                    Destroy(hitArray[j]);
                                    player.GetComponent<PlayerHealthManager>().giveDamage(1);
                                    if (frozenArray[j]) {
                                        Destroy(frozenArray[j]);
                                    }
                                }
                            }
                        }

                        else {
                            for (int j = 0; j < hitArray.Length; j++) {
                                // if click matches target, destroy target, damage boss
                                if (hitTarget[0].collider.gameObject == hitArray[j]) {
                                    Destroy(hitArray[j]);
                                    PlayerAttack();
                                    boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    // On PHONE TOUCH, do something
    private void OnTouchDoSomethingAttackPattern1(GameObject[] hitArray, GameObject[] frozenArray, int badTargetNumberRand) {
        for (int i = 0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                RaycastHit2D[] hitTarget = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                      Vector2.zero);

                // Check for length of hit array (Meaning there is a frozen and a skull)
                if (hitTarget.Length > 0) {
                    // If there is a frozen, since length is > 1, find frozen and destroy it
                    if (hitTarget.Length > 1) {
                        if (hitTarget[0].collider.gameObject.tag == "frozen") {
                            for (int j = 0; j < hitArray.Length; j++) {
                                if (hitTarget[0].collider.gameObject == frozenArray[j]) {
                                    Destroy(frozenArray[j]);
                                }
                            }
                        }
                        else if (hitTarget[1].collider.gameObject.tag == "frozen") {
                            for (int j = 0; j < hitArray.Length; j++) {
                                if (hitTarget[1].collider.gameObject == frozenArray[j]) {
                                    Destroy(frozenArray[j]);
                                }
                            }
                        }
                        else {
                            if (hitTarget[1].collider.gameObject == hitArray[badTargetNumberRand]) {
                                BossAttack();
                                for (int j = 0; j < hitArray.Length; j++) {
                                    if (hitArray[j]) {
                                        Destroy(hitArray[j]);
                                        player.GetComponent<PlayerHealthManager>().giveDamage(1);
                                        if (frozenArray[j]) {
                                            Destroy(frozenArray[j]);
                                        }
                                    }
                                }
                            }

                            else {
                                for (int j = 0; j < hitArray.Length; j++) {
                                    // if click matches target, destroy target, damage boss
                                    if (hitTarget[1].collider.gameObject == hitArray[j]) {
                                        Destroy(hitArray[j]);
                                        PlayerAttack();
                                        boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                                    }
                                }
                            }
                        }
                    }
                    // Else there is no frozen so just destroy the skull
                    else {
                        if (hitTarget[0].collider.gameObject.tag == "frozen") {
                            for (int j = 0; j < hitArray.Length; j++) {
                                if (hitTarget[0].collider.gameObject == frozenArray[j]) {
                                    Destroy(frozenArray[j]);
                                }
                            }
                        }
                        else {
                            if (hitTarget[0].collider.gameObject == hitArray[badTargetNumberRand]) {
                                BossAttack();
                                for (int j = 0; j < hitArray.Length; j++) {
                                    if (hitArray[j]) {
                                        Destroy(hitArray[j]);
                                        player.GetComponent<PlayerHealthManager>().giveDamage(1);
                                        if (frozenArray[j]) {
                                            Destroy(frozenArray[j]);
                                        }
                                    }
                                }
                            }

                            else {
                                for (int j = 0; j < hitArray.Length; j++) {
                                    // if click matches target, destroy target, damage boss
                                    if (hitTarget[0].collider.gameObject == hitArray[j]) {
                                        Destroy(hitArray[j]);
                                        PlayerAttack();
                                        boss.GetComponent<EnemyHealthManager>().giveDamage(1);
                                    }
                                }
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
