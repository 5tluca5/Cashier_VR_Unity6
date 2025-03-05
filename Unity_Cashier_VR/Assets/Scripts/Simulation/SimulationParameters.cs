using UnityEngine;

[CreateAssetMenu(fileName = "SimulationParameters", menuName = "Scriptable Objects/SimulationParameters")]
public class SimulationParameters : ScriptableObject
{
    public string SimulationName = "";
    public float TimeScale = 1.0f;
    public float dt = .02f;
    public float StartTime = 0.0f;
    public float EndTime = 300.0f;

    [Header("M/M/1 Queue Parameters")]
    public float lambda = 24;
    public float mu = 40;
    
}
