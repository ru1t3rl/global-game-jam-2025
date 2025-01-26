using System;
using BubblePuzzle.Behaviours;
using BubblePuzzle.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BubblePuzzle.Audio
{
    public class MicrophoneInputUI : MonoBehaviour
    {
        [SerializeField]
        private Slider volumeSlider;

        [SerializeField]
        private Slider multiplierSlider;

        [SerializeField]
        private MicrophoneConfiguration micConfiguration;

        private bool setFromEvent = false;

        private void OnEnable()
        {
            volumeSlider.value = 0;
            multiplierSlider.value = micConfiguration.InputMultiplier;

            multiplierSlider.onValueChanged.AddListener(UpdateConfigurationMultiplier);

            MicrophoneInput.Instance.onUpdate.AddListener(HandleInput);
            MicrophoneInput.Instance.StartCapturing();
        }

        private void OnDisable()
        {
            multiplierSlider.onValueChanged.RemoveListener(UpdateConfigurationMultiplier);

            MicrophoneInput.Instance.onUpdate.RemoveListener(HandleInput);
            MicrophoneInput.Instance.StopCapturing();
        }

        private void FixedUpdate()
        {
            if (setFromEvent)
            {
                setFromEvent = false;
                return;
            }

            volumeSlider.value = 0;
        }

        private void HandleInput(float volume)
        {
            volumeSlider.value = volume;
            setFromEvent = true;
        }

        private void UpdateConfigurationMultiplier(float value)
        {
            micConfiguration.InputMultiplier = value;
        }
    }
}