using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingTarget : MonoBehaviour
{
    [SerializeField] private Animator textAnimation;
    [SerializeField]private Text _text;
    [SerializeField] bool TempPlus;
    //private const string ALREADY = "�w�O��";
    //private const string TARGET ="�ؼ�";
    [SerializeField] private int _currentAmount;
    [SerializeField] private int _targetAmount;
    /// <summary>
    /// �����]�w
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
    /// �W�[�ƶq
    /// </summary>
    public void Add()
    {
        UpdateText(_currentAmount + 1, _targetAmount);
    }
    /// <summary>
    /// ���]��e�ƶq
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