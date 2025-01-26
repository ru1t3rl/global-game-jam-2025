using System;
using System.Linq;
using BubblePuzzle.Utilities;
using TMPro;
using UnityEngine;

namespace BubblePuzzle.Audio
{
    public class AudioDeviceDropdown : MonoBehaviour
    {
        [SerializeField]
        private MicrophoneConfiguration micConfiguration;

        [SerializeField]
        private TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown.onValueChanged.AddListener(SetMicrophoneIndex);
        }

        private void OnEnable()
        {
            SetDropdownOptions();
        }

        private void SetDropdownOptions()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(Microphone.devices.ToList());
            
            if (micConfiguration.DeviceIndex < Microphone.devices.Length)
            {
                _dropdown.SetValueWithoutNotify(micConfiguration.DeviceIndex);
            }
            else
            {
                _dropdown.SetValueWithoutNotify(0);
            }
            
            _dropdown.RefreshShownValue();
        }

        public void SetMicrophoneIndex(int index)
        {
            micConfiguration.DeviceIndex = index;
        }
    }
}