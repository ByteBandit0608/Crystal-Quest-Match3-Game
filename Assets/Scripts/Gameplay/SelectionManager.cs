using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    private Gem selectedGem;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectGem(Gem gem)
    {
        // Stop all input after Game Over
        if (GameManager.Instance.movesRemaining <= 0)
        {
            Debug.Log("Game Over - Input Disabled");
            return;
        }

        // First Selection
        if (selectedGem == null)
        {
            selectedGem = gem;
            Debug.Log("Selected Gem : (" + gem.x + "," + gem.y + ")");
            return;
        }

        // Same gem clicked again
        if (selectedGem == gem)
        {
            Debug.Log("Deselected");

            selectedGem = null;
            return;
        }

        // Adjacent?
        if (IsAdjacent(selectedGem, gem))
        {
            Debug.Log("Trying Swap...");

            bool validMove = GridManager.Instance.SwapGems(selectedGem, gem);

            if (validMove)
            {
                Debug.Log("Valid Move");
            }
            else
            {
                Debug.Log("Invalid Move - Swapped Back");
            }
        }
        else
        {
            Debug.Log("Not Adjacent");
        }

        selectedGem = null;
    }

    bool IsAdjacent(Gem a, Gem b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return dx + dy == 1;
    }
}