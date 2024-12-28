using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer spriteRD;
    [SerializeField] private Animator characterAnim;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private WeaponParent weapon;
    // Values
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private bool isRun = false;
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private Vector3 lookDirection;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRD = GetComponentInChildren<SpriteRenderer>();
        //characterAnim = GetComponentInChildren<Animator>();
        playerState = GetComponent<PlayerState>();
        weapon = GetComponentInChildren<WeaponParent>();

        playerState.StatusSetting();
    }

    // Update is called once per frame
    void Update()
    {
        RotateToPointer();
        if (Input.GetKeyDown(KeyCode.Mouse0) && !weapon.IsAttacking) weapon.Attack();
        Move();
        weapon.WeaponUpdate(lookDirection);
        playerState.UpdateStamina();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + movementDirection * moveSpeed * Time.deltaTime);
    }

    private void Move()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");
        movementDirection.Normalize();

        moveSpeed = speed;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && playerState.GetStatmina() > 0)
        {
            moveSpeed *= 2;
            isRun = true;
            if (movementDirection != Vector2.zero) playerState.DecreaseStamina();
        }
        else
        {
            isRun = false;
        }
        characterAnim.SetBool("run", isRun);
        characterAnim.SetFloat("speed", ((movementDirection.x != 0 || movementDirection.y != 0) ? moveSpeed : 0));
    }

    private void RotateToPointer()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection.z = Camera.main.nearClipPlane;
        Vector3 characterDirection = lookDirection - transform.position;

        if (characterDirection.x > 0)
        {
            spriteRD.flipX = false;
        }
        else if (characterDirection.x < 0)
        {
            spriteRD.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Pattern"))
        {
            Debug.Log("Hit Player");
        }
    }
}
