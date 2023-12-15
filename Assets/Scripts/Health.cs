using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health;

    private float _minHealth = 0;

    private UnityEvent _healthChange = new UnityEvent();

    public event UnityAction HealthChange
    {
        add => _healthChange.AddListener(value);
        remove => _healthChange.RemoveListener(value);
    }

    public float MaxHealth => _health;
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        _healthChange.Invoke();

        if (CurrentHealth <= _minHealth)
        {
            gameObject.SetActive(false);
        }
    }

    public void AddHealth(int addingHealth)
    {
        if (CurrentHealth + addingHealth <= MaxHealth)
        {
            CurrentHealth += addingHealth;
            _healthChange.Invoke();
        }
    }
}