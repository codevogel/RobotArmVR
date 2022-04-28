using UnityEngine;
using System.Collections;

namespace CameraScript
{
    enum MouseButtonDown
    {
        MBD_LEFT = 0,
        MBD_RIGHT,
        MBD_MIDDLE,
    };

    public class CameraScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject focusObj = null;

        private Vector3 oldPos;

        void setupFocusObject(string name)
        {
            GameObject obj = this.focusObj = new GameObject(name);
            obj.transform.position = Vector3.zero;

            return;
        }

        void Start()
        {
            if (this.focusObj == null)
                this.setupFocusObject("CameraFocusObject");

            Transform trans = this.transform;
            transform.parent = this.focusObj.transform;

            trans.LookAt(this.focusObj.transform.position);

            return;
        }

        void Update()
        {
            this.mouseEvent();

            return;
        }

        void mouseEvent()
        {
            float delta = Input.GetAxis("Mouse ScrollWheel");
            if (delta != 0.0f)
                this.mouseWheelEvent(delta);

            if (Input.GetMouseButtonDown((int)MouseButtonDown.MBD_LEFT) ||
            Input.GetMouseButtonDown((int)MouseButtonDown.MBD_MIDDLE) ||
            Input.GetMouseButtonDown((int)MouseButtonDown.MBD_RIGHT))
                this.oldPos = Input.mousePosition;

            this.mouseDragEvent(Input.mousePosition);

            return;
        }

        void mouseWheelEvent(float delta)
        {
            Vector3 focusToPosition = this.transform.position - this.focusObj.transform.position;

            Vector3 post = focusToPosition * (1.0f + delta);

            if (post.magnitude > 0.01f)
                this.transform.position = this.focusObj.transform.position + post;

            return;
        }

        void mouseDragEvent(Vector3 mousePos)
        {
            Vector3 diff = mousePos - oldPos;

            if (diff.magnitude < Vector3.kEpsilon)
                return;

            if (Input.GetMouseButton((int)MouseButtonDown.MBD_LEFT))
            {
            }
            else if (Input.GetMouseButton((int)MouseButtonDown.MBD_MIDDLE))
            {
                this.cameraTranslate(-diff / 10.0f);

            }
            else if (Input.GetMouseButton((int)MouseButtonDown.MBD_RIGHT))
            {
                this.cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
            }

            this.oldPos = mousePos;

            return;
        }

        void cameraTranslate(Vector3 vec)
        {
            Transform focusTrans = this.focusObj.transform;
            Transform trans = this.transform;

            focusTrans.Translate((trans.right * vec.x) + (trans.up * vec.y));

            return;
        }

        public void cameraRotate(Vector3 eulerAngle)
        {
            Vector3 focusPos = this.focusObj.transform.position;
            Transform trans = this.transform;

            Vector3 preUpV, preAngle, prePos;
            preUpV = trans.up;
            preAngle = trans.localEulerAngles;
            prePos = trans.position;

            trans.RotateAround(focusPos, Vector3.up, eulerAngle.y);
            trans.RotateAround(focusPos, trans.right, -eulerAngle.x);

            trans.LookAt(focusPos);

            Vector3 up = trans.up;
            if (Vector3.Angle(preUpV, up) > 90.0f)
            {
                trans.localEulerAngles = preAngle;
                trans.position = prePos;
            }

            return;
        }
    }
}