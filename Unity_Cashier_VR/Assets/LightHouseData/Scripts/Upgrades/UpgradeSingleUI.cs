using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSingleUI : MonoBehaviour
{
    #region Dirty Code

    public enum UpgradeType
    {
        LeftHandUpgrade,
        RightHandUpgrade,
        UncommonItemSpawnTimeReductionUpgrade,
        GoldenItemSpawnTimeReductionUpgrade,
    }
    private const int DEFAULT_GOLDEN_ITEM_SPAWN_TIME = 60;
    private const int DEFAULT_UNCOMMON_ITEM_SPAWN_TIME = 30;

    [SerializeField] private UpgradeType upgradeType;
    #endregion

    [SerializeField] private TextMeshProUGUI upgradeLevelTMP;
    [SerializeField] private Button buyBtn;

    private void Start()
    {
        switch (upgradeType)
        {
            case UpgradeType.LeftHandUpgrade:
                HandleLeftHandUpgradesUI();
                buyBtn.onClick.AddListener(() =>
                {
                    UpgradeManager.Instance.UpgradeLeftHand();
                    HandleLeftHandUpgradesUI();
                });
                break;

            case UpgradeType.RightHandUpgrade:
                HandleRightHandUpgradesUI();
                buyBtn.onClick.AddListener(() =>
                {
                    UpgradeManager.Instance.UpgradeRightHand();
                    HandleRightHandUpgradesUI();
                });
                break;

            case UpgradeType.UncommonItemSpawnTimeReductionUpgrade:
                HandleUncommonItemSpawnTimeReductionUpgradeUI();
                buyBtn.onClick.AddListener(() =>
                {
                    UpgradeManager.Instance.UpgradeUncommonItemSpawnTimeReduction();
                    HandleUncommonItemSpawnTimeReductionUpgradeUI();
                });
                break;

            case UpgradeType.GoldenItemSpawnTimeReductionUpgrade:
                HandleGoldenItemSpawnTimeReductionUpgradeUI();
                buyBtn.onClick.AddListener(() =>
                {
                    UpgradeManager.Instance.UpgradeGoldenItemSpawnTimeReduction();
                    HandleGoldenItemSpawnTimeReductionUpgradeUI();
                });
                break;
        }
    }

    private void HandleRightHandUpgradesUI()
    {
        bool isRightHandUpgradeMaxed = UpgradeManager.Instance.IsRightHandUpgradeMaxed;
        upgradeLevelTMP.text = isRightHandUpgradeMaxed ? "MAXED" : $"Lv. {UpgradeManager.Instance.GetRightHandUpgradeCount()}";
        buyBtn.gameObject.SetActive(!UpgradeManager.Instance.IsRightHandUpgradeMaxed); // Disable button if maxed
    }

    private void HandleLeftHandUpgradesUI()
    {
        bool isLeftHandUpgradeMaxed = UpgradeManager.Instance.IsLeftHandUpgradeMaxed;
        upgradeLevelTMP.text = isLeftHandUpgradeMaxed ? "MAXED" : $"Lv. {UpgradeManager.Instance.GetLeftHandUpgradeCount()}";
        buyBtn.gameObject.SetActive(!UpgradeManager.Instance.IsLeftHandUpgradeMaxed); // Disable button if maxed
    }

    private void HandleUncommonItemSpawnTimeReductionUpgradeUI()
    {
        int uncommonItemSpawnTime = DEFAULT_UNCOMMON_ITEM_SPAWN_TIME - UpgradeManager.Instance.GetUncommonItemSpawnTimeReductionInSec();
        bool isUncommonItemSpawnTimeReductionMaxed = UpgradeManager.Instance.IsUncommonItemSpawnTimeReductionMaxed;
        upgradeLevelTMP.text = $"{uncommonItemSpawnTime}s" + (isUncommonItemSpawnTimeReductionMaxed ? "(MAX)" : string.Empty);
        buyBtn.gameObject.SetActive(!isUncommonItemSpawnTimeReductionMaxed); // Disable button if maxed
    }

    private void HandleGoldenItemSpawnTimeReductionUpgradeUI()
    {
        int goldenItemSpawnTime = DEFAULT_GOLDEN_ITEM_SPAWN_TIME - UpgradeManager.Instance.GetGoldenItemSpawnTimeReductionInSec();
        bool isGoldenItemSpawnTimeReductionMaxed = UpgradeManager.Instance.IsGoldenItemSpawnTimeReductionMaxed;
        upgradeLevelTMP.text = $"{goldenItemSpawnTime}s" + (isGoldenItemSpawnTimeReductionMaxed ? "(MAX)" : string.Empty);
        buyBtn.gameObject.SetActive(!isGoldenItemSpawnTimeReductionMaxed); // Disable button if maxed
    }
}
