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
    // node �ֺ��� �̿� ������ Ž��
    public List<Node> NeighborsNode(Node node)
    {
        List<Node> result = new List<Node>();
        // ��� �������� from(������)�� node�� ��� ����Ǿ� �ִ� to(���� ���)�� �߰�.
        foreach(Edge e in edges)
        {
            if(e.from == node)
            {
                result.Add(e.to);
            }
        }

        return result;
    }
    // �׸��忡 �ִ� Ÿ���� ���� ����. (��ġ ����)
    public void AddNode(Vector3 pos)
    {
        nodes.Add(new Node(nodes.Count, pos));
    }
    // from ��忡�� to ������ ������ Edge����Ʈ�� �߰�.
    public void AddEdge(Node from, Node to)
    {   // from ��忡�� to ���� ������ ����ġ�� 1�� �����Ͽ� �߰�.
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
    {   // ���� ���
        List<Node> path = new List<Node>();

        if(start == end)    // ������� �������� ���ٸ�. ���ڸ�
        {
            path.Add(start);
            return path;
        }

        List<Node> unvisited = new List<Node>();    // �湮���� ���� ����.

        Dictionary<Node, Node> previous = new Dictionary<Node, Node>(); // �湮�ߴ� ����

        Dictionary<Node, float> distances = new Dictionary<Node, float>();  // �� ������ �Ÿ�

        for(int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            unvisited.Add(node);

            distances.Add(node, float.MaxValue);    // ������� �������� �� ������ �Ÿ��� ���Ѵ�� ����. ���ͽ�Ʈ��
        }

        distances[start] = 0f;  // ���� ���� Ž���� ������� �Ÿ��� 0���� ����
        while (unvisited.Count != 0)
        {   // �Ÿ� �����ؼ� ���� ù��° (ª��)�Ÿ��� �ִ� ��带 ������ Ž��. (������� ���� ������!)
            unvisited = unvisited.OrderBy(node => distances[node]).ToList();
            Node current = unvisited[0];
            unvisited.Remove(current);  // ������κ��� ���� ��带 ����

            if (current == end) // �����ߴٸ�?
            {   // �湮�ߴ� ������ ���Խ�Ų��.
                while (previous.ContainsKey(current))
                {   // ��ο� ���� ��带 �� �տ� �߰�. -> ���� �з������� �� ó�� start�� ù��°�� ��.
                    path.Insert(0, current);
                    current = previous[current]; // current�� �̾����ִ� �θ� ��带 current�� ����
                }
                // ����� �ֱ�!
                path.Insert(0, current);
                break;
            }
            // ���� ��带 �������� ������ �ִ� �̿� ������ Ž��
            foreach (Node neighbor in NeighborsNode(current))
            {   // ���� ���� �̿� ����� ���̸� ���ϰ�
                float length = Vector3.Distance(current.worldPosition, neighbor.worldPosition);
                // ���̿� ���� ����� �Ÿ� ��(������� ����)�� ���Ѵ�.
                float alt = distances[current] + length;
                // ���� ���� ���� �ּҶ��
                if(alt < distances[neighbor])
                {   // �̿� ����� �θ� ��带 ���� ���� �����ϰ� �Ÿ��� ������Ʈ.
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
    public int index;   // ��� �ε��� ( 0 ~ N )
    public Vector3 worldPosition; // ��� ��ġ

    private EntityBase entityOccupied;  // �����?

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
    public Node from; // ���� ���
    public Node to; // ���� ���

    private float weight;   // ����ġ (1, ���ш�)

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