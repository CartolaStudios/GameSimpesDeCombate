using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    bool IsMovingUp
    {
        set
        {
            isMovingUp = value;
            animator.SetBool("isMovingUp", isMovingUp);
        }
    }
    bool IsMovingDown
    {
        set
        {
            isMovingDown = value;
            animator.SetBool("isMovingDown", isMovingDown);
        }
    }
    [SerializeField] private bool Mobile;//<<< essa opiçao decide para qual plataforma o seu codigo fuciona! para celular entao e verdadeira para pc deixe falsa
    [SerializeField] private VariableJoystick VariableJoystick;
    [SerializeField] private Button ButtonAttack;
    public float moveSpeed = 500f;
    public float maxSpeed = 8f;
    public float idleFriction = 0.9f;
    SpriteRenderer spriteRenderer;
    public Vector2 moveInput = Vector2.zero;
    Rigidbody2D rb;
    Animator animator;

    public GameObject attackHitBox;

    bool isMoving = false;
    bool isMovingUp = false;
    bool isMovingDown = false;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        MobileConver();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

        if (Mobile)
        {
            moveInput = VariableJoystick.Direction;
        }
        if (canMove && moveInput != Vector2.zero)
        {

            // Accelerate with cap

            rb.AddForce(moveInput * moveSpeed * Time.deltaTime);
            if (rb.velocity.magnitude > maxSpeed)
            {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;
            }

            if (moveInput.y > 0)
            {

                if (moveInput.x <= 0.2f && moveInput.x >= -0.2f)
                {
                    moveUp();
                }

            }
            else if (moveInput.y < 0)
            {

                if (moveInput.x <= 0.2f && moveInput.x >= -0.2f)
                {
                    moveDow();
                }
            }
            if (moveInput.x != 0)
            {
                // look left or right
                if (moveInput.x > 0)
                {

                    if (moveInput.x >= 0.2f)
                    {
                        moveRight();
                    }
                }
                else
                {
                    if (moveInput.x <= -0.2f)
                    {
                        moveleft();
                    }
                }
            }
        }
        else
        {
            // no movement
            IsMoving = false;
            IsMovingUp = false;
            IsMovingDown = false;
        }
    }

    private void MobileConver()
    {
        if (Mobile)
        {
            ButtonAttack.onClick.AddListener(OnFire);//<<esse codigo fas adicionar a void de attack diretamente no butao de attack!!
            //vantagens de usar isso e envitar configurar na unity! mas nao se esqueça de conferir se o butao esta anexado na variavel



            Debug.Log("mobile esta ativado Joystick funciona");
            ButtonAttack.gameObject.SetActive(true);
            VariableJoystick.gameObject.SetActive(true);
            GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            Debug.Log("mobile esta desativado Joystick não funciona");
            ButtonAttack.gameObject.SetActive(false);
            VariableJoystick.gameObject.SetActive(false);
            GetComponent<PlayerInput>().enabled = true;
        }
    }


    public void moveUp()
    {
        IsMovingUp = true;
        IsMovingDown = false;
        IsMoving = false;
        gameObject.BroadcastMessage("IsFacing", 1);
    }
    public void moveDow()
    {
        IsMovingUp = false;
        IsMovingDown = true;
        IsMoving = false;
        gameObject.BroadcastMessage("IsFacing", 2);
    }
    public void moveRight()
    {
        spriteRenderer.flipX = false;
        gameObject.BroadcastMessage("IsFacing", 3);
        IsMoving = true;
        IsMovingUp = false;
        IsMovingDown = false;
    }


    public void moveleft()
    {
        print("moveInput.x >atras");
        spriteRenderer.flipX = true;
        gameObject.BroadcastMessage("IsFacing", 4);
        IsMoving = true;
        IsMovingUp = false;
        IsMovingDown = false;
    }





    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnFire()
    {
        animator.SetTrigger("Attack");
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
