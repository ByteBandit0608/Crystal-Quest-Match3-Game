using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Board Size")]
    public int width = 9;
    public int height = 9;

    [Header("Prefab")]
    public GameObject gemPrefab;
    public Color[] gemColors;

    public Gem[,] board;

    public float spacing = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        board = new Gem[width, height];
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateGem(x, y);
            }
        }
    }

    public void CreateGem(int x, int y)
    {
        Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, 0);

        GameObject gemObject =
            Instantiate(gemPrefab, spawnPosition, Quaternion.identity);

        gemObject.transform.localScale = Vector3.one * 0.75f;

        Gem gem = gemObject.GetComponent<Gem>();

        int randomType;

        do
        {
            randomType = Random.Range(0, gemColors.Length);
        }
        while (CreatesMatch(x, y, randomType));

        gem.gemType = randomType;
        gem.SetColor(gemColors[randomType]);

        gem.x = x;
        gem.y = y;

        board[x, y] = gem;
    }

    bool CreatesMatch(int x, int y, int type)
    {
        if (x >= 2)
        {
            Gem left1 = board[x - 1, y];
            Gem left2 = board[x - 2, y];

            if (left1 != null &&
                left2 != null &&
                left1.gemType == type &&
                left2.gemType == type)
                return true;
        }

        if (y >= 2)
        {
            Gem down1 = board[x, y - 1];
            Gem down2 = board[x, y - 2];

            if (down1 != null &&
                down2 != null &&
                down1.gemType == type &&
                down2.gemType == type)
                return true;
        }

        return false;
    }

    public bool SwapGems(Gem gem1, Gem gem2)
    {
        int x1 = gem1.x;
        int y1 = gem1.y;

        int x2 = gem2.x;
        int y2 = gem2.y;

        board[x1, y1] = gem2;
        board[x2, y2] = gem1;

        gem1.x = x2;
        gem1.y = y2;

        gem2.x = x1;
        gem2.y = y1;

        gem1.transform.position = new Vector3(x2 * spacing, y2 * spacing, 0);
        gem2.transform.position = new Vector3(x1 * spacing, y1 * spacing, 0);

// --------------------
// VOLT GEM ACTIVATION
// --------------------

if (gem1.isVoltGem || gem2.isVoltGem)
{
    Gem voltGem = gem1.isVoltGem ? gem1 : gem2;
    Gem normalGem = gem1.isVoltGem ? gem2 : gem1;

    List<Gem> gemsToDestroy =
        SpecialGemSystem.Instance.ActivateVoltGem(normalGem.gemType);

    // Also destroy the Volt Gem itself
    gemsToDestroy.Add(voltGem);

    GameManager.Instance.UseMove();

    // Start processing the destruction
    GameManager.Instance.ProcessMatches(gemsToDestroy);

    return true;
}

// --------------------
// NORMAL MATCH
// --------------------

List<Gem> matchedGems = MatchDetector.Instance.FindMatches();

if (matchedGems.Count == 0)
{
    board[x1, y1] = gem1;
    board[x2, y2] = gem2;

    gem1.x = x1;
    gem1.y = y1;

    gem2.x = x2;
    gem2.y = y2;

    gem1.transform.position = new Vector3(x1 * spacing, y1 * spacing, 0);
    gem2.transform.position = new Vector3(x2 * spacing, y2 * spacing, 0);

    return false;
}

GameManager.Instance.UseMove();
GameManager.Instance.ProcessMatches(matchedGems);

return true;
    }

    public void ProcessMatches(List<Gem> matchedGems)
    {
        Gem voltCandidate = MatchDetector.Instance.voltGemCandidate;

        if (voltCandidate != null)
        {
            matchedGems.Remove(voltCandidate);

            SpecialGemSystem.Instance.CreateVoltGem(voltCandidate);
        }

        DestroyMatchedGems(matchedGems);

        GravityManager.Instance.ApplyGravity();

        SpawnManager.Instance.SpawnNewGems();

        GravityManager.Instance.ApplyGravity();

        Debug.Log("Board Updated");
    }

    public void DestroyMatchedGems(List<Gem> matchedGems)
    {
        foreach (Gem gem in matchedGems)
        {
            if (gem == null)
                continue;

            board[gem.x, gem.y] = null;

            Destroy(gem.gameObject);
        }

        Debug.Log("Destroyed " + matchedGems.Count + " gems.");
    }
}