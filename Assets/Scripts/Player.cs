using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string Jump = "Jump";
    private const string Run = "Run";

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private int _wallet = 0;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            _spriteRenderer.flipX = true;
            _animator.SetTrigger(Run);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            _spriteRenderer.flipX = false;
            _animator.SetTrigger(Run);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _animator.SetTrigger(Jump);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Coin>(out Coin coin))
        {
            _wallet += coin.CoinPar;
            Debug.Log($"У игрока {_wallet} монет");
            Destroy(coin.gameObject);
        }

        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Debug.Log(_health);

        if (_health <= 0)
        {
            Debug.Log("игрок погиб");
            Time.timeScale = 0;
        }
    }

    public void AddHealth(int addingHealth)
    {
        _health += addingHealth;
        Debug.Log($"Игрок прибавил {addingHealth} к здоровью");
    }
}