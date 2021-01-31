using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float MaxSpoilage;
    public float SpoilageMeter;
    public float SpoilRate;

    public Slider SpoilageBar;
    public GameObject GameOverScreen;

    [HideInInspector]
    public bool isGameOver = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        SpoilageBar.minValue = 0;
        SpoilageBar.maxValue = MaxSpoilage;
    }

    private void FixedUpdate()
    {
        UpdateSpoilage(-SpoilRate);
    }

    public void UpdateSpoilage(float change)
    {
        SpoilageMeter = (SpoilageMeter + change <= MaxSpoilage) ? SpoilageMeter + change : MaxSpoilage;

        UpdateUI();

        if (SpoilageMeter < 0)
        {
            isGameOver = true;
            StartCoroutine(UpdateAlpha());
            GameOverScreen.SetActive(true);
        }
    }

    private IEnumerator UpdateAlpha()
    {
        while (GameOverScreen.GetComponent<CanvasGroup>().alpha < 1)
        {
            Camera.main.orthographicSize += 0.5f;
            GameOverScreen.GetComponent<CanvasGroup>().alpha += 0.01f;
            yield break;
        }
    }

    private void UpdateUI()
    {
        SpoilageBar.value = SpoilageMeter;
    }
}
