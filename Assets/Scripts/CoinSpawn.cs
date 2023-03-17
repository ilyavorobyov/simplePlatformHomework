using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] private Coin _coin;

    private Vector3[] _points;

    private void Start()
    {
        _points = new Vector3[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _points[i] = transform.GetChild(i).position;
            Instantiate(_coin, _points[i], Quaternion.identity);
        }
    }
}