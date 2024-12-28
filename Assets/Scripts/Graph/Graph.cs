using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.iOS;

public class Graph
{
    private List<Node> nodes;
    private List<Edge> edges;

    public List<Node> Nodes => nodes;
    public List<Edge> Edges => edges;

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>(); 
    }
    // node 주변의 이웃 노드들을 탐색
    public List<Node> NeighborsNode(Node node)
    {
        List<Node> result = new List<Node>();
        // 모든 간선에서 from(시작점)이 node일 경우 연결되어 있는 to(연결 노드)를 추가.
        foreach(Edge e in edges)
        {
            if(e.from == node)
            {
                result.Add(e.to);
            }
        }

        return result;
    }
    // 그리드에 있는 타일을 노드로 저장. (위치 포함)
    public void AddNode(Vector3 pos)
    {
        nodes.Add(new Node(nodes.Count, pos));
    }
    // from 노드에서 to 노드로의 간선을 Edge리스트에 추가.
    public void AddEdge(Node from, Node to)
    {   // from 노드에서 to 노드로 간선의 가중치를 1로 설정하여 추가.
        edges.Add(new Edge(from, to, 1));
    }

    public float Distance(Node from, Node to)
    {
        foreach(Edge e in edges)
        {
            if(e.from == from && e.to == to)
            {
                return e.GetWeight();
            }
        }

        return Mathf.Infinity;
    }

    public virtual List<Node> GetShortestPath(Node start, Node end)
    {   // 최종 경로
        List<Node> path = new List<Node>();

        if(start == end)    // 출발지와 도착지가 같다면. 제자리
        {
            path.Add(start);
            return path;
        }

        List<Node> unvisited = new List<Node>();    // 방문하지 않은 노드들.

        Dictionary<Node, Node> previous = new Dictionary<Node, Node>(); // 방문했던 노드들

        Dictionary<Node, float> distances = new Dictionary<Node, float>();  // 각 노드들의 거리

        for(int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            unvisited.Add(node);

            distances.Add(node, float.MaxValue);    // 출발지를 기점으로 각 노드들의 거리를 무한대로 설정. 다익스트라
        }

        distances[start] = 0f;  // 가장 먼저 탐색할 출발지의 거리를 0으로 설정
        while (unvisited.Count != 0)
        {   // 거리 정렬해서 가장 첫번째 (짧은)거리에 있는 노드를 꺼내서 탐색. (출발지가 제일 먼저임!)
            unvisited = unvisited.OrderBy(node => distances[node]).ToList();
            Node current = unvisited[0];
            unvisited.Remove(current);  // 출발지로부터 현재 노드를 제거

            if (current == end) // 도착했다면?
            {   // 방문했던 노드들을 포함시킨다.
                while (previous.ContainsKey(current))
                {   // 경로에 현재 노드를 맨 앞에 추가. -> 점점 밀려나가서 맨 처음 start가 첫번째로 옴.
                    path.Insert(0, current);
                    current = previous[current]; // current로 이어져있는 부모 노드를 current로 변경
                }
                // 출발지 넣기!
                path.Insert(0, current);
                break;
            }
            // 현재 노드를 중점으로 인접해 있는 이웃 노드들을 탐색
            foreach (Node neighbor in NeighborsNode(current))
            {   // 현재 노드와 이웃 노드의 길이를 구하고
                float length = Vector3.Distance(current.worldPosition, neighbor.worldPosition);
                // 길이와 현재 노드의 거리 값(출발지로 부터)을 합한다.
                float alt = distances[current] + length;
                // 만약 구한 값이 최소라면
                if(alt < distances[neighbor])
                {   // 이웃 노드의 부모 노드를 현재 노드로 변경하고 거리를 업데이트.
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                }
            }
        }

        return path;
    }
}

public class Node
{
    public int index;   // 노드 인덱스 ( 0 ~ N )
    public Vector3 worldPosition; // 노드 위치

    private EntityBase entityOccupied;  // 사용중?

    public Node(int index, Vector3 worldPosition, EntityBase occupied = null)
    {
        this.index = index;
        this.worldPosition = worldPosition;
        this.entityOccupied = occupied;
    }

    public EntityBase EntityOccupied
    {
        get => entityOccupied;
        set => entityOccupied = value;
    }

    public bool IsOccupied => entityOccupied != null;
}

public class Edge
{
    public Node from; // 시작 노드
    public Node to; // 도착 노드

    private float weight;   // 가중치 (1, 무한댸)

    public Edge(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight()
    {
        if (to.IsOccupied)
            return Mathf.Infinity;

        return weight;
    }
}