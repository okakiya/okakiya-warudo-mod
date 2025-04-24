using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Localization;
using System.Collections.Generic;
using System;

[NodeType(
    Id = "0b75fe9e-c7f1-4964-20a3-1936b913dd9f",
    Title = "PerfectSyncStay",
    Category = "CATEGORY_MOTION_CAPTURE"
)]
public class PerfectSyncStay : Node
{
    private Quaternion[] TmpBoneRotateList;
    private Dictionary<string, float> TmpBlendShapeDict;

    [DataInput]
    [Label("tracking")]
    public bool Tracking;

    [DataInput]
    [Label("BoneRotateList")]
    public Quaternion[] BoneRotateList;

    [DataInput]
    [Label("BlendShapeList")]
    public Dictionary<string, float> BlendShapeDict;

    [DataOutput]
    [Label("BoneRotateList")]
    public Quaternion[] OutputBoneRotateList()
    {
        return TmpBoneRotateList;
    }

    [DataOutput]
    [Label("BlendShapeList")]
    public Dictionary<string, float> OutputBlendShapeDict()
    {
        return TmpBlendShapeDict;
    }

    public PerfectSyncStay()
    {
        TmpBoneRotateList = new Quaternion[0];
        TmpBlendShapeDict = new Dictionary<string, float>();
        Tracking = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Tracking) {
            TmpBoneRotateList = BoneRotateList;
            TmpBlendShapeDict = BlendShapeDict;
        }
    }
}