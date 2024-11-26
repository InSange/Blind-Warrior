using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PattenType
{
    Magic = 0,
    End
}

public class PatternManager : MonoBehaviour
{
    [SerializeField] private Pattern[] patterns;
    [SerializeField] private List<Pattern>[] pools;
    [SerializeField] private int maxCnt;

    public void Setting()
    {
        pools = new List<Pattern>[patterns.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Pattern>();
        }
    }

    public Pattern Get(PattenType type, int patternIndex, int y, int x)
    {
        Pattern select = null;
        int typeIndex = ((int)type);

        foreach(Pattern item in pools[typeIndex])
        {
            if(item.gameObject.activeSelf)
            {
                select = item;
                select.PatternSetting(patternIndex, y, x);
                select.gameObject.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            select = Instantiate(patterns[typeIndex], transform);
            pools[typeIndex].Add(select);
        }

        return select;
    }

    public void Clear(int index)
    {
        foreach (Pattern item in pools[index])
            item.gameObject.SetActive(false);
    }

    public void ClearAll()
    {
        for (int index = 0; index < pools.Length; index++)
            foreach (Pattern item in pools[index])
                item.gameObject.SetActive(false);
    }
}
