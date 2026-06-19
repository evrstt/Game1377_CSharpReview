using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{

    [SerializeField] private int movementSpeed = 120;
    [SerializeField] private Rigidbody2D rb2D;

    private float moveX;
    private float moveY;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        Vector2 moveDirection = (moveY * Vector3.up + moveX * Vector3.right).normalized;
        rb2D.AddForce(moveDirection* movementSpeed, ForceMode2D.Force);
    }
}
