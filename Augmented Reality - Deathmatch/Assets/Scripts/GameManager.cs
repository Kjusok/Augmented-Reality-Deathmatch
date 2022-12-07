using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int MaxLimitOfAvailableWarriors = 5;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    [SerializeField] private Warrior _warriorPrefab;
    [SerializeField] private GameObject _setupModeText;
    [SerializeField] private GameObject _destroyModeText;
    [SerializeField] private Text _numbersOfWarriorsOnSceneText;
    [SerializeField] private List<Warrior> _enemies;
    [SerializeField] private Toggle _toggleSetupMode;
    [SerializeField] private Toggle _toggleDestroyMode;
 
    private int _amountWarriors;

    public List<Warrior> Enemies => _enemies;
    public Toggle ToggleSetupMode => _toggleSetupMode;
    public Toggle ToggleDestoryMode => _toggleDestroyMode;


    private void Awake()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
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

        SetButtonDown(_toggleSetupMode,_setupModeText);
        SetButtonDown(_toggleDestroyMode,_destroyModeText);
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

    public void WarriorDead()
    {
        _amountWarriors--;
        _numbersOfWarriorsOnSceneText.text = _amountWarriors + "/5";
    }

    public void TryInstantiateWarrior(Vector3 position, Quaternion rotation)
    {
        if (_amountWarriors < MaxLimitOfAvailableWarriors)
        {
            var enemy = Instantiate(_warriorPrefab, position, rotation);
            _enemies.Add(enemy);

            AddNumbersOnUI();
        }
    }
}
