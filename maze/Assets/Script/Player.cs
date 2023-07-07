using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5f;
    Vector3 rotation;
    private Animation anim;
    AnimationClip clip;
    bool isPlay=false;
    bool isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.eulerAngles;
        anim = gameObject.AddComponent<Animation>();
        clip = new AnimationClip()
        {
            name = "test",
            legacy = true,
            wrapMode = WrapMode.Once
        };
    }

    public void SetIsWin(bool flag)
    {
        isWin = flag;
    }

    void StepMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += transform.forward * v * speed * Time.deltaTime;
        transform.position += transform.right * h * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWin) return;
        StepMove();
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isPlay)
            {
                AnimationCurve curveX = AnimationCurve.Linear(0f, rotation.x, 1f, rotation.x);
                AnimationCurve curveY = AnimationCurve.Linear(0f, rotation.y, 1f, rotation.y - 90f);
                AnimationCurve curveZ = AnimationCurve.Linear(0f, rotation.z, 1f, rotation.z);
                rotation.y -= 90.0f;
                clip.SetCurve("", typeof(Transform), "localEulerAngles.x", curveX);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.y", curveY);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.z", curveZ);
                anim.AddClip(clip, "MyAnimation");
                anim.Play("MyAnimation");
                isPlay = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            if (!isPlay && rotation.x <= 0f)
            {
                AnimationCurve curveX = AnimationCurve.Linear(0f, rotation.x, 1f, rotation.x + 90f);
                AnimationCurve curveY = AnimationCurve.Linear(0f, rotation.y, 1f, rotation.y);
                AnimationCurve curveZ = AnimationCurve.Linear(0f, rotation.z, 1f, rotation.z);
                rotation.x += 90.0f;
                clip.SetCurve("", typeof(Transform), "localEulerAngles.x", curveX);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.y", curveY);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.z", curveZ);
                anim.AddClip(clip, "MyAnimation");
                anim.Play("MyAnimation");
                isPlay=true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            if (!isPlay)
            {
                AnimationCurve curveX = AnimationCurve.Linear(0f, rotation.x, 1f, rotation.x);
                AnimationCurve curveY = AnimationCurve.Linear(0f, rotation.y, 1f, rotation.y + 90f);
                AnimationCurve curveZ = AnimationCurve.Linear(0f, rotation.z, 1f, rotation.z);
                rotation.y += 90.0f;
                clip.SetCurve("", typeof(Transform), "localEulerAngles.x", curveX);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.y", curveY);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.z", curveZ);
                anim.AddClip(clip, "MyAnimation");
                anim.Play("MyAnimation");
                isPlay = true;
            }
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            if (!isPlay&&rotation.x>=0)
            {
                AnimationCurve curveX = AnimationCurve.Linear(0f, rotation.x, 1f, rotation.x - 90f);
                AnimationCurve curveY = AnimationCurve.Linear(0f, rotation.y, 1f, rotation.y);
                AnimationCurve curveZ = AnimationCurve.Linear(0f, rotation.z, 1f, rotation.z);
                rotation.x -= 90f;
                clip.SetCurve("", typeof(Transform), "localEulerAngles.x", curveX);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.y", curveY);
                clip.SetCurve("", typeof(Transform), "localEulerAngles.z", curveZ);
                anim.AddClip(clip, "MyAnimation");
                anim.Play("MyAnimation");
                isPlay=true;
            }
        }
        if(isPlay&&!anim.isPlaying)
        {
            isPlay = false;
            transform.eulerAngles = rotation;
        }
    }
}
