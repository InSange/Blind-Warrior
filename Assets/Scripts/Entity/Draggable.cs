using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Draggable : MonoBehaviour
{
    [SerializeField] private LayerMask layerTileMask;
    [SerializeField] private Vector3 dragOffset;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Vector3 previousPos;
    [SerializeField] private int previousSortingOrder;
    [SerializeField] private Tile previousTile = null;
    [SerializeField] private bool IsDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDragStart()
    {
        Debug.Log("�巡�� ��ŸƮ");
        previousPos = this.transform.position;
        previousSortingOrder = spriteRenderer.sortingOrder;

        spriteRenderer.sortingOrder = previousSortingOrder + 20;
        IsDragging = true;

    }

    public void OnDragging()
    {
        if(!IsDragging)
        {
            return;
        }

        Vector3 newPos = playerCamera.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        newPos.z = 0;
        this.transform.position = newPos;

        Tile tile = CheckTile();    // �� ���콺 ������ ��ġ�� �ִ� Ÿ�� ��������.

        if(tile)
        {
            tile.SetColor(!tile.NodeInfo.IsOccupied);

            if(previousTile && tile != previousTile)
            {
                //previousTile.SetColor(false);
                previousTile.ClearColor();
            }

            previousTile = tile;
        }
    }

    public void OnDragEnd()
    {
        if (!IsDragging)
        {
            return;
        }

        if(!TryRelease())
        {
            this.transform.position = previousPos;
        }

        if(previousTile)
        {
            previousTile.ClearColor();
            previousTile = null;
        }

        spriteRenderer.sortingOrder = previousSortingOrder;

        IsDragging = false;
    }

    private bool TryRelease()
    {
        Tile tile = CheckTile();

        if (tile)
        {
            EntityBase curEntity = GetComponent<EntityBase>();
            Node releaseNode = GridManager.instance.GetNodeForTile(tile);

            if (releaseNode != null && curEntity != null)
            {
                if(releaseNode.IsOccupied)
                {   // ���� ������ �ִٸ�? �ش� ��忡 �ִ� ��ƼƼ�� ������ ������ ����

                }
                else// if(!releaseNode.IsOccupied) ������ִٸ�?
                {
                    if(curEntity.EntityNode != null) curEntity.EntityNode.IsOccupied = false; // ���� ��� ����
                    curEntity.EntityNode = releaseNode;
                    releaseNode.IsOccupied = true;
                    curEntity.transform.position = releaseNode.worldPosition;

                    return true;
                }
            }
        }

        return false;
    }

    public Tile CheckTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, layerTileMask);

        if(hit &&  hit.collider != null)
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            return tile;
        }

        return null;
    }
}
