using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private int patternNum;
    [SerializeField] private Collider2D col;
    void OnEnable()
    {
        anim.SetInteger("Magic", patternNum);
    }

    public void PatternSetting(int patternIndex, int y, int x)
    {
        transform.localPosition = GetWorldPositionCenter(y, x);
        patternNum = patternIndex;
    }

    public Vector3 GetWorldPositionCenter(int y, int x)
    {
        return new Vector3(x - 0.5f, y - 0.5f);
    }
    /// <summary>
    /// ���� ���� -> ���� ������Ʈ ��Ȱ��ȭ
    /// �ִϸ����� �̺�Ʈ�� ���
    /// </summary>
    void OffPattern()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ���� ���� -> �ݶ��̴� Ȱ��ȭ -> �÷��̾� trigger ����
    /// �ִϸ����� �̺�Ʈ�� ���
    /// </summary>
    void AttackStart()
    {
        col.enabled = true;
    }
    /// <summary>
    /// ���� ���� -> �ݶ��̴� ��Ȱ��ȭ
    /// </summary>
    void AttackFinish()
    {
        col.enabled = false;
    }
}
