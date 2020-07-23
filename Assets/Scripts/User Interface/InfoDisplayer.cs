using System.Collections;
using TMPro;
using UnityEngine;

public class InfoDisplayer : MonoBehaviour
{
    //Reference Variables
    public static InfoDisplayer sharedInstance;

    //Configuration Parameters
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] float waitTime = 0.5f;

    //State Variables
    private TextMeshProUGUI[] textFields = null;
    private bool[] displayStatus = null;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        GetTextFields();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void GetTextFields() {
        textFields = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        if (textFields.Length == 0) {
            Debug.LogError("No Text Fields Available For InfoDisplayer");
            gameObject.SetActive(false);
        }
    }

    private void Start() {
        InitializeDisplays();
    }

    private void InitializeDisplays() {
        displayStatus = new bool[textFields.Length];
        for (int i = 0; i < textFields.Length; i++) {
            textFields[i].text = "";
            displayStatus[i] = false;
        }
    }

    private IEnumerator DisplayText(string text) {
        int index = GetAvailableDisplayIndex();
        textFields[index].text = text;
        displayStatus[index] = true;
        float timer = 0f;
        while (timer <= fadeTime) {
            textFields[index].color = Color.Lerp(Color.clear, Color.white, timer / fadeTime);
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        textFields[index].color = Color.white;
        timer = 0f;
        while (timer <= waitTime) {
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        timer = 0f;
        while (timer <= fadeTime) {
            textFields[index].color = Color.Lerp(Color.white, Color.clear, timer / fadeTime);
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        textFields[index].color = Color.clear;
        textFields[index].text = "";
        displayStatus[index] = false;
    }

    private int GetAvailableDisplayIndex() {
        for(int i = 0; i < displayStatus.Length; i++) {
            if (displayStatus[i] == false) {
                return i;
            }
        }
        return 0;
    }

    //Public Methods
    public void DisplayInfo(string text) {
        StartCoroutine(DisplayText(text));
    }

    public void ClearDisplays() {
        StopAllCoroutines();
        foreach(TextMeshProUGUI textField in textFields) {
            textField.text = "";
        }
    }
}
