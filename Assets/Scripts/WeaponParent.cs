using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer, weaponRenderer, handRenderer;
    [SerializeField] private Animator weaponAnim;

    public Transform circleOrigin;
    public float radius;

    public bool IsAttacking;
    /// <summary>
    /// ���� ������Ʈ
    /// </summary>
    /// <param name="pointer"> ���콺�����Ͱ� �ٶ󺸴� �������� ���⸦ �Ű��ش�. </param>
    public void WeaponUpdate(Vector2 pointer)
    {
        if (IsAttacking)
        {
            return;
        }
        Vector2 direction = (pointer - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 2;
            handRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
            handRenderer.sortingOrder = characterRenderer.sortingOrder + 2;

        }
    }
    /// <summary>
    /// ����! �ִϸ��̼� Ʈ���� �ߵ�
    /// </summary>
    public void Attack()
    {
        IsAttacking = true;
        weaponAnim.SetTrigger("attack");
    }
    /// <summary>
    /// WeaponParent ������Ʈ�� Animator�� Event�� ���
    /// </summary>
    public void ResetIsAttack()
    {
        IsAttacking = false;
    }
    /// <summary>
    /// ���� ���� CHECK
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    /// <summary>
    /// WeaponParent ������Ʈ�� Animator�� Event�� ���
    /// </summary>
    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Debug.Log(collider.name);
            if (gameObject.layer == 7 && collider.gameObject.layer == 7) continue;
            else if ((gameObject.layer == 6 || gameObject.layer == 9) && collider.gameObject.layer != 7) continue;
        }
    }
}
