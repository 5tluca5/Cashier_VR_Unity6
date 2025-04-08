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
    }

    [SerializeField] private UpgradeType upgradeType;
    #endregion

    [SerializeField] private TextMeshProUGUI upgradeLevelTMP;
    [SerializeField] private Button buyBtn;

    private void Start()
    {
        if (upgradeType == UpgradeType.LeftHandUpgrade)
        {
            upgradeLevelTMP.text = $"Lv. {UpgradeManager.Instance.GetLeftHandUpgradeCount()}";

            buyBtn.onClick.AddListener(() =>
            {
                UpgradeManager.Instance.UpgradeLeftHand();
                upgradeLevelTMP.text = $"Lv. {UpgradeManager.Instance.GetLeftHandUpgradeCount()}";
            });
        }
        else if (upgradeType == UpgradeType.RightHandUpgrade)
        {
            upgradeLevelTMP.text = $"Lv. {UpgradeManager.Instance.GetRightHandUpgradeCount()}";

            buyBtn.onClick.AddListener(() =>
            {
                UpgradeManager.Instance.UpgradeRightHand();
                upgradeLevelTMP.text = $"Lv. {UpgradeManager.Instance.GetRightHandUpgradeCount()}";
            });
        }
    }
}
