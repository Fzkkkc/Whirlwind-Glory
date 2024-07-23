using UnityEngine;

namespace GameCore
{
    public class SpotForObject : MonoBehaviour
    {
        public bool HasChildren => transform.childCount > 0;
        
        public string GetChildName()
        {
            if (HasChildren)
            {
                return transform.GetChild(0).name;
            }
            return null;
        }
    }
}