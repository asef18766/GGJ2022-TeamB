using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingTarget : MonoBehaviour
{
    private Text _text;
    const string ALREADY = "¤w±O±þ";
    const string TARGET = "¥Ø¼Ð";
    private void UpdateText(int alreadyKillAmount, int targetAmount)
    {
        _text.text = $"{ALREADY} {alreadyKillAmount}/{TARGET} {targetAmount}";
    }
}