using System.Collections.Generic;
using UnityEngine;

public class EnemiesPlayerSetter : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private Player _player;

    private void Awake()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.Init(_player);
        }
    }
}