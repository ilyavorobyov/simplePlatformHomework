using Random = System.Random;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Random _random = new Random();
    public int CoinPar { get; private set; }

    private void Start()
    {
        int minPar = 1;
        int maxPar = 5;
        CoinPar = _random.Next(minPar, maxPar);
    }
}