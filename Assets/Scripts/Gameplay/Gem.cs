using UnityEngine;

public class Gem : MonoBehaviour
{
    public int x;
    public int y;

    public int gemType;

    public bool isVoltGem = false;

    private SpriteRenderer spriteRenderer;

    private Color normalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        normalColor = color;

        // Don't overwrite the Volt Gem appearance
        if (!isVoltGem)
            spriteRenderer.color = color;
    }

    public void MakeVoltGem()
    {
        isVoltGem = true;

        // Bigger
        transform.localScale = Vector3.one * 1.1f;

        // Bright white so it's obvious
        spriteRenderer.color = Color.white;

        Debug.Log($"⚡ Volt Gem Created at ({x}, {y})");
    }

    public void RemoveVoltGem()
    {
        isVoltGem = false;

        transform.localScale = Vector3.one * 0.75f;

        spriteRenderer.color = normalColor;
    }

    private void OnMouseDown()
    {
        if (SelectionManager.Instance != null)
        {
            SelectionManager.Instance.SelectGem(this);
        }
    }
}