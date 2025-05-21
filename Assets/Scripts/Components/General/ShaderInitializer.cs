using UnityEngine;

namespace Shaders
{
    public class ShaderInitializer : MonoBehaviour
    {
        [SerializeField] private Texture texture;
        [SerializeField] private Color colorMult;
        [SerializeField] private bool usesColorMult = false;
        private Renderer rend;

        void Awake()
        {
            rend = GetComponent<Renderer>();
            //Se crean copias de los materiales personalizadas para este objeto
            rend.materials[0] = new Material(rend.materials[0]);
            //Se asignan las propiedades deseadas
            if (texture != null) rend.materials[0].SetTexture("_Texture", texture);
            if (usesColorMult) rend.materials[0].SetColor("_Color", colorMult);
        }

        public void SetTexture(Texture texture)
        {
            this.texture = texture;
        }
    }
}
