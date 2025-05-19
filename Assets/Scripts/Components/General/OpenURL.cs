using UnityEngine;

namespace General
{
    public class OpenURL : MonoBehaviour
    {
        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }
    }
}
