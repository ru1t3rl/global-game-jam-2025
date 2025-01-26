using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BubblePuzzle.Audio
{
    public class ConfigurationStepsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _configurationSteps;

        [SerializeField]
        private Button previous;

        [SerializeField]
        private Button next;
        private TextMeshProUGUI nextText;

        private int _currentStep = 0;

        private void Awake()
        {
            next.onClick.AddListener(OnNextClicked);
            previous.onClick.AddListener(OnPreviousClicked);
            nextText = next.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Init();
        }

        private void OnDestroy()
        {
            next.onClick.RemoveListener(OnNextClicked);
            previous.onClick.RemoveListener(OnPreviousClicked);
        }

        private void Init()
        {
            for (int i = 0; i < _configurationSteps.Length; i++)
            {
                _configurationSteps[i].SetActive(false);
            }

            _configurationSteps[_currentStep].SetActive(true);

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            previous.interactable = _currentStep != 0;
            nextText.text = _currentStep == _configurationSteps.Length - 1
                ? "Confirm"
                : "Next";
        }

        private void OnNextClicked()
        {
            if (_currentStep < _configurationSteps.Length - 1)
            {
                _currentStep++;
                Init();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnPreviousClicked()
        {
            _currentStep--;
            Init();
        }
    }
}