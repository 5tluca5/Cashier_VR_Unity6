using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinPouchUIHandler : MonoBehaviour
{
    [SerializeField] private CoinPouchHandler coinPouchHandler;

    [SerializeField] private TextMeshProUGUI coinTMP;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 1f);
        coinTMP.text = "0";
    }

    private void OnEnable()
    {
        coinPouchHandler.OnCoinPouchGrabbed += OnCoinPouchGrabbed;
        coinPouchHandler.OnCoinPouchReleased += OnCoinPouchReleased;
        coinPouchHandler.OnCoinCountChanged += OnCoinDroppedInPouch;
    }

    private void OnDisable()
    {
        coinPouchHandler.OnCoinPouchGrabbed -= OnCoinPouchGrabbed;
        coinPouchHandler.OnCoinPouchReleased -= OnCoinPouchReleased;
        coinPouchHandler.OnCoinCountChanged -= OnCoinDroppedInPouch;
    }

    private void OnCoinDroppedInPouch(int coinCount)
    {
        coinTMP.text = coinCount.ToString();
    }

    private void OnCoinPouchGrabbed()
    {
        canvasGroup.DOFade(1f, 1f);
    }

    private void OnCoinPouchReleased()
    {
        canvasGroup.DOFade(0f, 1f);
    }
}
