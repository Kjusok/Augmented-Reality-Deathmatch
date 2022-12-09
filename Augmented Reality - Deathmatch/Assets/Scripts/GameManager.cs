using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int MaxLimitOfAvailableWarriors = 5;

    [SerializeField] private Warrior _warriorPrefab;
    [SerializeField] private GameObject _setupModeText;
    [SerializeField] private GameObject _destroyModeText;
    [SerializeField] private Text _numbersOfWarriorsOnSceneText;
    [SerializeField] private List<Warrior> _enemies;
    [SerializeField] private Toggle _toggleSetupMode;
    [SerializeField] private Toggle _toggleDestroyMode;

    private int _amountWarriors;

    public IEnumerable<Warrior> Enemies => _enemies;
    public bool IsSetupState
    {
        get; private set;
    }
    public bool IsDestroyState
    {
        get; private set;
    }


    private void Update()
    {
        if (_amountWarriors == MaxLimitOfAvailableWarriors)
        {
            _toggleSetupMode.interactable = false;
            _toggleSetupMode.isOn = false;
        }
        else
        {
            _toggleSetupMode.interactable = true;
        }

        SetButtonDown(_toggleSetupMode, _setupModeText);
        SetButtonDown(_toggleDestroyMode, _destroyModeText);

        CheckToggleState();
    }

    private void CheckToggleState()
    {
        if (_toggleSetupMode.isOn)
        {
            IsSetupState = true;
        }
        else
        {
            IsSetupState = false;
        }

        if (_toggleDestroyMode.isOn)
        {
            IsDestroyState = true;
        }
        else
        {
            IsDestroyState = false;
        }
    }

    private void SetButtonDown(Toggle mode, GameObject modeText)
    {
        ColorBlock colorBlock = _toggleSetupMode.colors;

        if (mode.isOn)
        {
            modeText.SetActive(true);
            colorBlock.normalColor = Color.gray;
            colorBlock.highlightedColor = Color.gray;
            colorBlock.selectedColor = Color.gray;
        }
        else
        {
            modeText.SetActive(false);
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.selectedColor = Color.white;
        }

        mode.colors = colorBlock;
    }

    private void AddNumbersOnUI()
    {
        _amountWarriors++;
        _numbersOfWarriorsOnSceneText.text = _amountWarriors + "/5";
    }

    private void WarriorDead(Warrior warrior)
    {
        warrior.Dead -= WarriorDead;

        _amountWarriors--;
        _numbersOfWarriorsOnSceneText.text = _amountWarriors + "/5";
        _enemies.Remove(warrior);
    }

    public void TryInstantiateWarrior(Vector3 position, Quaternion rotation)
    {
        if (_amountWarriors < MaxLimitOfAvailableWarriors)
        {
            var enemy = Instantiate(_warriorPrefab, position, rotation);
            _enemies.Add(enemy);

            enemy.Init(this);
            enemy.Dead += WarriorDead;

            AddNumbersOnUI();
        }
    }
}
