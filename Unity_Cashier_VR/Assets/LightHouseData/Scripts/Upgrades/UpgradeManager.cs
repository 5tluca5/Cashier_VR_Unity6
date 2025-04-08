using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { private set; get; }

    // Hands upgrade data
    private const string LeftHandUpgradeKey = "LeftHandUpgrade";
    private const string RightHandUpgradeKey = "RightHandUpgrade";
    private const int FIXED_HAND_UPGRADE_COST = 100;
    private const int MAX_HAND_UPGRADE_COUNT = 40;

    // Item spawn time reduction data
    private const string UncommonItemSpawnTimeReductionKey = "UncommonItemSpawnTimeReduction";
    private const string GoldenItemSpawnTimeReductionKey = "GoldenItemSpawnTimeReduction";
    private const int FIXED_UNCOMMON_ITEM_SPAWN_TIME_REDUCTION_COST = 500;
    private const int FIXED_GOLDEN_ITEM_SPAWN_TIME_REDUCTION_COST = 2000;
    private const int MAX_UNCOMMON_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT = 10;
    private const int MAX_GOLDEN_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT = 10;
    private const int UNCOMMON_ITEM_SPAWN_TIME_REDUCTION = 2;
    private const int GOLDEN_ITEM_SPAWN_TIME_REDUCTION = 5;
    public event System.Action OnSpawnTimeReductionUpgraded;

    public bool IsLeftHandUpgradeMaxed => leftHandUpgradeCount >= MAX_HAND_UPGRADE_COUNT;
    public bool IsRightHandUpgradeMaxed => rightHandUpgradeCount >= MAX_HAND_UPGRADE_COUNT;
    public bool IsUncommonItemSpawnTimeReductionMaxed => uncommonItemSpawnTimeReductionUpgradeCount >= MAX_UNCOMMON_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT;
    public bool IsGoldenItemSpawnTimeReductionMaxed => goldenItemSpawnTimeReductionUpgradeCount >= MAX_GOLDEN_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT;

    [Header("Upgrade Settings")]
    [SerializeField] private CurveInteractionCaster leftHandInteractor;
    [SerializeField] private CurveInteractionCaster rightHandInteractor;

    private int leftHandUpgradeCount = 1;
    private int rightHandUpgradeCount = 1;
    private int uncommonItemSpawnTimeReductionUpgradeCount = 2;
    private int goldenItemSpawnTimeReductionUpgradeCount = 2;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadUpgradesDataFromPlayerPrefs();
    }

    private void LoadUpgradesDataFromPlayerPrefs()
    {
        // Load the upgrade counts from PlayerPrefs
        leftHandUpgradeCount = PlayerPrefs.GetInt(LeftHandUpgradeKey, 1);
        rightHandUpgradeCount = PlayerPrefs.GetInt(RightHandUpgradeKey, 1);
        uncommonItemSpawnTimeReductionUpgradeCount = PlayerPrefs.GetInt(UncommonItemSpawnTimeReductionKey, 2);
        goldenItemSpawnTimeReductionUpgradeCount = PlayerPrefs.GetInt(GoldenItemSpawnTimeReductionKey, 2);

        leftHandInteractor.castDistance = leftHandUpgradeCount * 0.5f;
        rightHandInteractor.castDistance = rightHandUpgradeCount * 0.5f;
    }

    private void ApplyHandUpgradeSettings()
    {
        CoinPouchHandler.Instance.DeductCoins(FIXED_HAND_UPGRADE_COST); // Deduct coins when upgrades are applied

        leftHandInteractor.castDistance = leftHandUpgradeCount * 0.5f; // Example: each upgrade increases cast distance by 0.5 units
        rightHandInteractor.castDistance = rightHandUpgradeCount * 0.5f; // Example: each upgrade increases cast distance by 0.5 units
    }

    public int GetLeftHandUpgradeCount() => leftHandUpgradeCount;

    public int GetRightHandUpgradeCount() => rightHandUpgradeCount;

    public int GetUncommonItemSpawnTimeReductionInSec() => uncommonItemSpawnTimeReductionUpgradeCount * UNCOMMON_ITEM_SPAWN_TIME_REDUCTION;

    public int GetGoldenItemSpawnTimeReductionInSec() => goldenItemSpawnTimeReductionUpgradeCount * GOLDEN_ITEM_SPAWN_TIME_REDUCTION;

    public void UpgradeLeftHand()
    {
        if (leftHandUpgradeCount >= MAX_HAND_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_HAND_UPGRADE_COST) // Check if player has enough coins
            return;

        leftHandUpgradeCount++;
        ApplyHandUpgradeSettings();
        PlayerPrefs.SetInt(LeftHandUpgradeKey, leftHandUpgradeCount);
        PlayerPrefs.Save();
    }

    public void UpgradeRightHand()
    {
        if (rightHandUpgradeCount >= MAX_HAND_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_HAND_UPGRADE_COST) // Check if player has enough coins
            return;

        rightHandUpgradeCount++;
        ApplyHandUpgradeSettings();
        PlayerPrefs.SetInt(RightHandUpgradeKey, rightHandUpgradeCount);
        PlayerPrefs.Save();
    }

    public void UpgradeUncommonItemSpawnTimeReduction()
    {
        if (uncommonItemSpawnTimeReductionUpgradeCount >= MAX_UNCOMMON_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_UNCOMMON_ITEM_SPAWN_TIME_REDUCTION_COST) // Check if player has enough coins
            return;

        uncommonItemSpawnTimeReductionUpgradeCount++;
        PlayerPrefs.SetInt(UncommonItemSpawnTimeReductionKey, uncommonItemSpawnTimeReductionUpgradeCount);
        PlayerPrefs.Save();

        OnSpawnTimeReductionUpgraded?.Invoke();
    }

    public void UpgradeGoldenItemSpawnTimeReduction()
    {
        if (goldenItemSpawnTimeReductionUpgradeCount >= MAX_GOLDEN_ITEM_SPAWN_TIME_REDUCTION_UPGRADE_COUNT) // Check if max upgrade count is reached
            return;

        if (CoinPouchHandler.Instance.GetCoinCount() < FIXED_GOLDEN_ITEM_SPAWN_TIME_REDUCTION_COST) // Check if player has enough coins
            return;

        goldenItemSpawnTimeReductionUpgradeCount++;
        PlayerPrefs.SetInt(GoldenItemSpawnTimeReductionKey, goldenItemSpawnTimeReductionUpgradeCount);
        PlayerPrefs.Save();

        OnSpawnTimeReductionUpgraded?.Invoke();
    }
}
