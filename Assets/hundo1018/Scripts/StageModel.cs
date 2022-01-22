using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Assets.hundo1018.Scripts.Stage
{

    public class StageModel : MonoBehaviour
    {
        /// <summary>
        /// 時間改變委派
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public delegate void TimeEventHandler(object sender, TimeEventArgs eventArgs);
        /// <summary>
        /// 當時間更新時觸發
        /// </summary>
        public event TimeEventHandler OnTimeUpdatedEventHandler;
        /// <summary>
        /// 當時間到時被觸發
        /// </summary>
        public event TimeEventHandler OnStageChangedEventHandler;
        /// <summary>
        /// 最小晝夜時間(秒)
        /// </summary>
        [SerializeField] private float _duringTimeMin;
        /// <summary>
        /// 最大晝夜時間(秒)
        /// </summary>
        [SerializeField] private float _duringTimeMax;

        /// <summary>
        /// 當前階段剩餘時間(秒)
        /// </summary>
        [SerializeField] private float _currentRemainingTime;

        /// <summary>
        /// 當前階段剩餘時間只是是比例
        /// </summary>
        [SerializeField] private float _getCurrentRemainingRate { get => _currentRemainingTime / _currentMaxTime; }

        /// <summary>
        /// 當前階段最大時間(秒)
        /// </summary>
        private float _currentMaxTime;

        private bool isNight;
        /// <summary>
        /// 當前是否為夜，非夜即晝
        /// </summary>
        [SerializeField]
        private bool _isNight
        {
            get => isNight;
            set
            {
                if (isNight != value)
                    isNight = value;
                OnStageChangedEventHandler(this, new TimeEventArgs(isNight));
            }
        }

        [SerializeField] private bool _isContinue = false;
        /// <summary>
        /// 在最小與最大時間之間找一個時間代表當前階段
        /// </summary>
        float ChooseTime()
        {
            return Random.Range(_duringTimeMin, _duringTimeMax);
        }

        /// <summary>
        /// 當晝夜階段被狼人變身改變
        /// </summary>
        void OnStageChanged()
        {
            _currentMaxTime = ChooseTime();
            TimeRestart();
        }

        /// <summary>
        /// 時間從頭開始計算
        /// </summary>
        void TimeRestart()
        {
            _currentRemainingTime = _currentMaxTime;
            _isContinue = true;
        }


        /// <summary>
        /// 暫停
        /// </summary>
        void TimePause()
        {
            _isContinue = false;
        }

        /// <summary>
        /// 繼續
        /// </summary>
        void TimeContinue()
        {
            _isContinue = true;
        }

        /// <summary>
        /// 測試用糞code
        /// </summary>
        void TimeUpdateSim()
        {
            _currentRemainingTime -= Time.deltaTime;
            bool isTimeUp = _currentRemainingTime <= 0;

            OnTimeUpdatedEventHandler.Invoke(
                this,
                new TimeEventArgs(_currentRemainingTime, _getCurrentRemainingRate));
            if (isTimeUp)
            {
                _isNight = !_isNight;
                OnStageChanged();
            }
        }

        public bool TempWolfTrigger = false;
        // Update is called once per frame
        void Update()
        {
            //測試用
            if (TempWolfTrigger)
            {
                OnStageChanged();
                TempWolfTrigger = false;
            }
            //循環
            if (_isContinue)
            {
                TimeUpdateSim();
            }
        }
    }
}