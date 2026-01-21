using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinCollecting : MonoBehaviour
{
    [SerializeField] private GameObject animatedCoinPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private AnimType type;

    [Space] [Header("Available coins : (coins to pool)")] [SerializeField]
    private int maxCoins = 100;

    private readonly Queue<GameObject> _coinsQueue = new();

    [Header("For line anim")] [SerializeField]
    private float delayBetweenCoins = 0.1f;

    [SerializeField] private float moveCoinDuration = 0.5f;

    [Header("For spread anim")] [SerializeField]
    private float spreadRadius = 2;

    [SerializeField] private float spreadDuration = 0.5f;
    [SerializeField] private float collectDuration = 0.5f;


    private void Awake()
    {
        //prepare pool
        PrepareCoins();
    }

    private void PrepareCoins()
    {
        for (var i = 0; i < maxCoins; i++)
        {
            var coin = Instantiate(animatedCoinPrefab, transform);
            coin.SetActive(false);
            _coinsQueue.Enqueue(coin);
        }
    }

    private IEnumerator LineAnim(Vector3 collectedCoinPosition, int amount, Action onCollectComplete = null)
    {
        var coinsSeq = DOTween.Sequence();
        for (var i = 0; i < amount; i++)
        {
            //check if there's coins in the pool
            if (_coinsQueue.Count <= 0) continue;
            //extract a coin from the pool
            var coin = _coinsQueue.Dequeue();
            coin.SetActive(true);

            //move coin to the collected coin pos
            coin.transform.position = collectedCoinPosition;

            var coinSeq = DOTween.Sequence();
            coinSeq.Append(coin.transform.DOMove(target.position, moveCoinDuration)
                .OnComplete(() =>
                {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    _coinsQueue.Enqueue(coin);
                }));
            coinsSeq.Join(coinSeq);
            yield return Helpers.GetWaitForSeconds(delayBetweenCoins);
        }

        coinsSeq.OnComplete(() => onCollectComplete?.Invoke());
    }

    private void SpreadAnim(Vector3 collectedCoinPosition, int amount, Action onCollectComplete = null)
    {
        var coinsSeq = DOTween.Sequence();
        var targetPos = target.position;
        for (var i = 0; i < amount; i++)
        {
            //check if there's coins in the pool
            if (_coinsQueue.Count <= 0) continue;
            //extract a coin from the pool
            var coin = _coinsQueue.Dequeue();
            coin.SetActive(true);

            //move coin to the collected coin pos
            coin.transform.position = collectedCoinPosition;

            //animate coin to target position
            var coinSeq = DOTween.Sequence();
            coinSeq.Append(coin.transform.DOJump(
                collectedCoinPosition + (Vector3)(Random.insideUnitCircle * spreadRadius), 5f, 1,
                Random.Range(spreadDuration - 0.1f, spreadDuration + 0.1f)));
            coinSeq.Append(coin.transform
                .DOMove(targetPos,
                    Random.Range(collectDuration - 0.1f, collectDuration + 0.1f)).SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    _coinsQueue.Enqueue(coin);
                }));
            coinsSeq.Join(coinSeq);
        }

        coinsSeq.OnComplete(() => onCollectComplete?.Invoke());
    }


    [Button]
    public void CollectCoins(Vector3 collectedCoinPosition, int amount, Action onCollectComplete = null)
    {
        switch (type)
        {
            case AnimType.Spread:

                SpreadAnim(collectedCoinPosition, amount, () => { onCollectComplete?.Invoke(); });
                break;
            case AnimType.Line:
                StartCoroutine(LineAnim(collectedCoinPosition, amount, () => { onCollectComplete?.Invoke(); }));
                break;
            default:
                SpreadAnim(collectedCoinPosition, amount, () => { onCollectComplete?.Invoke(); });
                break;
        }
    }
}

public enum AnimType
{
    Spread,
    Line
}