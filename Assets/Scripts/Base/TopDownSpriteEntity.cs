using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class TopDownSpriteEntity : MonoBehaviour
{

    public const float MOVEMENT_MULTIPLIER = .01f;
    SpriteRenderer spriteRender;
    BoxCollider2D boxCollider;

    public float speed = 2.0f;
    public Vector3 MoveDir = Vector3.zero;
    private Vector3 TweenMoveDir = Vector3.zero;

    public Sprite[] sprites = new Sprite[4];
    public SpriteDirection sDirection = SpriteDirection.DOWN;

    public enum SpriteDirection
    {
        DOWN = 0,
        RIGHT = 1,
        UP = 2,
        LEFT = 3
    }

    // Use this for initialization
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void HandleUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
            return;
        MoveDir = Vector3.zero;
        HandleUpdate();
        if (MoveDir.x > 0)
            sDirection = SpriteDirection.RIGHT;
        else if (MoveDir.x < 0)
            sDirection = SpriteDirection.LEFT;
        else if (MoveDir.y > 0)
            sDirection = SpriteDirection.UP;
        else if (MoveDir.y < 0)
            sDirection = SpriteDirection.DOWN;

        spriteRender.sprite = sprites[(int)sDirection];

    }

    protected virtual void HandleFixedUpdate()
    {

    }

    private const float TweenValue = .2f;
    public LayerMask StopMovementMask;

    void FixedUpdate()
    {
        HandleFixedUpdate();
        TweenMoveDir = EaseFunctions.Ease(EaseFunctions.Type.Linear, TweenValue, TweenMoveDir, MoveDir - TweenMoveDir, 1.0f);
        RaycastHit2D[] results = new RaycastHit2D[10];
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y);

        RaycastHit2D result = Physics2D.BoxCast(new Vector2(newPos.x + boxCollider.size.x / 4f * (TweenMoveDir.x > 0 ? 1 : -1), newPos.y + boxCollider.offset.y), new Vector2(boxCollider.size.x / 2f, boxCollider.size.y * .5f), 0, Vector2.right, TweenMoveDir.x * speed * MOVEMENT_MULTIPLIER, StopMovementMask);

        if (result.collider != null)
        {
            if (TweenMoveDir.x > 0)
                newPos.x = result.collider.bounds.center.x - result.collider.bounds.extents.x - boxCollider.size.x / 2f;
            else if (TweenMoveDir.x < 0)
                newPos.x = result.collider.bounds.center.x + result.collider.bounds.extents.x + boxCollider.size.x / 2f;
        }
        else
        {
            newPos.x += TweenMoveDir.x * speed * MOVEMENT_MULTIPLIER;

        }

        result = Physics2D.BoxCast(new Vector2(newPos.x, newPos.y + boxCollider.offset.y + boxCollider.size.y / 4f * (TweenMoveDir.y > 0 ? 1 : -1)), new Vector2(boxCollider.size.x * .5f, boxCollider.size.y / 2f), 0, Vector2.up, TweenMoveDir.y * speed * MOVEMENT_MULTIPLIER, StopMovementMask);
        if (result.collider != null)
        {
            if (TweenMoveDir.y > 0)
                newPos.y = result.collider.bounds.center.y - result.collider.bounds.extents.y - boxCollider.offset.y - boxCollider.size.y / 2f;
            else if (TweenMoveDir.y < 0)
                newPos.y = result.collider.bounds.center.y + result.collider.bounds.extents.y - boxCollider.offset.y + boxCollider.size.y / 2f;

        }
        else
            newPos.y += TweenMoveDir.y * speed * MOVEMENT_MULTIPLIER;
        transform.position = newPos;

        //        Debug.Log(boxCollider.Cast(TweenMoveDir, results));// (f, results));







    }
}
