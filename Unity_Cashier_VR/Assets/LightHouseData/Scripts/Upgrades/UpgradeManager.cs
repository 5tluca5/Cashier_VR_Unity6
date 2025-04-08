using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { private set; get; }

    private const string LeftHandUpgradeKey = "LeftHandUpgrade";
    private const string RightHandUpgradeKey = "RightHandUpgrade";
    private const int FIXED_UPGRADE_COST = 100;
    private const int MAX_UPGRADE_COUNT = 40;

    [Header("Upgrade Settings")]
    [SerializeField] private int leftHandUpgradeCount = 1;
    [SerializeField] private int rightHandUpgradeCount = 1;
    [SerializeField] private CurveInteractionCaster leftHandInteractor;
    [SerializeField] private CurveInteractionCaster rightHandInteractor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Load the upgrade counts from PlayerPrefs
        leftHandUpgradeCount = PlayerPrefs.GetInt(LeftHandUpgradeKey, 1);
        rightHandUpgradeCount = PlayerPrefs.GetInt(RightHandUpgradeKey, 1);

        leftHandInteractor.castDistance = leftHandUpgradeCount * 0.5f;
        rightHandInteractor.castDistance = rightHandUpgradeCount * 0.5f;
    }

    private void ApplyUpgradeSettings()
    {
        CoinPouchHandler.Instance.DeductCoins(FIXED_UPGRADE_COST); // Deduct coins when upgrades are applied

        leftHandInteractor.castDistance = leftHandUpgradeCount * 0.5f; // Example: each upgrade increases cast distance by 0.5 units
        rightHandInteractor.castDistance = rightHandUpgradeCount * 0.5f; // Example: each upgrade increases cast distance by 0.5 units
    }

    public int GetLeftHandUpgradeCount() => leftHandUpgradeCount;

    public int GetRightHandUpgradeCount() => rightHandUpgradeCount;

    public void UpgradeLeftHand()
    {
        if (leftHandUpgradeCount >= MAX_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_UPGRADE_COST) // Check if player has enough coins
            return;

        leftHandUpgradeCount++;
        ApplyUpgradeSettings();
        PlayerPrefs.SetInt(LeftHandUpgradeKey, leftHandUpgradeCount);
        PlayerPrefs.Save();
    }

    public void UpgradeRightHand()
    {
        if (rightHandUpgradeCount >= MAX_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_UPGRADE_COST) // Check if player has enough coins
            return;

        rightHandUpgradeCount++;
        ApplyUpgradeSettings();
        PlayerPrefs.SetInt(RightHandUpgradeKey, rightHandUpgradeCount);
        PlayerPrefs.Save();
    }
}
