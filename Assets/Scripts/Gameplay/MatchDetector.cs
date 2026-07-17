using System.Collections.Generic;
using UnityEngine;

public class MatchDetector : MonoBehaviour
{
    public static MatchDetector Instance;

    // Stores one gem that should become a Volt Gem
    public Gem voltGemCandidate;

    private void Awake()
    {
        Instance = this;
    }

    public List<Gem> FindMatches()
    {
        List<Gem> matchedGems = new List<Gem>();

        voltGemCandidate = null;

        GridManager grid = GridManager.Instance;

        // ---------- Horizontal Matches ----------
        for (int y = 0; y < grid.height; y++)
        {
            int matchLength = 1;

            for (int x = 1; x < grid.width; x++)
            {
                Gem current = grid.board[x, y];
                Gem previous = grid.board[x - 1, y];

                if (current != null &&
                    previous != null &&
                    current.gemType == previous.gemType)
                {
                    matchLength++;
                }
                else
                {
                    if (matchLength >= 3)
                    {
                        // Save one gem if this is a 4+ match
                        if (matchLength >= 4 && voltGemCandidate == null)
                        {
                            voltGemCandidate = grid.board[x - 1, y];
                        }

                        for (int i = 0; i < matchLength; i++)
                        {
                            Gem gem = grid.board[x - 1 - i, y];

                            if (gem != null && !matchedGems.Contains(gem))
                                matchedGems.Add(gem);
                        }
                    }

                    matchLength = 1;
                }
            }

            if (matchLength >= 3)
            {
                if (matchLength >= 4 && voltGemCandidate == null)
                {
                    voltGemCandidate = grid.board[grid.width - 1, y];
                }

                for (int i = 0; i < matchLength; i++)
                {
                    Gem gem = grid.board[grid.width - 1 - i, y];

                    if (gem != null && !matchedGems.Contains(gem))
                        matchedGems.Add(gem);
                }
            }
        }

        // ---------- Vertical Matches ----------
        for (int x = 0; x < grid.width; x++)
        {
            int matchLength = 1;

            for (int y = 1; y < grid.height; y++)
            {
                Gem current = grid.board[x, y];
                Gem previous = grid.board[x, y - 1];

                if (current != null &&
                    previous != null &&
                    current.gemType == previous.gemType)
                {
                    matchLength++;
                }
                else
                {
                    if (matchLength >= 3)
                    {
                        if (matchLength >= 4 && voltGemCandidate == null)
                        {
                            voltGemCandidate = grid.board[x, y - 1];
                        }

                        for (int i = 0; i < matchLength; i++)
                        {
                            Gem gem = grid.board[x, y - 1 - i];

                            if (gem != null && !matchedGems.Contains(gem))
                                matchedGems.Add(gem);
                        }
                    }

                    matchLength = 1;
                }
            }

            if (matchLength >= 3)
            {
                if (matchLength >= 4 && voltGemCandidate == null)
                {
                    voltGemCandidate = grid.board[x, grid.height - 1];
                }

                for (int i = 0; i < matchLength; i++)
                {
                    Gem gem = grid.board[x, grid.height - 1 - i];

                    if (gem != null && !matchedGems.Contains(gem))
                        matchedGems.Add(gem);
                }
            }
        }

        Debug.Log("Matches Found : " + matchedGems.Count);

        return matchedGems;
    }
}