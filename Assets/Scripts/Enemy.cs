using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _damage;

    private const string AttackAnimationName = "Attack";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Player _player;
    private Health _health;
    private List<Vector3> _points;
    private Transform[] _allChild;
    private Vector3 _target;
    private float _playerDistance;
    private float _viewDistance = 6;
    private int _currentPoint;
    private bool _isMoving;
    private bool _isAttacking;
    private bool _isFollowPlayer;
    private Coroutine _attackPlayer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _allChild = new Transform[transform.childCount];
        _points = new List<Vector3>();
        _isMoving = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            _allChild[i] = gameObject.transform.GetChild(i);
        }

        foreach (var child in _allChild)
        {
            if (child.transform.TryGetComponent(out TargetPoint targetPoint))
            {
                _points.Add(targetPoint.transform.position);
            }
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            _target = _points[_currentPoint];
            RotateSprite();
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed
                * Time.deltaTime);

            if (transform.position == _target)
            {
                _currentPoint++;

                if (_currentPoint >= _points.Count)
                {
                    _currentPoint = 0;
                }
            }
        }

        if (_isFollowPlayer)
        {
            _target = _player.transform.position;
            RotateSprite();
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed
                * Time.deltaTime);
        }

        _playerDistance = Vector3.Distance(_player.transform.position, transform.position);

        if (_playerDistance <= _viewDistance)
        {
            _isFollowPlayer = true;
            _isMoving = false;
        }
        else
        {
            _isFollowPlayer = false;
            _isMoving = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _isAttacking = true;
            _isMoving = false;

            if (_attackPlayer != null)
                StopCoroutine(_attackPlayer);

            _attackPlayer = StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _isAttacking = false;
            _isMoving = true;

            if (_attackPlayer != null)
                StopCoroutine(_attackPlayer);
        }
    }

    public void Init(Player player)
    {
        _player = player;
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }

    private void RotateSprite()
    {
        if (_target.x > transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private IEnumerator AttackPlayer()
    {
        var waitForSeconds = new WaitForSeconds(_attackCooldown);

        while (_isAttacking)
        {
            _player.TakeDamage(_damage);
            _animator.SetTrigger(AttackAnimationName);
            yield return waitForSeconds;
        }
    }
}