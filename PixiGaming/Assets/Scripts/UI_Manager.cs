using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Slider healthSlider;

    public Button resetButton;
    public Button autoplayButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void AverageScore(int score)
    {
        scoreText.text = "Average Score: " + score;
    }

    public void ChangeHealth(float healthPercentage)
    {
        healthSlider.value = healthPercentage;
    }
}
