using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    public static TestManager instance;
    [Header("Test Mode Activator")]
    [SerializeField] private bool testModeOn = false; public bool IsTestModeOn { get { return testModeOn; } }
    [Header("Test Things")]
    [SerializeField] private Class testClass;
    [SerializeField] private MapData testMap;
    [Header("Test References")]
    [SerializeField] private GameObject testRoomButton;
    [SerializeField] private GameObject testWaveButton;
    [SerializeField] private GameObject unlockallButton;
    [SerializeField] private GameObject eraseSaveButton;
    private void Awake() {
        if (instance == null)
            instance = this;
        
        testRoomButton.SetActive(testModeOn);
        testWaveButton.SetActive(testModeOn);
        unlockallButton.SetActive(testModeOn);
        eraseSaveButton.SetActive(testModeOn);
        if (testModeOn) Debug.LogWarning("Test mode is on");
    }

    public void ToTestroom() {
        CharacterClassManager.instance.SetPlayerClass(testClass);
        SceneManager.LoadScene("Testroom");
        //MapManager.instance.LoadMap("Testroom", false);
    }
    public void ToTestwave() {
        CharacterClassManager.instance.SetPlayerClass(testClass);
        SceneManager.LoadScene("TestMap");
    }
    public void UnlockAll() {
        CharacterClassManager.instance.UnlockAll();
        ObjectiveManager.instance.UnlockAll();
        MapManager.instance.UnlockAll();
        AddOffGameGold(69420);
    }
    public void EraseSave() {
        //Objective.instance.UnlockAll();
    }

    public void AddOffGameGold(int amount) {
        MainMenuManager.instance.AddGold(amount);
        MainMenuManager.instance.SaveGame();
        MainMenuManager.instance.LoadGame();
    }
    

    public void SetTestClass() {
        CharacterClassManager.instance.SetPlayerClass(-69);
    }
    public Class GetTestClass() {
        return testClass;
    }
}
