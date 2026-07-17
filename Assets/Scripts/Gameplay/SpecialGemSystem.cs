using System.Collections.Generic;
using UnityEngine;

public class SpecialGemSystem : MonoBehaviour
{
    public static SpecialGemSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Creates a Volt Gem
    public void CreateVoltGem(Gem gem)
    {
        if (gem == null)
            return;

        gem.MakeVoltGem();

        Debug.Log($"⚡ Volt Gem Created at ({gem.x}, {gem.y})");
    }

    // Activates a Volt Gem by clearing all gems
    // of the selected color.
    public List<Gem> ActivateVoltGem(int targetGemType)
    {
        List<Gem> gemsToDestroy = new List<Gem>();

        GridManager grid = GridManager.Instance;

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                Gem gem = grid.board[x, y];

                if (gem == null)
                    continue;

                if (gem.gemType == targetGemType)
                {
                    if (!gemsToDestroy.Contains(gem))
                        gemsToDestroy.Add(gem);
                }
            }
        }

        Debug.Log($"⚡ Volt Gem activated! Destroying {gemsToDestroy.Count} gems.");

        return gemsToDestroy;
    }
}