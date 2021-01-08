namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class AddImageBtnClick : MonoBehaviour
    {
        //设置为 public 才能拖动赋值
        public Text MsgTextView;

        public Image ContentImageView;

        public Sprite ContentImageSprite;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClickCallBack);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClickCallBack()
        {
            var text = "BtnOnClickTest OnClick AddImage";
            Debug.Log(text);
            ToastUtil.showToast(text);
            MsgTextView.text = text;
            ContentImageView.overrideSprite = ContentImageSprite;
        }
    }
}