using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

public class TrackerCalibrationController : MonoBehaviour {
    public GameObject modelHead, modelPelvis, modelLeftFoot, modelRightFoot, modelLeftWrist, modelRightWrist;
    [HideInInspector]
    public GameObject tracker1, tracker2, tracker3, tracker4, tracker5;
    private int count;
    public static bool count1;
    private StringBuilder result;
    private uint tracker;

    // Use this for initialization
    void Start() {
        count = 1;
        count1 = false;
        tracker = 1;

        modelHead = GameObject.Find("[CameraRig]/Camera (eye)");
        modelPelvis = GameObject.Find("Other Targets/Pelvis_Bone_Tracker");
        modelLeftFoot = GameObject.Find("Other Targets/Left_Foot_Tracker");
        modelRightFoot = GameObject.Find("Other Targets/Right_Foot_Tracker");
        modelLeftWrist = GameObject.Find("Other Targets/Left_Hand_Tracker");
        modelRightWrist = GameObject.Find("Other Targets/Right_Hand_Tracker");
        tracker1 = GameObject.Find("Actual Targets/Tracker 1");
        tracker2 = GameObject.Find("Actual Targets/Tracker 2");
        tracker3 = GameObject.Find("Actual Targets/Tracker 3");
        tracker4 = GameObject.Find("Actual Targets/Tracker 4");
        tracker5 = GameObject.Find("Actual Targets/Tracker 5");

        var error = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i <= 7; i++)
        {
            result = new StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
            Debug.Log("Result = " + result.ToString());
            if (result.ToString().Contains("tracker"))
            {
                if (tracker == 1)
                    tracker1.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 2)
                    tracker2.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 3)
                    tracker3.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 4)
                    tracker4.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                else if (tracker == 5)
                    tracker5.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)i;
                ++tracker;
            }
        }
    }


    // Update is called once per frame
    void Update() {
        if (count < 3)
            count++;
        else if (count > 3)
            count = 4;
        else
        {
            Debug.Log("Tracker 1 distance = " + Mathf.Round(tracker1.transform.position.y * 10f) / 10f);
            Debug.Log("Tracker 2 distance = " + Mathf.Round(tracker2.transform.position.y * 10f) / 10f);
            Debug.Log("Tracker 3 distance = " + Mathf.Round(tracker3.transform.position.y * 10f) / 10f);
            Debug.Log("Tracker 4 distance = " + Mathf.Round(tracker4.transform.position.y * 10f) / 10f);
            Debug.Log("Tracker 5 distance = " + Mathf.Round(tracker5.transform.position.y * 10f) / 10f);

            GameObject[] tracker_array = new GameObject[] { tracker1, tracker2, tracker3, tracker4, tracker5 };
            tracker_array = tracker_array.OrderByDescending(v => Mathf.Round((v.transform.position.y) * 10f) / 10f).ToArray<GameObject>();

            Debug.Log("T1 = " + tracker_array[0].transform.name);
            Debug.Log("T2 = " + tracker_array[1].transform.name);
            Debug.Log("T3 = " + tracker_array[2].transform.name);
            Debug.Log("T4 = " + tracker_array[3].transform.name);
            Debug.Log("T5 = " + tracker_array[4].transform.name);

            for (int i = 0; i < 2; i++)
            {
                float min = tracker_array[i].transform.position.z;

                for (int j = i + 1; j < 3; j++)
                {
                    if (min > tracker_array[j].transform.position.z)
                    {
                        GameObject tmp = tracker_array[i];
                        tracker_array[i] = tracker_array[j];
                        tracker_array[j] = tmp;
                    }
                }
            }

            if (tracker_array[3].transform.position.z > tracker_array[4].transform.position.z)
            {
                GameObject tmp = tracker_array[3];
                tracker_array[3] = tracker_array[4];
                tracker_array[4] = tmp;
            }

            Debug.Log("After Swapping T1 = " + tracker_array[0].transform.name);
            Debug.Log("After Swapping T2 = " + tracker_array[1].transform.name);
            Debug.Log("After Swapping T3 = " + tracker_array[2].transform.name);
            Debug.Log("After Swapping T4 = " + tracker_array[3].transform.name);
            Debug.Log("After Swapping T5 = " + tracker_array[4].transform.name);

            uint index1 = (uint)tracker_array[0].GetComponent<SteamVR_TrackedObject>().index;
            uint index2 = (uint)tracker_array[1].GetComponent<SteamVR_TrackedObject>().index;
            uint index3 = (uint)tracker_array[2].GetComponent<SteamVR_TrackedObject>().index;
            uint index4 = (uint)tracker_array[3].GetComponent<SteamVR_TrackedObject>().index;
            uint index5 = (uint)tracker_array[4].GetComponent<SteamVR_TrackedObject>().index;

            modelPelvis.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index2;
            modelLeftWrist.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index3;
            modelRightWrist.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index1;
            modelLeftFoot.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index5;
            modelRightFoot.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)index4;

            count1 = true;
            count++;

            for (int i = 0; i <= 4; i++)
            {
                Destroy(tracker_array[i]);
            }
        }
    }
}