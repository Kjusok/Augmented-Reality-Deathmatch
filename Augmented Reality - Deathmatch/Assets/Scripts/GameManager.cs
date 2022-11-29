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
    [SerializeField] private Toggle _toggleSetupMode;

    private int _counter;

    public List<GameObject> Enemies;


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

    private void InstantiateWarrior()
    {
        if (_counter < 5)
        {
            var enemy = Instantiate(_warriorPrefab, new Vector3(Random.Range(-12, 12), 0.062f, Random.Range(3, 20)), Quaternion.identity);
            Enemies.Add(enemy);

            AddNumbersOnUI();
        }
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

    private void CheckButtonDown()
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

    private void Update()
    {
        if (_counter == 5)
        {
            _toggleSetupMode.interactable = false;
        }
        else
        {
            _toggleSetupMode.interactable = true;
        }

        CheckButtonDown();

        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiateWarrior();
        }
    }

    
}
