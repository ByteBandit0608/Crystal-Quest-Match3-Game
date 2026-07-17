using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnNewGems()
    {
        GridManager grid = GridManager.Instance;

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (grid.board[x, y] == null)
                {
                    grid.CreateGem(x, y);
                }
            }
        }

        Debug.Log("Board Refilled");
    }
}