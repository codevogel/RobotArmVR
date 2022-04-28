Robotic Arm with Inverse Kinematics / Ver 2.0

===
Created by Toshiyuki Nakamura(Meuse Robotics Inc)

Robotic Arm with Inverse Kinematics is a 6 axis robotic arm simulator with inverse kinematics.

Usage is pretty simple, just move sliders to determine the position and rotation of the end effector.

==== NumericIK ====

Assets > NumericIK > NumericIK

This scene uses toy arm robot model that you can easily modify.
It calculates the joint angles by numeric iteration using inverse Jacobian.

-- Scripts --
- IKManager : IKManager calculates the joint angles of the robot using slider values in real time. This unity project uses the numerical approach to calculate the inverse kinematics.
- CameraScript : CameraScript controls the view.

* Please don't delete the small cube in the scene. It's the target of the camara.


==== ArmRobotIK project ====

Assets > Scenes > ArticulationRobot

This project is similar to NumericIK project exept for the robot model (UR3).
The robot model is from ArticulationRobot demo project from Unity.

-- Scripts --
- IKManager : IKManager calculates the joint angles of the robot using slider values in real time. This unity project uses the numerical approach to calculate the inverse kinematics.
- CameraScript : CameraScript controls the view.

* Please don't delete the small cube in the scene. It's the target of the camara.

==== AnalyticIK ====

Assets > AnalyticIK > AnalyticIK

-- Scripts --
- CalcIK : CalcIK calculates the joint angles of the robot using slider values in real time. This unity project uses the analytical approach to calculate the inverse kinematics.
 The support site
https://meuse.co.jp/unity/unity-arm-robot-inverse-kinematics-analytical-approach/
explains the details of the method.
- RotationScript : RotationScript rotates the arm sub-assembly around a joint if the rotation should be made.
- CameraScript : CameraScript controls the view.

-- To edit robot dimensions --
If you want to change the arm length, edit CalcIK to change the values of L1ÅcL6 and also edit Scale and Position of the relevant arms.

* Please don't delete the small cube in the scene. It's the target of the camara.