using TMPro;
using UnityEngine;

namespace Utils
{
    public class ShowFPS : MonoBehaviour
    {
        public TextMeshProUGUI fpsText1;
        public TextMeshProUGUI fpsText2;
        private float deltaTime = 0.0f;

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText1.text = Mathf.Ceil(fps).ToString() + " FPS";
            fpsText2.text = Mathf.Ceil(fps).ToString() + " FPS";
        }
    }
}