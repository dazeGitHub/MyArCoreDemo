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

            //设置中间 ImageView 的图像为 ContentImageSprite
            ContentImageView.overrideSprite = ContentImageSprite;

            //通知 ARCoreImageController
            //GameObject.Find("需要传递到游戏对象的Name").SendMessage(
            //      "脚本中的方法名称",
            //      Object 传递的参数类型,
            //      SendMessageOptions.DontRequireReceiver//这句代码表示不需要请求返回参数
            //）;
            GameObject.Find("ExampleController").SendMessage("AddImageBtnClick", 0, SendMessageOptions.DontRequireReceiver);
        }
    }
}