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
    /// 패턴 종료 -> 게임 오브젝트 비활성화
    /// 애니메이터 이벤트로 사용
    /// </summary>
    void OffPattern()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 공격 시작 -> 콜라이더 활성화 -> 플레이어 trigger 감지
    /// 애니메이터 이벤트로 사용
    /// </summary>
    void AttackStart()
    {
        col.enabled = true;
    }
    /// <summary>
    /// 공격 종료 -> 콜라이더 비활성화
    /// </summary>
    void AttackFinish()
    {
        col.enabled = false;
    }
}
