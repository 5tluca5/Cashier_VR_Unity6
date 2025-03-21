using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplay : MonoBehaviour
{
    Dictionary<string, string> debugLogs = new Dictionary<string, string>();
    public TMPro.TextMeshProUGUI display;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = display.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //Debug.Log("time:" + Time.time);
        //Debug.Log(gameObject.name);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    List<string> allLogs = new List<string>();
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitString = logString.Split(char.Parse(":"));
            string debugKey = splitString[0];
            string debugValue = splitString.Length > 1 ? splitString[1] : "";

            if (debugLogs.ContainsKey(debugKey))
                debugLogs[debugKey] = debugValue;
            else
                debugLogs.Add(debugKey, debugValue);

            allLogs.Add(logString);
        }

        string displayText = "";
        //foreach (KeyValuePair<string, string> log in debugLogs)
        //{
        //    if (log.Value == "")
        //        displayText += log.Key + "\n";
        //    else
        //        displayText += log.Key + ": " + log.Value + "\n";
        //}

        foreach (string key in allLogs)
        {
            displayText += key + "\n";
        }

        display.text = displayText;

        //if(debugLogs.Count > 0)
        //    rectTransform.localPosition = new Vector3(0, debugLogs.Count * 0.5f, 0);

        if (allLogs.Count > 0)
            rectTransform.localPosition = new Vector3(0, allLogs.Count * 0.5f, 0);
    }
}