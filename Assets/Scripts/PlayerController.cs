using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody2D),
    typeof(BoxCollider2D)
)]
public class PlayerController : MonoBehaviour
{
    public LayerMask environmentLayerMask;
    
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    private Vector2 _movementDirection;
    private bool _jumped;
    private bool _running;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);
        _jumped = Input.GetKey(KeyCode.Space);
        _running = Input.GetKey(KeyCode.LeftShift);
        
        float speed = (_running) ? 15.0f : 10.0f;
        transform.Translate(Vector3.right * _movementDirection * speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {

        if (_jumped && OnGround())
        {
            _rigidbody2D.AddForce(Vector2.up * 2.0f, ForceMode2D.Impulse);
        }
    }

    private bool OnGround()
    {
        Vector3 position = transform.position;
        position.y = position.y - transform.localScale.y;
        
        return Physics2D.OverlapCircle(position , 0.15f, environmentLayerMask);
    }
}
