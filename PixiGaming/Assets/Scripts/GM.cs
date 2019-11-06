using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GM : MonoBehaviour {

    #region Variables
    // prefabs
    [SerializeField]
    private UI_Manager ui;
    [Space()]
    [SerializeField]
    private GameObject bubblePrefab;
    public Material[] bubbleMaterials;
    [Space()]
    public GameObject[] powerups;

    List<GameObject> bubbles = new List<GameObject>();

    // Boundaries of the arena
    private Vector3 arenaMinAnchor = new Vector3(-1.5f, -1);
    private Vector3 arenaMaxAnchor = new Vector3(1.5f, 1);

    // Different Scale Multiplies (used as scale units)
    private float[] scaleUnit = new float[] { 0.5f, 1f,  1.5f }; 

    // Amount of bubble that are generated on startup
    private const int STARTUP_AMOUNT = 4;

    // Used to check if player is in a combo
    private float lastLocalScale = 0f;
    private int comboLength = 1;

    // Score related variables
    private int defaultScore = 10;
    private int comboMultiplier = 2;
    private int score;

    // Health
    private const float MAX_HEALTH = 1000;
    private const int STARTUP_DECRATE = 50;
    private float health = MAX_HEALTH;
    private int decRate = STARTUP_DECRATE;

    private float nextTimeToDecrease;
    private float nextTimeToChangeRate;

    private bool powerSplash = false;
    private bool gameOver = false;
    #endregion Variables

    // Use this for initialization
    void Start () {
        // initialize bubbles
        Init();
        // initialize health values
        nextTimeToDecrease = Time.time + 1;
        nextTimeToChangeRate = Time.time + 5;
	}
	
	// Update is called once per frame
	void Update () {

        HandleInput();
        HandleHealth();
    }

    void HandleInput()
    {
        if (gameOver) return;
        if (((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) || Input.GetMouseButtonDown(0))
        {
            Ray raycast = new Ray();
            try
            {
                raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            }
            catch
            {
                raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.CompareTag("Bubble"))
                {
                    Debug.Log("TappedBubble");
                    DestroyBubble(raycastHit.collider.gameObject);
                }
                else if (raycastHit.collider.CompareTag("Powerup"))
                {
                    GameObject powerup = raycastHit.collider.gameObject;

                    int powerupID = powerup.GetComponent<ID>().GetID;

                    Destroy(powerup);

                    if(powerupID == 0)
                    {
                        // bomb
                        GameObject[] bubblesToDestroy = bubbles.ToArray();
                        foreach(GameObject bubble in bubblesToDestroy)
                        {
                            DestroyBubble(bubble);
                        }
                    }
                    if(powerupID == 1)
                    {
                        // firstaid
                        ChangeHealth(20);
                    }
                    if(powerupID == 2)
                    {
                        // color splash
                        powerSplash = true;
                    }
                }
            }
        }
    }

    void HandleHealth()
    {
        if(Time.time >= nextTimeToDecrease)
        {
            nextTimeToDecrease = Time.time + 1;
            ChangeHealth(-decRate);
        }
        if(Time.time >= nextTimeToChangeRate)
        {
            nextTimeToChangeRate = Time.time + 5;
            decRate += 5;
        }
    }

    void ChangeHealth(float diff)
    {
        health += diff;
        float healthPercentage = health / MAX_HEALTH;
        if (health <= 0)
        {
            ui.resetButton.gameObject.SetActive(true);
            ui.autoplayButton.gameObject.SetActive(true);
            gameOver = true;
        }
        ui.ChangeHealth(healthPercentage);
        
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void AutoPlay()
    {
       // gameOver = false;
       // ui.autoplayButton.gameObject.SetActive(false);
       // ui.autoplayButton.gameObject.SetActive(false);
       //
       //
       // float autoPlayDiff = 0.05f;
       // int amountOfAutoPlays = 30;
       // int[] scores = new int[amountOfAutoPlays];
       //
       // float future = Time.time + autoPlayDiff;
       //
       // Time.timeScale = 10;
       //
       // for (int i = 0; i < amountOfAutoPlays; i++)
       // {
       //     GameObject[] bubblesToDestroy = bubbles.ToArray();
       //     foreach (GameObject bubble in bubblesToDestroy)
       //     {
       //         RemoveBubble(bubble);
       //     }
       //
       //     ui.autoplayButton.gameObject.SetActive(false);
       //     ui.autoplayButton.gameObject.SetActive(false);
       //
       //     score = 0;
       //     health = MAX_HEALTH;
       //
       //     Init();
       //
       //     while (true)
       //     {
       //         if (gameOver)
       //         {
       //             scores[i] = score;
       //             print("Game over");
       //             break;
       //         }
       //         if (Time.time >= future)
       //         {
       //             future = Time.time + autoPlayDiff;
       //             DestroyBubble(bubbles.ToArray()[0]);
       //             print("destroying bubble");
       //             print("bubbles: " + bubbles.ToArray().Length);
       //         }
       //     }
       // }
       // ui.resetButton.gameObject.SetActive(true);
       // ui.autoplayButton.gameObject.SetActive(true);
       // gameOver = true;
       //
       // int scoreAvg = 0;
       // for (int i = 0; i < scores.Length; i++)
       // {
       //     scoreAvg += scores[i];
       // }
       // scoreAvg /= scores.Length;
       // ui.AverageScore(scoreAvg);
    }

    void Init()
    {
        for (int i = 0; i < STARTUP_AMOUNT; i++)
        {
            CreateBubble();
        }
    }

    void CreateBubble()
    {
        // randomize position
        Vector3 position = new Vector3(Random.Range(arenaMinAnchor.x, arenaMaxAnchor.x), 2);
        // add to bubbles list
        bubbles.Add(Instantiate(bubblePrefab, position, Quaternion.Euler(Vector3.zero)));
        // randomize color
        bubbles[bubbles.Count - 1].GetComponent<Renderer>().material = bubbleMaterials[Random.Range(0, bubbleMaterials.Length)];
        // randomize scale
        bubbles[bubbles.Count - 1].GetComponent<Transform>().localScale *= scaleUnit[Random.Range(0, scaleUnit.Length)];
    }


    void DestroyBubble(GameObject bubble)
    {
        if (lastLocalScale == 0)
        {          
            score += defaultScore;
        }
        else
        {
            if (lastLocalScale == bubble.GetComponent<Transform>().localScale.magnitude)
            {
                comboLength++;
                score += defaultScore + comboMultiplier * comboLength;
            }
            else
            {
                comboLength = 1;
                score += defaultScore;                
            }
        }
        // Because I'm using Spheres scale could be measured by a float (radius is equal all across)
        lastLocalScale = bubble.GetComponent<Transform>().localScale.magnitude;

        // Destroy current bubble
        if (!powerSplash)
        {
            bubbles.Remove(bubble);
            Destroy(bubble);
        }
        // Special destruction
        if(powerSplash)
        {
            CollisionManager.IteratorMethod(bubble.GetComponent<Collider>(), bubble.GetComponent<Renderer>().material);
            GameObject[] bubblesToDestroy = CollisionManager.Colliders.ToArray();
            foreach (GameObject fittingBubble in bubblesToDestroy)
            {
                DestroyBubble(bubble);
            }
        }

        // Chance for powerup
        if(Random.Range(1,6) == 1)
        {
            Instantiate(powerups[Random.Range(0, powerups.Length)], bubble.transform.position, Quaternion.Euler(Vector3.zero));
        }

        // Display UI
        ui.ChangeScore(score);

        // Generate new bubble
        CreateBubble();
    }

    void RemoveBubble(GameObject bubble)
    {
        bubbles.Remove(bubble);
        Destroy(bubble);
    }

 
}
