using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.hundo1018.Scripts.Stage
{

    public class StageView : MonoBehaviour
    {
        StageModel _stageModel;
        [SerializeField] private Sprite _sun;
        [SerializeField] private Sprite _moon;
        [SerializeField] private Animator _SunMoonAnimator;
        private Image _image;
        private void Awake()
        {
            _stageModel = GameObject.Find("Stage").GetComponent<StageModel>();
            _stageModel.OnTimeUpdatedEventHandler += OnTimeUpdate;
            _stageModel.OnStageChangedEventHandler += OnStageChanged;
            _image = GetComponent<Image>();
        }

        public void OnTimeUpdate(object sender, TimeEventArgs args)
        {
            _image.fillAmount = args.RemainingRate;
            _image.color = Color.HSVToRGB(0, 0, args.RemainingRate);
        }

        public void OnStageChanged(object sender, TimeEventArgs args)
        {
            _image.sprite = (args.IsNight ? _moon : _sun);
            if (_sun) ResetToSun();
            if (args.IsNight) _SunMoonAnimator.Play("ShowMoon");
        }

        private void ResetToSun()
        {
            _image.fillAmount = 1;
            _image.color = Color.HSVToRGB(0, 0, 1);
        }
    }
}
