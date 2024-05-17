using UnityEngine;
using UnityEngine.UI;
namespace Dawn
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyRaycast : MaskableGraphic
    {
        protected EmptyRaycast(){
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper fill)
        {
            fill.Clear();
        }

        public void BeginDrag(){
            Debug.Log("111111111111111111111");
        }
    }
}
