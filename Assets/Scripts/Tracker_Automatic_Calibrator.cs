using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using RootMotion.Demos;

public class Tracker_Automatic_Calibrator : MonoBehaviour {
    public GameObject leftHand, rightHand, leftLeg, rightLeg, pelvis, head;
    private Quaternion leftHand_Default_Rotation, left_Foot_Default_Rotation, rightHand_Default_Rotation, right_Foot_Default_Rotation, pelvis_Default_Rotation, head_Default_Rotation;
    private Quaternion leftHand_Updated_Rotation, left_Foot_Updated_Rotation, rightHand_Updated_Rotation, right_Foot_Updated_Rotation, pelvis_Updated_Rotation, head_Updated_Rotation;
    public model_and_Steam_VR_Controller m1;
    public GameObject SteamVR_Activator, left_Hand_Target, right_Hand_Target, left_Foot_Target, right_Foot_Target, pelvis_Target, head_Target;
    private bool configuration;
    private Quaternion calculated_difference_of_leftHand, calculated_difference_of_rightHand, calculated_difference_of_leftFoot, calculated_difference_of_rightFoot, calculated_difference_of_pelvis, calculated_difference_of_head;
    private Vector3 default_head_position, default_pelvis_position, updated_head_tracker_position, updated_pelvis_tracker_position;
    private GameObject headTracker, pelvisTracker;
    private float distance_between_default_head_position_and_default_pelvis_position, distance_between_headTracker_and_pelvis_tracker, distance_between_default_head_position_and_headTracker, distance_between_default_and_updated_head_and_pelvis;

    // Use this for initialization
    void Start () {
        leftHand = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root/spine05/spine04/spine03/spine02/spine01/clavicle.L/shoulder01.L/upperarm01.L/upperarm02.L/lowerarm01.L/lowerarm02.L/wrist.L");
        rightHand = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root/spine05/spine04/spine03/spine02/spine01/clavicle.R/shoulder01.R/upperarm01.R/upperarm02.R/lowerarm01.R/lowerarm02.R/wrist.R");
        leftLeg = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root/pelvis.L/upperleg01.L/upperleg02.L/lowerleg01.L/lowerleg02.L/foot.L");
        rightLeg = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root/pelvis.R/upperleg01.R/upperleg02.R/lowerleg01.R/lowerleg02.R/foot.R");
        pelvis = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root");
        head = GameObject.Find(this.gameObject.name + "/MakeHuman default skeleton/root/spine05/spine04/spine03/spine02/spine01/neck01/neck02/neck03/head");

        leftHand_Default_Rotation = leftHand.transform.localRotation;
        rightHand_Default_Rotation = rightHand.transform.localRotation;
        left_Foot_Default_Rotation = leftLeg.transform.localRotation;
        right_Foot_Default_Rotation = rightLeg.transform.localRotation;
        pelvis_Default_Rotation = pelvis.transform.localRotation;
        head_Default_Rotation = head.transform.localRotation;

        default_head_position = head.transform.position;
        default_pelvis_position = pelvis.transform.position;
        distance_between_default_head_position_and_default_pelvis_position = default_head_position.y - default_pelvis_position.y;

        SteamVR_Activator = GameObject.Find("Steam_VR_Activator_&_Avatar_Handler");
        m1 = SteamVR_Activator.GetComponent<model_and_Steam_VR_Controller>();

        configuration = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (headTracker == null)
            headTracker = GameObject.Find("[CameraRig]/Camera (eye)");

        if (pelvisTracker == null)
            pelvisTracker = GameObject.Find("Other Targets/Pelvis_Bone_Tracker");

        if (head_Target == null)
            head_Target = GameObject.Find(headTracker.name + "/Head Target");

        if (pelvis_Target == null)
            pelvis_Target = GameObject.Find(pelvisTracker.name + "/Pelvis Target");

        if (left_Hand_Target == null)
            left_Hand_Target = GameObject.Find("Other Targets/Left_Hand_Tracker/Left Hand Target");

        if (right_Hand_Target == null)
            right_Hand_Target = GameObject.Find("Other Targets/Right_Hand_Tracker/Right Hand Target");

        if (left_Foot_Target == null)
            left_Foot_Target = GameObject.Find("Other Targets/Left_Foot_Tracker/Left Foot Target");

        if (right_Foot_Target == null)
            right_Foot_Target = GameObject.Find("Other Targets/Right_Foot_Tracker/Right Foot Target");



        if (head_Target != null && pelvis_Target != null && left_Hand_Target != null && right_Hand_Target != null && left_Foot_Target != null && right_Foot_Target != null && m1.tracker_configuration == true)
        {
            left_Foot_Updated_Rotation = leftLeg.transform.localRotation;
            calculated_difference_of_leftFoot = left_Foot_Default_Rotation * Quaternion.Inverse(left_Foot_Updated_Rotation);

            right_Foot_Updated_Rotation = rightLeg.transform.localRotation;
            calculated_difference_of_rightFoot = right_Foot_Default_Rotation * Quaternion.Inverse(right_Foot_Updated_Rotation);

            pelvis_Updated_Rotation = pelvis.transform.localRotation;
            calculated_difference_of_pelvis = pelvis_Default_Rotation * Quaternion.Inverse(pelvis_Updated_Rotation);

            leftHand_Updated_Rotation = leftHand.transform.localRotation;
            calculated_difference_of_leftHand = leftHand_Default_Rotation * Quaternion.Inverse(leftHand_Updated_Rotation);

            rightHand_Updated_Rotation = rightHand.transform.localRotation;
            calculated_difference_of_rightHand = rightHand_Default_Rotation * Quaternion.Inverse(rightHand_Updated_Rotation);

            head_Updated_Rotation = head.transform.localRotation;
            calculated_difference_of_head = head_Default_Rotation * Quaternion.Inverse(head_Updated_Rotation);

            Quaternion a = left_Hand_Target.transform.localRotation;
            a *= calculated_difference_of_leftHand;
            left_Hand_Target.transform.localRotation = a;

            Quaternion b = right_Hand_Target.transform.localRotation;
            b *= calculated_difference_of_rightHand;
            right_Hand_Target.transform.localRotation = b;

            Quaternion e = pelvis_Target.transform.localRotation;
            e *= calculated_difference_of_pelvis;
            pelvis_Target.transform.localRotation = e;

            Quaternion f = head_Target.transform.localRotation;
            f *= calculated_difference_of_head;
            head_Target.transform.localRotation = f;

            Quaternion c = left_Foot_Target.transform.localRotation;
            c *= calculated_difference_of_leftFoot;
            c *= Quaternion.Euler(0, 0, -90);
            left_Foot_Target.transform.localRotation = c;

            Quaternion d = right_Foot_Target.transform.localRotation;
            d *= calculated_difference_of_rightFoot;
            d *= Quaternion.Euler(0, 0, 90);
            right_Foot_Target.transform.localRotation = d;

            Vector3 head_target_position = head_Target.transform.localPosition;
            Vector3 pelvis_target_position = pelvis_Target.transform.localPosition;
            Quaternion left_hand_target_rotation = left_Hand_Target.transform.localRotation;
            Quaternion right_hand_target_rotation = right_Hand_Target.transform.localRotation;
            Quaternion left_foot_target_rotation = left_Foot_Target.transform.localRotation;
            Quaternion right_foot_target_rotation = right_Foot_Target.transform.localRotation;

            Vector3 head_target_Offset = new Vector3(0f, 0f, -0.12f);
            head_target_position += head_target_Offset;
            head_Target.transform.localPosition = head_target_position;

            updated_head_tracker_position = headTracker.transform.position;
            updated_pelvis_tracker_position = pelvisTracker.transform.position;

            distance_between_headTracker_and_pelvis_tracker = updated_head_tracker_position.y - updated_pelvis_tracker_position.y;

            //distance_between_default_head_position_and_headTracker = default_head_position.y - updated_head_tracker_position.y;

            //if (distance_between_default_head_position_and_headTracker < 0f)
            //{
            //    pelvis_target_position = new Vector3(0f, 0f, distance_between_default_head_position_and_headTracker);
            //    pelvis_Target.transform.localPosition = pelvis_target_position;
            //}

            //else if (distance_between_default_head_position_and_headTracker > 0f)
            //{
            //    pelvis_target_position = new Vector3(0f, 0f, -distance_between_default_head_position_and_headTracker);
            //    pelvis_Target.transform.localPosition = pelvis_target_position;
            //}

            pelvis_target_position = new Vector3(0f, pelvis_target_position.y / 2, pelvis_target_position.z);
            pelvis_Target.transform.localPosition = pelvis_target_position;

            //distance_between_default_and_updated_head_and_pelvis = distance_between_default_head_position_and_default_pelvis_position - distance_between_headTracker_and_pelvis_tracker;

            //if (distance_between_default_and_updated_head_and_pelvis < 0f)
            //{
            //    pelvis_target_position = new Vector3(0f, /*0f*/ -head_target_Offset.z, distance_between_default_and_updated_head_and_pelvis);
            //    pelvis_Target.transform.localPosition = pelvis_target_position;
            //}

            //else if (distance_between_default_and_updated_head_and_pelvis > 0f)
            //{
            //    pelvis_target_position = new Vector3(0f, /*0f*/ -head_target_Offset.z, -distance_between_default_and_updated_head_and_pelvis);
            //    pelvis_Target.transform.localPosition = pelvis_target_position;
            //}

            //pelvis_target_position = new Vector3(0.003f, -0.029f, -0.1f/*head_target_Offset.z - 0.002f, head_target_Offset.z, -head_target_Offset.z + (-0.09f)*/);
            //pelvis_Target.transform.localPosition = pelvis_target_position;

            right_hand_target_rotation *= Quaternion.Euler(25, 0, 0);
            right_Hand_Target.transform.localRotation = right_hand_target_rotation;

            left_hand_target_rotation *= Quaternion.Euler(25, 0, 0);
            left_Hand_Target.transform.localRotation = left_hand_target_rotation;

            left_foot_target_rotation *= Quaternion.Euler(-25, -10, 0);
            Quaternion left_foot_target_rotation_new = left_foot_target_rotation * Quaternion.Euler(0, 0, 20);
            left_Foot_Target.transform.localRotation = left_foot_target_rotation_new/*Quaternion.Euler(-15, 170, -90)*/;

            right_foot_target_rotation *= Quaternion.Euler(-25, 10, 0);
            Quaternion right_foot_target_rotation_new = right_foot_target_rotation * Quaternion.Euler(0, 0, -20);
            right_Foot_Target.transform.localRotation = right_foot_target_rotation_new/*Quaternion.Euler(20, 180, -30)*/;

            m1.tracker_configuration = false;
        }
    }
}
