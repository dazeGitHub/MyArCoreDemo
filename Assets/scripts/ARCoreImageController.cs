namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    public class ARCoreImageController : MonoBehaviour
    {

        [Header("Drag Augmented Image prefab to this")]
        public List<AugmentedImageVisualizerOverride> AugmentedImageVisualizerPrefab;

        [Header("Drag FitToScanOverlay to this")]
        public GameObject FitToScanOverlay;

        private Dictionary<int, AugmentedImageVisualizerOverride> m_Visualizers = new Dictionary<int, AugmentedImageVisualizerOverride>();
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        public void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            Session.GetTrackables(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            foreach (var image in m_TempAugmentedImages)
            {
                Debug.Log("name======" + image.Name);
                Debug.Log("index======" + image.DatabaseIndex);
                AugmentedImageVisualizerOverride visualizer = null;
                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                {
                    Debug.Log("TrackName" + image.Name);
                    Debug.Log("TrackIndex" + image.DatabaseIndex);
                    Anchor anchor = image.CreateAnchor(image.CenterPose);

                    //visualizer = AugmentedImageVisualizerPrefab[image.DatabaseIndex];
                    //visualizer.gameObject.SetActive(true);
                    //visualizer.transform.parent = anchor.transform;
                    //visualizer.transform.localPosition = Vector3.zero;
                    //visualizer.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    //visualizer.transform.parent = null;

                    visualizer = (AugmentedImageVisualizerOverride)Instantiate(AugmentedImageVisualizerPrefab[image.DatabaseIndex], anchor.transform);
                    visualizer.Image = image;
                    m_Visualizers.Add(image.DatabaseIndex, visualizer);
                }
            }

            foreach (var visualizer in m_Visualizers.Values)
            {
                if (visualizer.Image.TrackingState == TrackingState.Tracking)
                {
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            }
        }

        //重置位置方法
        public void OnclickRe(int index)
        {
            AugmentedImageVisualizerOverride visualizer = null;
            m_Visualizers.TryGetValue(index, out visualizer);
            if (visualizer != null)
            {
                m_Visualizers.Remove(index);
                visualizer.gameObject.SetActive(false);
                FitToScanOverlay.SetActive(true);
            }
        }
    }
}