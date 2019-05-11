using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public Hologram hologram;

    public Vector3 InitialPos;

    public void SetIntialPos()
    {
        InitialPos = this.transform.position;
    }

    public void ResetToInitial()
    {
        this.transform.position = InitialPos;
    }
}
