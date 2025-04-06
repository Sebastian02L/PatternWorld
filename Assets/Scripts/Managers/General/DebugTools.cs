using UnityEngine;

namespace Utils
{
    public class DebugTools : MonoBehaviour
    {
        [SerializeField] GameObject debugPanel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ToggleDebugPanel();
            }
        }

        void ToggleDebugPanel()
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
}
