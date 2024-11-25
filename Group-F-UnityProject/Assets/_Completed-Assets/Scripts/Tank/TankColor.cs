using UnityEngine;

namespace Complete
{
    public class TankColor : MonoBehaviour
    {
        [SerializeField]
        private GameObject icon;

        public void ChangeColor(Color color)
        {
            foreach (var renderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material.color = color;
            }
            icon.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
