using UnityEngine;

namespace Automation
{
    public class CameraMovement : MonoBehaviour
    {
        private bool md;
        private float dist;
        private float screencoof;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            screencoof = (Screen.height / 1000f) * 12;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.touchCount >= 2)
                {
                    Vector2 touch1 = Input.GetTouch(0).position;
                    Vector2 touch2 = Input.GetTouch(1).position;

                    if (dist == 0) dist = Vector2.Distance(touch1, touch2);

                    float delta = Vector2.Distance(touch1, touch2) - dist;

                    ChangeSize(delta / 180);

                    dist = Vector2.Distance(touch1, touch2);
                    if (md) md = false;
                }
                else
                {
                    if (md)
                    {
                        float newx = transform.position.x - Input.GetAxis("Mouse X") / screencoof;
                        float newy = transform.position.y - Input.GetAxis("Mouse Y") / screencoof;
                        float sizemult = 3.8f * (Bootstrap.instance.field.transform.localScale.x - 0.5f);
                        if (newx > sizemult) newx = sizemult;
                        else if (newx < -sizemult) newx = -sizemult;
                        if (newy > sizemult) newy = sizemult;
                        else if (newy < -sizemult) newy = -sizemult;
                        transform.position = new Vector3(newx, newy, transform.position.z);
                    }
                    else md = true;
                    if (dist != 0) dist = 0;
                }
            }
            else
            {
                if (md) md = false;
                if (dist != 0) dist = 0;
            }
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0)
            {
                ChangeSize(scroll / 18);
            }
        }

        void ChangeSize(float change)
        {
            float newscale = Bootstrap.instance.field.transform.localScale.x + change / (Screen.height / 1500f);
            if (newscale > 8) newscale = 8;
            else if (newscale < 1) newscale = 1;
            float mult = newscale / Bootstrap.instance.field.transform.localScale.x;
            Bootstrap.instance.field.transform.localScale = new Vector3(newscale, newscale, 1);
            float newx = transform.position.x * mult;
            float newy = transform.position.y * mult;
            float sizemult = 3.8f * (Bootstrap.instance.field.transform.localScale.x - 0.5f);
            if (newx > sizemult) newx = sizemult;
            else if (newx < -sizemult) newx = -sizemult;
            if (newy > sizemult) newy = sizemult;
            else if (newy < -sizemult) newy = -sizemult;
            transform.position = new Vector3(newx, newy, transform.position.z);
        }
    }
}
