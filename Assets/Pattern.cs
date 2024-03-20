using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private int patternNum;
    void OnEnable()
    {
        anim.SetInteger("Magic", patternNum);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OffPattern()
    {
        gameObject.SetActive(false);
    }
}
