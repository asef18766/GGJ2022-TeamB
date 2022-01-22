using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingTarget : MonoBehaviour
{
    [SerializeField] private Animator textAnimation;
    [SerializeField]private Text _text;
    [SerializeField] bool TempPlus;
    //private const string ALREADY = "已屠殺";
    //private const string TARGET ="目標";
    [SerializeField] private int _currentAmount;
    [SerializeField] private int _targetAmount;
    /// <summary>
    /// 直接設定
    /// </summary>
    /// <param name="currenAmount"></param>
    /// <param name="targetAmount"></param>
    public void UpdateText(int currenAmount, int targetAmount)
    {
        _currentAmount = currenAmount;
        _targetAmount = targetAmount;
        if (_currentAmount >= _targetAmount) _currentAmount = _targetAmount;
        _text.text = $"{_currentAmount}/{_targetAmount}";
        textAnimation.Play("Shake");
    }
    /// <summary>
    /// 增加數量
    /// </summary>
    public void Add()
    {
        UpdateText(_currentAmount + 1, _targetAmount);
    }
    /// <summary>
    /// 重設當前數量
    /// </summary>
    public void ResetCurrent()
    {
        _currentAmount = 0;
    }

    private void Update()
    {
        if (TempPlus)
        {
            TempPlus = false;
            Add();
        }
    }
}