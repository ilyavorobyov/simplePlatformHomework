using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _damage;

    private const string JumpAnimationName = "Jump";
    private const string RunAnimationName = "Run";
    private const string AttackAnimationName = "Attack";

    private int _wallet = 0;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Health _health;
    private Coroutine _attackEnemy;
    private bool _isAttacking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            _spriteRenderer.flipX = true;
            _animator.SetTrigger(RunAnimationName);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            _spriteRenderer.flipX = false;
            _animator.SetTrigger(RunAnimationName);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce);
            _animator.SetTrigger(JumpAnimationName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
        {
            _wallet += coin.CoinPar;
            Destroy(coin.gameObject);
        }

        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _isAttacking = true;

            if (_attackEnemy != null)
                StopCoroutine(_attackEnemy);

            _attackEnemy = StartCoroutine(AttackEnemy(enemy));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _isAttacking = false;

            if (_attackEnemy != null)
                StopCoroutine(_attackEnemy);
        }
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }

    public void AddHealth(int addingHealth)
    {
        _health.AddHealth(addingHealth); 
    }

    private IEnumerator AttackEnemy(Enemy enemy)
    {
        var waitForSeconds = new WaitForSeconds(_attackCooldown);

        while (_isAttacking)
        {
            _animator.SetTrigger(AttackAnimationName);
            enemy.TakeDamage(_damage);
            yield return waitForSeconds;
        }
    }
}