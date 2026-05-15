using UnityEngine;

public class MobileControlsManager : MonoBehaviour
{
    [SerializeField] private GameObject mobileControlsRoot;

    private void Awake()
    {
        if (mobileControlsRoot)
            mobileControlsRoot.SetActive(MobileBrowserDetector.IsMobile());
    }
}