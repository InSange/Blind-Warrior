using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Node nodeInfo;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Node NodeInfo
    {
        get => nodeInfo;
        set => nodeInfo = value;
    }

    public void Init()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Tile");
        spriteRenderer = GetComponent<SpriteRenderer>();
        validColor = new Color(0, 0, 1, 0.3f);
        invalidColor = new Color(1, 0, 0, 0.3f);
        normalColor = new Color(1, 1, 1, 0.3f);
    }

    public void SetColor(bool isValid)
    { 
        spriteRenderer.color = isValid ? validColor : invalidColor;
    }

    public void ClearColor()
    {
        spriteRenderer.color = normalColor;
    }
}
