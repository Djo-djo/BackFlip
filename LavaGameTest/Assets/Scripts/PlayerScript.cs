using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    [Tooltip("Jump place area")]
    private GameObject jumpPlace;
    [SerializeField]
    [Tooltip("Need for jump count and setting next level")]
    private GameManagerScript gameManager;
    [SerializeField]
    [Tooltip("Animation which is used for backflip")]
    private AnimationClip backflipAnimation;

    private bool isGrounded;
    private bool isJumpAvailable;
    private bool isJumping;
    private bool isJumpToMiddle;

    private Vector3 goal;
    private Vector3 jumpGoal;
    private Vector3 jumpStart;
    private Vector3 jumpMiddle;
    private Vector3 playerStartPosition;

    private Animator animatorComponent;
    private Rigidbody rigidbodyComponent;

    public float JumpHeight { get; set; }

    void Start()
    {
        playerStartPosition = transform.position;
        animatorComponent = GetComponent<Animator>();
        rigidbodyComponent = GetComponent<Rigidbody>();
        goal = jumpPlace.transform.position + new Vector3(0, 0, 100);
    }

    void Update()
    {
        if (isGrounded)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        }

        if (isJumpAvailable && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            jumpStart = transform.position;

            var position = Input.GetMouseButtonDown(0) ? Input.mousePosition : new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

            if (Physics.Raycast(Camera.main.ScreenPointToRay(position), out RaycastHit hit))
            {
                jumpGoal = new Vector3(0, transform.localScale.y / 2, hit.point.z);
            }

            if (hit.point.z < transform.position.z)
            {
                jumpMiddle = new Vector3(jumpGoal.x / 2, jumpGoal.y + JumpHeight, jumpStart.z - (jumpStart.z - jumpGoal.z) / 2);
                isJumping = isJumpToMiddle = true;
                rigidbodyComponent.isKinematic = true;
                rigidbodyComponent.useGravity = false;
                float animationSpeed = backflipAnimation.length / ((Vector3.Distance(jumpStart, jumpMiddle) + Vector3.Distance(jumpMiddle, jumpGoal)) / speed);
                animatorComponent.SetFloat("Speed", animationSpeed);
                animatorComponent.SetBool("IsJumping", isJumping);
            }
        }

        if (isJumping)
        {
            if (isJumpToMiddle)
            {
                transform.position = Vector3.MoveTowards(transform.position, jumpMiddle, speed * Time.deltaTime);
                if (transform.position == jumpMiddle)
                {
                    isJumpToMiddle = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, jumpGoal, speed * Time.deltaTime);
                if (transform.position == jumpGoal)
                {
                    rigidbodyComponent.isKinematic = false;
                    rigidbodyComponent.useGravity = true;
                    isJumping = false;
                    gameManager.JumpCount++;
                    animatorComponent.SetBool("IsJumping", isJumping);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (!isJumping)
        {
            transform.position = playerStartPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            isGrounded = true;
        }
        if (collision.gameObject == jumpPlace)
        {
            isJumpAvailable = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals("Floor"))
        {
            isGrounded = false;
        }
        if (collision.gameObject == jumpPlace)
        {
            isJumpAvailable = false;
        }
    }
}