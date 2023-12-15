using System.Collections;
using UnityEngine;

public class HealthSmoothSliderView : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _smoothSlider;
    [SerializeField] private Health _health;
    [SerializeField] private float _handleSpeed;

    private Coroutine _changeValue;

    private void Start()
    {
        _smoothSlider.maxValue = _health.MaxHealth;
        _smoothSlider.value = _health.CurrentHealth;
    }

    private void OnEnable()
    {
        _health.HealthChange += OnChangeSliderValue;
    }

    private void OnDisable()
    {
        _health.HealthChange -= OnChangeSliderValue;
    }

    private void OnChangeSliderValue()
    {
        if(_changeValue != null)
            StopCoroutine( _changeValue );

        _changeValue = StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        bool isChangeSliderValue = true;

        while (isChangeSliderValue)
        {
            _smoothSlider.value = Mathf.MoveTowards(_smoothSlider.value, _health.CurrentHealth, 
                _handleSpeed * Time.deltaTime);

            if (_smoothSlider.value == _health.CurrentHealth)
            {
                isChangeSliderValue = false;
                StopCoroutine(_changeValue);
            }

            yield return waitForFixedUpdate;
        }
    }
}