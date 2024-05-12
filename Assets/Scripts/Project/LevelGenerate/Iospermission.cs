using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
//using UnityEngine.iOS;
#endif 
public class Iospermission : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Setup());
    }
    
    IEnumerator Setup()
    {
#if UNITY_IOS
        yield return new WaitForSeconds(1f);
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus()== ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED ||
            ATTrackingStatusBinding.GetAuthorizationTrackingStatus()== ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED ||
            ATTrackingStatusBinding.GetAuthorizationTrackingStatus()==ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED)
        {
            yield return new WaitForSeconds(1f);
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
        yield return null;
    }
}
