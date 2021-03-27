using System;
using UnityEngine;

public class Patrol : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayLengthDown;
    [SerializeField] private float rayLengthRight;
    [SerializeField] private Transform groundDetection;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float slopeForceDown;
    [SerializeField] private float slopeForceUp;
    [SerializeField] private float slopeForceRayLength;
    [SerializeField] private Vector2 detectionAreaSize;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask playerLayerMask;
    
    private BoxCollider2D boxCollider;
    private Vector2 enemyDirection;
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Vector3 smoothDampVelocity = Vector3.zero;
    private Vector2 velocity;
    private Animator animator;
    private static readonly int MoveSpeed = Animator.StringToHash("Move_Speed");
    private const float GroundedRadius = .5f;

    public event EventHandler OnAttack;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        // If it's not grounded than it's falling
        if (!IsGrounded()) {
            Destroy(gameObject, 1f);
        }
        else {
            animator.SetFloat(MoveSpeed, Mathf.Abs(rb.velocity.x));
            
            Collider2D playerCollider = Physics2D.OverlapBox(transform.position, detectionAreaSize, 0f, playerLayerMask);
            if (playerCollider != null) {
                if (playerCollider.transform.position.x > transform.position.x && !movingRight ||
                    playerCollider.transform.position.x < transform.position.x && movingRight) {
                    Flip();
                }
                OnAttack?.Invoke(this, EventArgs.Empty);
            }
            else {
                MoveEnemy();
            }
        }
    }

    private bool IsGrounded() {
        var position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down,
            boxCollider.bounds.extents.y * slopeForceRayLength, groundLayerMask);
        
        if (hit) {
            return true;
        }
        
        return false;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionAreaSize);
    }

    private void MoveEnemy() {
        enemyDirection = movingRight ? Vector2.right : Vector2.left;
        //animator.Play("Enemy_Walk");

        if (IsOnSlope()) {
            Vector2 rayBottomEdgeOrigin = transform.position + 
                                          new Vector3(boxCollider.bounds.extents.x / 2 * enemyDirection.x, 
                                              -boxCollider.bounds.extents.y);
            RaycastHit2D hit = Physics2D.Raycast(rayBottomEdgeOrigin, enemyDirection,
                boxCollider.bounds.extents.x * slopeForceRayLength, groundLayerMask);
            Debug.DrawRay(rayBottomEdgeOrigin,
                enemyDirection * (boxCollider.bounds.extents.x * slopeForceRayLength), Color.green);
            
            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                ClimbSlope(ref velocity, slopeAngle);
            }
            else {
                velocity.y = -(boxCollider.bounds.extents.y * slopeForceDown);
            }
        }
        
        var targetVelocity = new Vector2(moveSpeed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(velocity, targetVelocity * enemyDirection, ref smoothDampVelocity,
            movementSmoothing);

        if (ShouldEnemyBeFlipped()) {
            Flip();
        }
    }

    private bool ShouldEnemyBeFlipped() {
        Vector2 rayOrigin = groundDetection.position;
        RaycastHit2D groundInfoHit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLengthDown);
        RaycastHit2D obstacleInfoHit = Physics2D.Raycast(rayOrigin, enemyDirection, rayLengthRight);
        Debug.DrawRay(rayOrigin, Vector2.down * rayLengthDown, Color.red);
        Debug.DrawRay(rayOrigin, enemyDirection * rayLengthRight, Color.red);

        return !groundInfoHit.collider || obstacleInfoHit;
    }

    private void ClimbSlope(ref Vector2 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs(rb.velocity.x) / 2;
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * slopeForceUp;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(rb.velocity.x);
    }

    private void Flip() {
        velocity = Vector2.zero;
        transform.Rotate(0f, 180f, 0f);
        movingRight = !movingRight;
    }

    private bool IsOnSlope() {
        var position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down,
            boxCollider.bounds.extents.y * slopeForceRayLength, groundLayerMask);
        Debug.DrawRay(position, Vector2.down * (boxCollider.bounds.extents.y * slopeForceRayLength),
            Color.green);
        //print(hit.point + " normal:" + hit.normal.ToString("F8") + " v2: " + Vector2.up);

        if (hit) {
            // of if (Vector3.Dot(hit.normal, Vector2.up) < 0.99f)
            if (Vector3.Angle(hit.normal, Vector2.up) >= 0.1f) {
                return true;
            }
        }

        return false;
    }
}