using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private const string Attack = "Attack";

    [SerializeField] private float _speed;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector3[] _points;
    private int _currentPoint;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _points = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _points[i] = transform.GetChild(i).position;
        }
    }
    private void Update()
    {
        Vector3 target = _points[_currentPoint];

        if (target.x > transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if (transform.position == target)
        {
            _currentPoint++;

            if (_currentPoint >= _points.Length)
            {
                _currentPoint = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _animator.SetTrigger(Attack);
        }
    }
}