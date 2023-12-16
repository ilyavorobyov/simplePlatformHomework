using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class VampirismAbility : MonoBehaviour
{
    [SerializeField] private float _vampirismRange;

    private Health health;
    private int _vampirismDuration = 6;
    private int _drainingHealthPerIteration = 1;
    private Coroutine _drainHealth;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (TrySearchEnemy(out Enemy enemy))
            {
                StopDrainHealth();
                _drainHealth = StartCoroutine(DrainHealth(enemy));
            }
        }
    }

    private void OnDestroy()
    {
        StopDrainHealth();
    }

    private void StopDrainHealth()
    {
        if (_drainHealth != null)
        {
            StopCoroutine(_drainHealth);
        }
    }

    private bool TrySearchEnemy(out Enemy enemy)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _vampirismRange);
        List<Enemy> enemiesWithinAbilityRange = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.TryGetComponent(out Enemy enemyTemplate))
            {
                enemiesWithinAbilityRange.Add(enemyTemplate);
            }
        }

        if (enemiesWithinAbilityRange.Count > 0)
        {
            enemy = enemiesWithinAbilityRange.OrderBy(enemy =>
            Vector3.Distance(enemy.transform.position, transform.position)).FirstOrDefault();
            return true;
        }
        else
        {
            enemy = null;
            return false;
        }
    }

    private IEnumerator DrainHealth(Enemy enemy)
    {
        float iterationTime = 0.4f;
        float currentDrainingTime = 0;
        var waitForSeconds = new WaitForSeconds(iterationTime);
        float abilityRange = 3;
        float distance = Vector3.Distance(enemy.transform.position, transform.position);

        while (enemy.isActiveAndEnabled && currentDrainingTime < _vampirismDuration &&
            distance <= abilityRange)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position);
            enemy.TakeDamage(_drainingHealthPerIteration);
            health.AddHealth(_drainingHealthPerIteration);
            currentDrainingTime += iterationTime;
            yield return waitForSeconds;
        }

        StopDrainHealth();
    }
}