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

    [SerializeField] private GameObject _warriorPrefab;
    [SerializeField] private GameObject _spawnAuraEffect;
    [SerializeField] private GameObject _setupModeText;
    [SerializeField] private Text _numbersOfWarriorsOnSceneText;
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private Toggle _toggleSetupMode;

    private int _counter;

    public List<GameObject> Enemies => _enemies;
    public Toggle ToggleSetupMode => _toggleSetupMode;


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
        if (Input.GetKeyDown(KeyCode.S))
        {
            var enemy = Instantiate(_warriorPrefab, new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)), Quaternion.identity);
            _enemies.Add(enemy);
        }

        if (_counter == MaxLimitOfAvailableWarriors)
        {
            _toggleSetupMode.interactable = false;
            _toggleSetupMode.isOn = false;
        }
        else
        {
            _toggleSetupMode.interactable = true;
        }

        SetButtonDown();
    }

    private void SetButtonDown()
    {
        ColorBlock colorBlock = _toggleSetupMode.colors;

        if (_toggleSetupMode.isOn)
        {
            _setupModeText.SetActive(true);
            colorBlock.normalColor = Color.gray;
            colorBlock.highlightedColor = Color.gray;
            colorBlock.selectedColor = Color.gray;
        }
        else
        {
            _setupModeText.SetActive(false);
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.selectedColor = Color.white;
        }

        _toggleSetupMode.colors = colorBlock;
    }

    private void AddNumbersOnUI()
    {
        _counter++;
        _numbersOfWarriorsOnSceneText.text = _counter + "/5";
    }

    public void WarriorDead()
    {
        _counter--;
        _numbersOfWarriorsOnSceneText.text = _counter + "/5";
    }

    public void TryInstantiateWarrior(Vector3 position, Quaternion rotation)
    {
        if (_counter < MaxLimitOfAvailableWarriors)
        {
            var enemy = Instantiate(_warriorPrefab, position, rotation);
            _enemies.Add(enemy);

            AddNumbersOnUI();
        }
    }
}
