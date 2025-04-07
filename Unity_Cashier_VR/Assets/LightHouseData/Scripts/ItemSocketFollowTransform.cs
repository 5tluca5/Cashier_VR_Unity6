using UnityEngine;

public class ItemSocketFollowTransform : MonoBehaviour
{
    [SerializeField] private Transform HMD;
    [SerializeField] private bool followPosition = true;
    [SerializeField] private bool followRotation = true;
    [SerializeField] private BodySocket[] bodySockets;

    private Vector3 _currentHMDlocalPosition;
    private Quaternion _currentHMDRotation;
    void Update()
    {
        _currentHMDlocalPosition = HMD.localPosition;
        _currentHMDRotation = HMD.rotation;
        foreach (var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketInventory();
    }

    private void UpdateBodySocketHeight(BodySocket bodySocket)
    {
        bodySocket.transform.localPosition = new Vector3(bodySocket.transform.localPosition.x, (HMD.position.y * bodySocket.heightRatio), bodySocket.transform.localPosition.z);
    }

    private void UpdateSocketInventory()
    {
        if (followPosition)
            transform.localPosition = new Vector3(_currentHMDlocalPosition.x, 0, _currentHMDlocalPosition.z);
        if (followRotation)
            transform.rotation = new Quaternion(transform.rotation.x, _currentHMDRotation.y, transform.rotation.z, _currentHMDRotation.w);
    }
}

[System.Serializable]
public class BodySocket
{
    public Transform transform;
    [Range(0.01f, 1f)]
    public float heightRatio = 1f;
}
