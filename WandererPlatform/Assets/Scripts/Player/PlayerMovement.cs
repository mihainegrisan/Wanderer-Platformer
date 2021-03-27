using UnityEngine;


[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {
    private static readonly int PlayerSpeed = Animator.StringToHash("playerSpeed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsCrouching = Animator.StringToHash("IsCrouching");

    public float runSpeed = 40f;
    private Animator animator;

    private CharacterController2D controller;
    private bool isCrouching;
    private bool isJumping;
    private float horizontalMove;
    private bool isReloadingScene;
    private bool isHoldingJumpButton;

    private void Awake() {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }
    
    private void Update() {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat(PlayerSpeed, Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
            animator.SetBool(IsJumping, true);
            
        }

        if (Input.GetButtonDown("Crouch")) {
            isCrouching = true;
        }
        else if (Input.GetButtonUp("Crouch")) {
            isCrouching = false;
        }

        if (transform.position.y < -25 && !isReloadingScene) {
            isReloadingScene = true;
            GameManager.Instance.ReloadLevel();
        }
    }

    public void OnLanding() {
        isJumping = false;
        animator.SetBool(IsJumping, false);
    }
    public void OnCrouching(bool isCrouching) {
        animator.SetBool(IsCrouching, isCrouching);
    }

    private void FixedUpdate() {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, isCrouching, isJumping);
    }
}