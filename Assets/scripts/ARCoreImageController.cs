namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;

    public class ARCoreImageController : MonoBehaviour
    {
        //模型 prefab 集合
        [Header("Drag Augmented Image prefab to this")]
        public List<AugmentedImageVisualizerOverride> AugmentedImageVisualizerPrefab;

        //图片 prefab 集合
        [Header("Drag ImageView prefab to this")]
        public List<ARcoreImagePrefabVisualizerOverride> ImageViewContentVisualizerPrefab;

        //中间的 ScanView 
        [Header("Drag FitToScanOverlay to this")]
        public GameObject FitToScanOverlay;

        //key 为识别的图像的索引，Value 为所识别的图像添加的增强图像 prefab
        private Dictionary<int, AugmentedImageVisualizerOverride> m_Visualizers = new Dictionary<int, AugmentedImageVisualizerOverride>();

        //key 是识别出来的图像索引(第几个图像)，value 是给该图像添加的锚点节点
        private Dictionary<int, Anchor> m_anchorDict = new Dictionary<int, Anchor>();

        /// <summary>
        /// The rotation in degrees need to apply to prefab when it is placed.
        /// </summary>
        private const float _prefabRotation = 180.0f;

        private bool m_isDetectedImgShowToast = false;

        //存放检测到的增强图像
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
                AugmentedImageVisualizerOverride tempAugmentedImageVisualizer = null;
                m_Visualizers.TryGetValue(image.DatabaseIndex, out tempAugmentedImageVisualizer);

                if (image.TrackingState == TrackingState.Tracking && tempAugmentedImageVisualizer == null)
                {
                    Debug.Log("TrackName" + image.Name);
                    Debug.Log("TrackIndex" + image.DatabaseIndex);
                    if (!m_isDetectedImgShowToast)
                    {
                        ToastUtil.showToast("ARCoreImageController Update() 检测到图像成功 !");
                        m_isDetectedImgShowToast = true;
                    }

                    Anchor anchor = image.CreateAnchor(image.CenterPose);

                    //将锚点放到集合里
                    if (!m_anchorDict.ContainsKey(image.DatabaseIndex))
                    {
                        m_anchorDict.Add(image.DatabaseIndex, anchor);
                    }

                    //visualizer = AugmentedImageVisualizerPrefab[image.DatabaseIndex];
                    //visualizer.gameObject.SetActive(true);
                    //visualizer.transform.parent = anchor.transform;
                    //visualizer.transform.localPosition = Vector3.zero;
                    //visualizer.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    //visualizer.transform.parent = null;

                    //识别出来就添加了一个图片，这里先注释了改为手动添加
                    //使用 AugmentedImageVisualizerPrefab 通过 image.DatabaseIndex 初始化一个 AugmentedImageVisualizerOverride 对象，并设置锚点的 transform
                    //tempAugmentedImageVisualizer = (AugmentedImageVisualizerOverride)Instantiate(AugmentedImageVisualizerPrefab[image.DatabaseIndex], anchor.transform);

                    ////为该 AugmentedImageVisualizerOverride 对象设置识别到的 AugmentedImage (增强图像) 对象
                    //tempAugmentedImageVisualizer.Image = image;

                    //m_Visualizers.Add(image.DatabaseIndex, tempAugmentedImageVisualizer);
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

        private Anchor getAnchor()
        {
            //默认取第 1 个的锚点
            var index = 0;
            if (m_anchorDict.Count != 0)
            {
                return m_anchorDict[index];
            }
            return null;
        }

        private void addSimpleImage()
        {
            //每次只取第 0 个图

            //这个不好使
            //tempAugmentedImageVisualizer = (ARcoreImagePrefabVisualizerOverride)Instantiate(ImageViewContentVisualizerPrefab[0], anchor.transform);
            //var gameObject = Instantiate(ImageViewContentVisualizerPrefab[0], hit.Pose.position, hit.Pose.rotation);

            var targetAnchor = getAnchor();

            if (targetAnchor != null)
            {
                ToastUtil.showToast("addSimpleImage targetAnchor is not null , ImageViewContentVisualizerPrefab.Count= " + ImageViewContentVisualizerPrefab.Count);
                var position = new Vector3(0.0f, 0f, 0.0f);
                var rotation = new Quaternion(1f, 0f, 0f, -90f);
                var gameObject = Instantiate(ImageViewContentVisualizerPrefab[0], position, rotation); //ImageViewContentVisualizerPrefab  AugmentedImageVisualizerPrefab

                gameObject.transform.Rotate(0, _prefabRotation, 0, Space.Self);
                //var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                gameObject.transform.parent = targetAnchor.transform;
            }
            else
            {
                Debug.Log("addSimpleImage targetAnchor is null");
                ToastUtil.showToast("addSimpleImage targetAnchor is null");
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

        public void AddImageBtnClick()
        {
            ToastUtil.showToast("ARCoreImageController AddImageBtnClick() invoked");
            addSimpleImage();
        }
    }
}