namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    public class AugmentedImageVisualizerOverride : MonoBehaviour
    {

        public AugmentedImage Image;

        public void Update()
        {

            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                return;
            }
        }
    }
}