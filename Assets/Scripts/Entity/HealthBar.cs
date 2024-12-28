using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float maxHealth;
    [SerializeField] private Transform target;

    public void SetUp(Transform target, float maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateBar(maxHealth);
        this.target = target;
    }

    public void UpdateBar(float newValue)
    {
        float newScale = newValue / maxHealth;
        Vector3 scale = bar.transform.localScale;
        scale.x = newScale;
        bar.transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            this.transform.position = target.position + offset;
    }
}
