using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyGravity()
    {
        GridManager grid = GridManager.Instance;

        bool moved;

        do
        {
            moved = false;

            for (int x = 0; x < grid.width; x++)
            {
                for (int y = 0; y < grid.height - 1; y++)
                {
                    if (grid.board[x, y] == null)
                    {
                        for (int above = y + 1; above < grid.height; above++)
                        {
                            if (grid.board[x, above] != null)
                            {
                                MoveGem(x, above, y);

                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

        } while (moved);

        Debug.Log("Gravity Complete");
    }

    void MoveGem(int x, int fromY, int toY)
    {
        GridManager grid = GridManager.Instance;

        Gem gem = grid.board[x, fromY];

        grid.board[x, toY] = gem;
        grid.board[x, fromY] = null;

        gem.y = toY;

        gem.transform.position =
            new Vector3(
                x * grid.spacing,
                toY * grid.spacing,
                0);
    }
}