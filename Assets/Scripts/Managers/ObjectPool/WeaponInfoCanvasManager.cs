using TMPro;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class WeaponInfoCanvasManager : MonoBehaviour
    {
        [SerializeField] GameObject weaponDescriptionPanel;
        [SerializeField] TextMeshProUGUI weaponDescription;
        [HideInInspector] public bool isPaneActive = false;

        public void EvaluaPanelVisualization(string description)
        {
            if (!isPaneActive)
            {
                isPaneActive = true;
                CursorVisibility.ShowCursor();
                Time.timeScale = 0.0f;
                weaponDescriptionPanel.SetActive(true);
                UpdateWeaponDescription(description);
            }
            else 
            {
                isPaneActive = false;
                CursorVisibility.HideCursor();
                Time.timeScale = 1.0f;
                weaponDescriptionPanel.SetActive(false);
            }
        }

        public void OnBackButtonClick()
        {
            EvaluaPanelVisualization("");
        }

        void UpdateWeaponDescription(string description)
        {
            weaponDescription.text = description; 
        }
    }
}
