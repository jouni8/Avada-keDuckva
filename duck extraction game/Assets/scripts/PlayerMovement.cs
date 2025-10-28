using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats stats;
    public CharacterController controller;
    public Camera cam;
    public Transform body;

    public Vector3 velocity;
    public float stopSpeed;

    bool sprinting;
    bool crouching;

    public float horizontalM;
    public float verticalM;

    public Vector3 point;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(point, 0.1F);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateVelocity();
        Move();
    }


    private void Update()
    {
        PlayerInput();
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            point = hit.point;
        }
    }
    void PlayerInput()
    {
        horizontalM = Input.GetAxisRaw("Horizontal"); //read a/d
        verticalM = Input.GetAxisRaw("Vertical"); //read w/s

        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouching = true;
            sprinting = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
            crouching = false;
        }

        Vector3 moveDir = transform.forward * verticalM + transform.right * horizontalM; //player direction
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (sprinting && stats.stamina > 0)
            {
                velocity.z = Mathf.Lerp(velocity.z, moveDir.normalized.z * stats.sprintSpeed, stats.acceleration * Time.deltaTime);
                velocity.x = Mathf.Lerp(velocity.x, moveDir.normalized.x * stats.sprintSpeed, stats.acceleration * Time.deltaTime);
                stats.UseStamina(stats.sprintStaminaUse * Time.deltaTime);
            }
            else if(crouching)
            {
                velocity.z = Mathf.Lerp(velocity.z, moveDir.normalized.z * stats.crouchSpeed, stats.acceleration * Time.deltaTime);
                velocity.x = Mathf.Lerp(velocity.x, moveDir.normalized.x * stats.crouchSpeed, stats.acceleration * Time.deltaTime);
            }
            else
            {
                velocity.z = Mathf.Lerp(velocity.z, moveDir.normalized.z * stats.walkingSpeed, stats.acceleration * Time.deltaTime);
                velocity.x = Mathf.Lerp(velocity.x, moveDir.normalized.x * stats.walkingSpeed, stats.acceleration * Time.deltaTime);
            }

        }
    }
    void UpdateVelocity()
    {
        if(horizontalM == 0 && verticalM == 0)
        {
            if ((velocity.x > 0.2 || velocity.z > 0.2) || (velocity.x < -0.2 || velocity.z < -0.2))
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, stats.decelaration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0, stats.decelaration * Time.deltaTime);
            }
            else
            {
                velocity.x = 0;
                velocity.z = 0;
            }
        }
    }
    void Move()
    {
        controller.Move(velocity * Time.deltaTime);

        body.LookAt(new Vector3(point.x, gameObject.transform.position.y, point.z));
    }
}
