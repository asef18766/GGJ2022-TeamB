using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Assets.hundo1018.Scripts.Stage
{

    public class StageModel : MonoBehaviour
    {
        /// <summary>
        /// �ɶ����ܩe��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public delegate void TimeEventHandler(object sender, TimeEventArgs eventArgs);
        /// <summary>
        /// ��ɶ���s��Ĳ�o
        /// </summary>
        public event TimeEventHandler OnTimeUpdatedEventHandler;
        /// <summary>
        /// ��ɶ���ɳQĲ�o
        /// </summary>
        public event TimeEventHandler OnStageChangedEventHandler;
        /// <summary>
        /// �̤p�ީ]�ɶ�(��)
        /// </summary>
        [SerializeField] private float _duringTimeMin;
        /// <summary>
        /// �̤j�ީ]�ɶ�(��)
        /// </summary>
        [SerializeField] private float _duringTimeMax;

        /// <summary>
        /// ��e���q�Ѿl�ɶ�(��)
        /// </summary>
        [SerializeField] private float _currentRemainingTime;

        /// <summary>
        /// ��e���q�Ѿl�ɶ��u�O�O���
        /// </summary>
        [SerializeField] private float _getCurrentRemainingRate { get => _currentRemainingTime / _currentMaxTime; }

        /// <summary>
        /// ��e���q�̤j�ɶ�(��)
        /// </summary>
        private float _currentMaxTime;

        private bool isNight;
        /// <summary>
        /// ��e�O�_���]�A�D�]�Y��
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
        /// �b�̤p�P�̤j�ɶ�������@�Ӯɶ��N���e���q
        /// </summary>
        float ChooseTime()
        {
            return Random.Range(_duringTimeMin, _duringTimeMax);
        }

        /// <summary>
        /// ��ީ]���q�Q�T�H�ܨ�����
        /// </summary>
        void OnStageChanged()
        {
            _currentMaxTime = ChooseTime();
            TimeRestart();
        }

        /// <summary>
        /// �ɶ��q�Y�}�l�p��
        /// </summary>
        void TimeRestart()
        {
            _currentRemainingTime = _currentMaxTime;
            _isContinue = true;
        }


        /// <summary>
        /// �Ȱ�
        /// </summary>
        void TimePause()
        {
            _isContinue = false;
        }

        /// <summary>
        /// �~��
        /// </summary>
        void TimeContinue()
        {
            _isContinue = true;
        }

        /// <summary>
        /// ���ե��Tcode
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
            //���ե�
            if (TempWolfTrigger)
            {
                OnStageChanged();
                TempWolfTrigger = false;
            }
            //�`��
            if (_isContinue)
            {
                TimeUpdateSim();
            }
        }
    }
}