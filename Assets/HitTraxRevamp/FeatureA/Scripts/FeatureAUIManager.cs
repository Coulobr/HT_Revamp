using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FeatureA
{
    public class FeatureAUIManager : MonoBehaviourSingleton<FeatureAUIManager>
    {
        public List<AsyncOperationHandle> addressableHandles = new List<AsyncOperationHandle>();
    }
}
