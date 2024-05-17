using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Dawn
{
    public class LoadingForm: MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_DescriptionText = null;

        [SerializeField]
        private Slider m_ProgressSlider = null;

        private void Start()
        {
            
        }

        private void Update()
        {

        }

        public void SetProgress(float progress, string description)
        {
            m_ProgressSlider.value = progress;
            m_DescriptionText.text = description;
        }
    }
}
