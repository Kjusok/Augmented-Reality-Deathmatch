using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

    private int _counter;

    public List<GameObject> Enemies;
    public Toggle ToggleSetupMode;


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
        if (_counter == 5)
        {
            ToggleSetupMode.interactable = false;
            ToggleSetupMode.isOn = false;
        }
        else
        {
            ToggleSetupMode.interactable = true;
        }

        CheckButtonDown();
    }

    private void CheckButtonDown()
    {
        ColorBlock colorBlock = ToggleSetupMode.colors;

        if (ToggleSetupMode.isOn)
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

        ToggleSetupMode.colors = colorBlock;
    }

    private void AddNumbersOnUI()
    {
        _counter++;
        _numbersOfWarriorsOnSceneText.text = _counter.ToString() + "/5";
    }

    public void RemoveNumbersFromUI()
    {
        _counter--;
        _numbersOfWarriorsOnSceneText.text = _counter.ToString() + "/5";
    }

    public void InstantiateWarrior(Vector3 position, Quaternion rotation)
    {
        if (_counter < 5)
        {
            var enemy = Instantiate(_warriorPrefab, position, rotation);
            Enemies.Add(enemy);

            AddNumbersOnUI();
        }
    }

}
