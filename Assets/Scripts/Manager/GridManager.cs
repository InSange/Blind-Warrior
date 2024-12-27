using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngineInternal;

public class GridManager : Manager<GridManager>
{
    // Grid Info
    [SerializeField] private float cellSize;
    [SerializeField] private int centerX;
    [SerializeField] private int centerY;
    [SerializeField] private Pattern[,] gridArray;
    [SerializeField] private int screenWidth;
    [SerializeField] private int screenHeight;

    // Graph Info
    [SerializeField] private Graph graph;
    [SerializeField] private Transform graphParent;

    //Tile Info
    [SerializeField] private GameObject baseTile;
    [SerializeField] private List<GameObject> tiles;

    private void Start()
    {
        Grid(9, 7, 1f);
        InitGraph();
    }

    public Node GetNodeForTile(Tile t)
    {
        var allNodes = graph.Nodes;

        foreach (var node in allNodes)
        {
            if(t.transform.GetSiblingIndex() == node.index)
            {
                return node;
            }
        }

        return null;
    }

    public void Grid(int screenWidth, int screenHeight, float cellSize)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.cellSize = cellSize;

        gridArray = new Pattern[screenWidth, screenHeight];

        int centerX = screenWidth >> 1 ;
        int centerY = screenHeight>>1;
        Debug.Log(centerX + " , " + centerY);

        for (int x = 0; x < screenWidth; x++)
        {
            for (int y = 0; y < screenHeight; y++)
            {
                GameObject tile = Instantiate(baseTile, graphParent);
                tile.transform.localPosition = new Vector3(x-centerX, y-centerY);
                tile.AddComponent<Tile>();
                tile.GetComponent<Tile>().Init();
                tile.AddComponent<BoxCollider2D>();
                tile.name = tiles.Count.ToString(); 
                tiles.Add(tile);
            }
        }
    }

    private void InitGraph()
    {
        graph = new Graph();

        for(int i = 0; i < tiles.Count; i++)
        {
            Vector3 pos = tiles[i].transform.position;
            graph.AddNode(pos);
        }

        for(int i = 0; i < graph.Nodes.Count; i++)
        {
            Tile tile = tiles[i].GetComponent<Tile>();
            tile.NodeInfo = graph.Nodes[i];
        }

        var allNodes = graph.Nodes;

        foreach(Node from in allNodes)
        {
            foreach(Node to in allNodes)
            {
                if(Vector3.Distance(from.worldPosition, to.worldPosition) < 2f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }

        Debug.Log(graph.Nodes.Count + ", " + graph.Edges.Count);
    }

    private Vector3 GetWorldPosition(int y, int x)
    {
        return new Vector3(x, y) * cellSize;
    }

    private void OnDrawGizmos()
    {
        int fromIndex = 0;
        int toIndex = 0;

        if (graph == null)
            return;

        var allEdges = graph.Edges;
        if (allEdges == null)
            return;

        foreach (Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 100);
        }

        var allNodes = graph.Nodes;
        if (allNodes == null)
            return;

        foreach (Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);
        }

        if (fromIndex >= allNodes.Count || toIndex >= allNodes.Count)
            return;

        List<Node> path = graph.GetShortestPath(allNodes[fromIndex], allNodes[toIndex]);
        if (path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].worldPosition, path[i].worldPosition, Color.red, 10);
            }
        }
    }
}
