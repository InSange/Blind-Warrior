using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Team1,
    Team2
}

public class GameManager : Manager<GameManager>
{
    [SerializeField] private Transform team1Parent; // 우리 진영
    [SerializeField] private Transform team2Parent; // 상대 진영

    public Transform Team1Parent
    {
        get
        {
            return team1Parent;
        }
    }

    public Transform Team2Parent
    {
        get
        {
            return team2Parent;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
