using System;
using UnityEngine;

namespace FantomLib
{
    /// <summary>
    /// SmoothFollow added right/left rotation angle, height and distance, 
    /// and added a corresponding to pinch (PinchInput) and swipe (SwipeInput) (originally StandardAssets)
    /// http://fantom1x.blog130.fc2.com/blog-entry-289.html
    /// </summary>
    public class SmoothFollow3 : MonoBehaviour
    {
        public Transform target;                    //Object to follow

        public bool autoInitOnPlay = true;          //Automatically calculates distance, height, preAngle from target position at startup
        public float distance = 2.0f;               //Distance of XZ plane
        public float height = 0f;                   //Y axis height
        public float preAngle = 0f;                 //Initial value of camera angle

        public bool widthReference = true;          //Make the screen width (Screen.width) size the standard of the ratio (false: based on height (Screen.height))

        //Area on screen to recognize: 0.0~1.0 [(0,0):Bottom left of screen, (1,1):Upper right of screen]
        public Rect validArea = new Rect(0, 0, 1, 1);


        //Rotation operation
        [Serializable]
        public class AngleOperation
        {
            public float damping = 3.0f;            //Smooth moving speed of left and right rotation

            //Key input
            public bool keyEnable = true;           //ON/OFF of rotation key operation
            public float keySpeed = 45f;            //Speed by key operation
            public KeyCode keyLeft = KeyCode.Z;     //Left rotation key
            public KeyCode keyRight = KeyCode.X;    //Right rotation key

            //Drag
            public bool dragEnable = true;          //ON/OFF of rotation drag operation
            public float dragSpeed = 10f;           //Speed by drag operation
            public float dragWidthLimit = 0.1f;     //Limit width that can be recognized as a drag (0: unlimited ~ 1: Screen.width [when widthReference=true]). Not recognize more than this width (to distinguish it from swipe).
        }
        public AngleOperation angleOperation;


        //Turn operation (constant angle rotation)
        [Serializable]
        public class TurnOperation
        {
            public float angle = 90f;                       //Angle of turn

            //Key input
            public bool keyEnable = true;                   //ON/OFF of rotation key operation
            public KeyCode keyLeft = KeyCode.KeypadMinus;   //Left rotation key
            public KeyCode keyRight = KeyCode.KeypadPlus;   //Right rotation key

            //Swipe
            public bool swipeEnable = true;                 //ON/OFF of rotation swipe operation
        }
        public TurnOperation turnOperation;


        //Height operation
        [Serializable]
        public class HeightOperation
        {
            public float damping = 2.0f;            //Smooth moving speed of height

            //Key input
            public bool keyEnable = true;           //ON/OFF of height key operation
            public float keySpeed = 1.5f;           //Speed by key operation
            public KeyCode keyUp = KeyCode.C;       //Key height up
            public KeyCode keyDown = KeyCode.V;     //Keys height down

            //Drag
            public bool dragEnable = true;          //ON/OFF of height drag operation
            public float dragSpeed = 0.5f;          //Speed by drag operation
        }
        public HeightOperation heightOperation;


        //Distance operation
        [Serializable]
        public class DistanceOperation
        {
            public float damping = 1.0f;            //Smooth moving speed of distance
            public float min = 1.0f;                //Minimum distance on XZ plane

            //Key input
            public bool keyEnable = true;           //ON/OFF of distance key operation
            public float keySpeed = 0.5f;           //Speed by key operation
            public KeyCode keyNear = KeyCode.B;     //Key distance near
            public KeyCode keyFar = KeyCode.N;      //Key distance far

            //Wheel
            public bool wheelEnable = true;         //ON/OFF of distance wheel operation
            public float wheelSpeed = 7f;           //Speed by wheel operation

            //Pinch
            public bool pinchEnable = true;         //ON/OFF of distance pinch operation
            public float pinchDamping = 5f;         //Smooth moving speed of distance at pinch
            public float pinchSpeed = 40f;          //Speed by pinch operation
        }
        public DistanceOperation distanceOperation;


        //Initial reset operation
        [Serializable]
        public class ResetOperation
        {
            public bool keyEnable = true;               //ON/OFF of reset key operation
            public KeyCode key = KeyCode.KeypadPeriod;  //Key reset
        }
        public ResetOperation resetOperation;


        //Local Values
        float angle;                                //Camera angle (XZ plane)
        Vector3 startPos;                           //Mouse movement start point
        float wantedDistance;                       //Destination distance
        float resetDistance;                        //For initial distance
        float resetHeight;                          //For initial height
        bool pinched = false;                       //Flag operated with pinch (switch between distanceDamping and pinchDistanceDamping)
        bool dragging = false;                      //Drag operation flag


        // Use this for initialization
        void Start()
        {
            if (autoInitOnPlay && target != null)
            {
                height = transform.position.y - target.position.y;
                Vector3 dir = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
                distance = dir.magnitude;
                preAngle = AngleXZWithSign(target.forward, dir);
            }

            angle = preAngle;
            resetDistance = wantedDistance = distance;
            resetHeight = height;
        }

        // Update is called once per frame
        void Update()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)   //Only platforms you want to obtain with touch
            if (Input.touchCount != 1 || Input.touches[0].fingerId != 0)
            {
                dragging = false;
                return;
            }
#endif

            //Rotation operation
            if (angleOperation.keyEnable)
            {
                if (Input.GetKey(angleOperation.keyLeft))
                    angle = Mathf.Repeat(angle + angleOperation.keySpeed * Time.deltaTime, 360f);

                if (Input.GetKey(angleOperation.keyRight))
                    angle = Mathf.Repeat(angle - angleOperation.keySpeed * Time.deltaTime, 360f);
            }

            //Turn operation (constant angle rotation)
            if (turnOperation.keyEnable)
            {
                if (Input.GetKeyDown(turnOperation.keyLeft))
                    TurnLeft();

                if (Input.GetKeyDown(turnOperation.keyRight))
                    TurnRight();
            }

            //Height operation
            if (heightOperation.keyEnable)
            {
                if (Input.GetKey(heightOperation.keyUp))
                    height += heightOperation.keySpeed * Time.deltaTime;

                if (Input.GetKey(heightOperation.keyDown))
                    height -= heightOperation.keySpeed * Time.deltaTime;
            }

            //Rotation or height operation by drag
            if (angleOperation.dragEnable || heightOperation.dragEnable)
            {
                Vector3 movePos = Vector3.zero;

                if (!dragging && Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    if (validArea.xMin * Screen.width <= startPos.x && startPos.x <= validArea.xMax * Screen.width &&
                        validArea.yMin * Screen.height <= startPos.y && startPos.y <= validArea.yMax * Screen.height)
                    {
                        dragging = true;
                    }
                }
                else if (dragging)
                {
                    if (Input.GetMouseButton(0))
                    {
                        movePos = Input.mousePosition - startPos;
                        startPos = Input.mousePosition;

                        //Restrict by drag width (to separate from swipe)
                        if (angleOperation.dragWidthLimit > 0)
                        {
                            float limit = (widthReference ? Screen.width : Screen.height) * angleOperation.dragWidthLimit;
                            float d = Mathf.Max(Mathf.Abs(movePos.x), Mathf.Abs(movePos.y));
                            if (d > limit)
                            {
                                movePos = Vector3.zero;     //to disable drag
                                dragging = false;
                            }
                        }
                    }
                    else //Input.GetMouseButtonUp(0), exit
                    {
                        dragging = false;
                    }
                }

                if (movePos != Vector3.zero)
                {
                    //Rotation drag operation
                    if (angleOperation.dragEnable)
                        angle = Mathf.Repeat(angle + movePos.x * angleOperation.dragSpeed * Time.deltaTime, 360f);

                    //Heigh drag operation
                    if (heightOperation.dragEnable)
                        height -= movePos.y * heightOperation.dragSpeed * Time.deltaTime;
                }
            }

            //Distance operation
            if (distanceOperation.keyEnable)
            {
                if (Input.GetKey(distanceOperation.keyNear))
                {
                    wantedDistance = Mathf.Max(distanceOperation.min, distance - distanceOperation.keySpeed);
                    pinched = false;
                }

                if (Input.GetKey(distanceOperation.keyFar))
                {
                    wantedDistance = distance + distanceOperation.keySpeed;
                    pinched = false;
                }
            }

            //Distance operation by wheel
            if (distanceOperation.wheelEnable)
            {
                float mw = Input.GetAxis("Mouse ScrollWheel");
                if (mw != 0)
                {
                    wantedDistance = Mathf.Max(distanceOperation.min, distance - mw * distanceOperation.wheelSpeed); //0.1 x n times
                    pinched = false;
                }
            }

            //Initial reset operation
            if (resetOperation.keyEnable)
            {
                if (Input.GetKeyDown(resetOperation.key))
                    ResetOperations();
            }
        }

        void LateUpdate()
        {
            if (target == null)
                return;

            float wantedRotationAngle = target.eulerAngles.y + angle;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle,
                angleOperation.damping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightOperation.damping * Time.deltaTime);
            distance = Mathf.Lerp(distance, wantedDistance,
                (pinched ? distanceOperation.pinchDamping : distanceOperation.damping) * Time.deltaTime);

            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
            Vector3 pos = target.position - currentRotation * Vector3.forward * distance;
            pos.y = currentHeight;
            transform.position = pos;

            transform.LookAt(target);
        }


        //Initial reset
        public void ResetOperations()
        {
            height = resetHeight;
            distance = wantedDistance = resetDistance;
            angle = preAngle;
        }


        //Pinch to operate distance (for mobile)
        //(*) use PinchInput callback
        //http://fantom1x.blog130.fc2.com/blog-entry-288.html
        //width: distance of two fingers of pinch
        //delta: The difference in pinch width just before
        //ratio: Stretch ratio from the start of pinch width (1:At the start of pinch, Expand by 1 or more, lower than 1 (1/2, 1/3, ...)
        public void OnPinch(float width, float delta, float ratio)
        {
            if (!distanceOperation.pinchEnable)
                return;

            if (delta != 0)
            {
                wantedDistance = Mathf.Max(distanceOperation.min, distance - delta * distanceOperation.pinchSpeed);
                pinched = true;
            }
        }

        //Swipe to operate turn
        //(*) use SwipeInput callback
        //http://fantom1x.blog130.fc2.com/blog-entry-250.html
        public void OnSwipe(Vector2 dir)
        {
            if (!turnOperation.swipeEnable)
                return;

            if (dir == Vector2.left)
                TurnLeft();
            else if (dir == Vector2.right)
                TurnRight();
        }


        //Turn left operation (constant angle rotation)
        public void TurnLeft()
        {
            angle = Mathf.Repeat(MultipleCeil(angle - turnOperation.angle, turnOperation.angle), 360f);
        }

        //Turn right operation (constant angle rotation)
        public void TurnRight()
        {
            angle = Mathf.Repeat(MultipleFloor(angle + turnOperation.angle, turnOperation.angle), 360f);
        }


        //From here, static method

        //Calculate a smaller multiple
        //http://fantom1x.blog130.fc2.com/blog-entry-248.html
        static float MultipleFloor(float value, float multiple)
        {
            return Mathf.Floor(value / multiple) * multiple;
        }

        //Calculate a larger multiple
        static float MultipleCeil(float value, float multiple)
        {
            return Mathf.Ceil(value / multiple) * multiple;
        }

        //Angle between direction vectors in 2D (XY plane) with sign (degrees)
        //http://fantom1x.blog130.fc2.com/blog-entry-253.html#AngleWithSign
        static float AngleXZWithSign(Vector3 from, Vector3 to)
        {
            Vector3 projFrom = from;
            Vector3 projTo = to;
            projFrom.y = projTo.y = 0;
            float angle = Vector3.Angle(projFrom, projTo);
            float cross = CrossXZ(projFrom, projTo);
            return (cross != 0) ? angle * -Mathf.Sign(cross) : angle;
        }

        //Outer product in 2D (XY plane)
        //http://fantom1x.blog130.fc2.com/blog-entry-253.html#Cross2D
        static float CrossXZ(Vector3 a, Vector3 b)
        {
            return a.x * b.z - a.z * b.x;
        }
    }
}