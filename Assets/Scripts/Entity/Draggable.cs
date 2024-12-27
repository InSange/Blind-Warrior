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
        Debug.Log("드래그 스타트");
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

        Tile tile = CheckTile();    // 내 마우스 포인터 위치에 있는 타일 가져오기.

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
                {   // 만약 누군가 있다면? 해당 노드에 있는 엔티티의 정보를 가져와 스왑

                }
                else// if(!releaseNode.IsOccupied) 비워져있다면?
                {
                    if(curEntity.EntityNode != null) curEntity.EntityNode.IsOccupied = false; // 현재 노드 비우기
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
