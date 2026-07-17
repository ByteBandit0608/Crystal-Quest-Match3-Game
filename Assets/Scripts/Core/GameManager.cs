using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int score = 0;
    public int movesRemaining = 20;

    private bool isProcessing = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;

        UIManager.Instance.UpdateUI();

        Debug.Log($"Score: {score}");
    }

    public void UseMove()
    {
        movesRemaining--;

        UIManager.Instance.UpdateUI();

        Debug.Log($"Moves Left: {movesRemaining}");

        if (movesRemaining <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        UIManager.Instance.ShowGameOver();
    }

    public void ProcessMatches(List<Gem> matchedGems)
    {
        if (!isProcessing)
            StartCoroutine(ProcessMatchesRoutine(matchedGems));
    }

    IEnumerator ProcessMatchesRoutine(List<Gem> matchedGems)
    {
        isProcessing = true;

        while (matchedGems.Count > 0)
        {
            Gem voltCandidate = MatchDetector.Instance.voltGemCandidate;
            Debug.Log("Volt Candidate: " +
            (voltCandidate != null ? $"({voltCandidate.x},{voltCandidate.y})" : "NULL"));

            if (voltCandidate != null &&
                matchedGems.Contains(voltCandidate))
            {
                matchedGems.Remove(voltCandidate);

                SpecialGemSystem.Instance.CreateVoltGem(voltCandidate);
            }

            AddScore(matchedGems.Count * 10);

            GridManager.Instance.DestroyMatchedGems(matchedGems);

            yield return null;

            GravityManager.Instance.ApplyGravity();

            yield return null;

            SpawnManager.Instance.SpawnNewGems();

            yield return null;

            GravityManager.Instance.ApplyGravity();

            yield return null;

            matchedGems = MatchDetector.Instance.FindMatches();
        }

        Debug.Log("Board Stable");

        isProcessing = false;
    }
}