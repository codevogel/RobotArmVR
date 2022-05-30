using UnityEngine;
using UnityEngine.UI;
using MathNet.Numerics.LinearAlgebra.Single;
using System.Collections.Generic;

namespace IKManager
{
    public class CustomIKManager : MonoBehaviour
    {
        // robot
        //public GameObject[] Joint = new GameObject[6];      // joints of arm robot
        public ArticulationBody[] Joint = new ArticulationBody[6];
        private float[] angle = new float[6];               // local rotation around its axis
        private float[] prevAngle = new float[6];
        private Vector3[] dim = new Vector3[7];             // local dimensions of each joint
        private Vector3[] point = new Vector3[7];           // world position of joint end
        private Vector3[] axis = new Vector3[6];            // local direction of each axis
        private Quaternion[] rotation = new Quaternion[6];  // local rotation(quaternion) of joint relative to its parent
        private Quaternion[] wRotation = new Quaternion[6]; // world rotation(quaternion) of joint
        public static Vector3 PRef;                                // reference(target) position
        public static Vector3 RRef;                                // reference(target) pose

        private float lambda = 0.1f;                        // convergence rate

        private float[] minAngle = new float[6];            // limits of joint rotatation
        private float[] maxAngle = new float[6];

        public List<Transform> transforms;

        [HideInInspector]
        public bool movementEnabled;

        void Start()
        {
            minAngle[0] = -180;
            maxAngle[0] = 180;
            minAngle[1] = -180;
            maxAngle[1] = 180;
            minAngle[2] = -180;
            maxAngle[2] = 180;
            minAngle[3] = -180;
            maxAngle[3] = 180;
            minAngle[4] = -180;
            maxAngle[4] = 180;
            minAngle[5] = -180;
            maxAngle[5] = 180;

            // local dimensions of each joint at initial pose
            // These numbers are taken from transform values in inspector window.
            dim[0] = transforms[0].localPosition;
            dim[1] = transforms[1].localPosition;
            dim[2] = transforms[2].localPosition;
            dim[3] = transforms[3].localPosition;
            dim[4] = transforms[4].localPosition;
            dim[5] = transforms[5].localPosition;

            // local direction of each axis
            axis[0] = new Vector3(0f, 1f, 0f);
            axis[1] = new Vector3(0f, 0f, 1f);
            axis[2] = new Vector3(0f, 0f, 1f);
            axis[3] = new Vector3(1f, 0f, 0f);
            axis[4] = new Vector3(0f, 0f, 1f);
            axis[5] = new Vector3(1f, 0f, 0f);

            // local initial rotation around axis
            angle[0] = prevAngle[0] = 0f;
            angle[1] = prevAngle[1] = 30f;
            angle[2] = prevAngle[2] = -60f;
            angle[3] = prevAngle[3] = 30f;
            angle[4] = prevAngle[4] = 0f;
            angle[5] = prevAngle[5] = 0f;
            ForwardKinematics();
            for (int i = 0; i < 6; i++)
            {
                var drive = Joint[i].xDrive;
                drive.target = angle[i];
                Joint[i].xDrive = drive;
            }
        }

        private void FixedUpdate()
        {
            // IK
            if (movementEnabled)
            {
                CalcIK();
            }
        }

        void CalcIK()
        {
            int count = 0;
            bool outOfLimit = false;
            for (int i = 0; i < 6; i++)
            {
                prevAngle[i] = angle[i];
            }
            //Debug.Log("angle= " + angle[0] + " " + angle[1] + " " + angle[2] + " " + angle[3] + "_" + angle[4] + "_" + angle[5]);
            
            for (int i = 0; i < 25; i++)   // iteration
            {
                count = i;
                //Debug.Log("iter_count=" + count);
                // find position/pose of hand with current angles
                ForwardKinematics();

                // create jacobian with current angles
                var J = CalcJacobian(); // 6x6 matrix

                // calculate position/pose error from reference
                var err = CalcErr();    // 6x1 matrix(vector)
                float err_norm = (float)err.L2Norm();
                if (err_norm < 1E-3)    // close enough
                {
                    for (int ii = 0; ii < 6; ii++)
                    {
                        if (angle[ii] < minAngle[ii] || angle[ii] > maxAngle[ii])
                        {
                            outOfLimit = true;
                            break;
                        }
                    }
                    break;
                }

                // correct angle of joionts
                var dAngle = lambda * J.PseudoInverse() * err; // 6x1 matrix
                for (int ii = 0; ii < 6; ii++)
                {
                    angle[ii] += dAngle[ii, 0] * Mathf.Rad2Deg;
                }
                if (outOfLimit) break;
            }

            if (count == 99 || outOfLimit)  // did not converge or angle out of limit
            {
                Debug.Log("did not converge or out of limit");
            }
            else // draw new robot
            {
                for (int i = 0; i < 6; i++)
                {
                    var drive = Joint[i].xDrive;
                    drive.target = angle[i];
                    Joint[i].xDrive = drive;
                }
            }
        }

        public void UpdateUI()
        {
            FlexpendantUIManager.Instance.ChangeFlangePosition(Joint[5].transform);
        }

        void ForwardKinematics()
        {
            point[0] = dim[0];
            wRotation[0] = Quaternion.AngleAxis(angle[0], axis[0]);
            for (int i = 1; i < Joint.Length; i++)
            {
                point[i] = wRotation[i - 1] * dim[i] + point[i - 1];
                rotation[i] = Quaternion.AngleAxis(angle[i], axis[i]);
                wRotation[i] = wRotation[i - 1] * rotation[i];
            }
            point[Joint.Length] = wRotation[Joint.Length - 1] * dim[Joint.Length] + point[Joint.Length - 1];
            //Debug.Log("end= " + point[6].x + " " + point[6].y + " " + point[6].z + " " + wRotation[5].eulerAngles.x + " " + wRotation[5].eulerAngles.y + " " + wRotation[5].eulerAngles.z);
        }

        DenseMatrix CalcJacobian()
        {
            Vector3 w0 = wRotation[0] * axis[0];
            Vector3 w1 = wRotation[1] * axis[1];
            Vector3 w2 = wRotation[2] * axis[2];
            Vector3 w3 = wRotation[3] * axis[3];
            Vector3 w4 = wRotation[4] * axis[4];
            Vector3 w5 = wRotation[5] * axis[5];
            Vector3 p0 = Vector3.Cross(w0, point[6] - point[0]);
            Vector3 p1 = Vector3.Cross(w1, point[6] - point[1]);
            Vector3 p2 = Vector3.Cross(w2, point[6] - point[2]);
            Vector3 p3 = Vector3.Cross(w3, point[6] - point[3]);
            Vector3 p4 = Vector3.Cross(w4, point[6] - point[4]);
            Vector3 p5 = Vector3.Cross(w5, point[6] - point[5]);

            var J = DenseMatrix.OfArray(new float[,]
            {
                { p0.x, p1.x, p2.x, p3.x, p4.x, p5.x },
                { p0.y, p1.y, p2.y, p3.y, p4.y, p5.y },
                { p0.z, p1.z, p2.z, p3.z, p4.z, p5.z },
                { w0.x, w1.x, w2.x, w3.x, w4.x, w5.x  },
                { w0.y, w1.y, w2.y, w3.y, w4.y, w5.y  },
                { w0.z, w1.z, w2.z, w3.z, w4.z, w5.z  }
            });
            return J;
        }

        DenseMatrix CalcErr()
        {
            // position error
            Vector3 perr = PRef - point[6];
            // pose error
            Quaternion rerr = Quaternion.Euler(RRef) * Quaternion.Inverse(wRotation[5]);
            // make error vector
            Vector3 rerrVal = new Vector3(rerr.eulerAngles.x, rerr.eulerAngles.y, rerr.eulerAngles.z);
            if (rerrVal.x > 180f) rerrVal.x -= 360f;
            if (rerrVal.y > 180f) rerrVal.y -= 360f;
            if (rerrVal.z > 180f) rerrVal.z -= 360f;
            var err = DenseMatrix.OfArray(new float[,]
            {
                { perr.x },
                { perr.y },
                { perr.z },
                { rerrVal.x * Mathf.Deg2Rad},
                { rerrVal.y * Mathf.Deg2Rad},
                { rerrVal.z * Mathf.Deg2Rad}
            });
            //Debug.Log("err= " + perr.x + " " + perr.y + " " + perr.z + " " + rerrVal.x + "_" + rerrVal.y + "_" + rerrVal.z);
            return err;
        }
    }
}



