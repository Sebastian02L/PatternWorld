using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class ImageCarruselManager : MonoBehaviour
    {
        Image currentImage;
        [SerializeField] List<Sprite> minigameImages = new List<Sprite>();
        [SerializeField] float imageShowTime = 2.5f;
        int imageIndex = 0;
        float timer = 0;

        private void Start()
        {
            currentImage = GetComponent<Image>();
            currentImage.sprite = minigameImages[imageIndex];
        }
        void Update()
        {
            if (gameObject.activeSelf)
            {
                timer += Time.deltaTime;

                if (timer > imageShowTime)
                {
                    NextIndex();
                    currentImage.sprite = minigameImages[imageIndex];
                    timer = 0;
                }
            }
        }

        void NextIndex()
        {
            imageIndex = (imageIndex + 1 < minigameImages.Count) ? imageIndex + 1 : 0;
        }
    }
}
