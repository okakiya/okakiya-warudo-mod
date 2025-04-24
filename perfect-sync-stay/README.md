# 概要

「フェイストラッキングiFacialMocap」ブループリント向けノード

iFacialMocapでトラッキング中の場合だけBlendShapeリストとボーン回転リストを保存するノードです。トラッキングが切れたときに最後の表情を残しておきたい場合等にお使いください。

Floatの切り替えノードはいい塩梅で調整してください。

# ノード
- PerfectSyncStay

# 入力
「iFacialMocap受信機データを取得」ノードの値を対応する入力に接続
- Tracking : 追跡中
- BoneRotateList : ボーン回転リスト
- BlendShapeList : BlendShapeリスト

# 出力
「BlendShapeリストの切り替え」ノード、「回転リストの切り替え」ノードの"条件が偽である"の入力に接続してください。

--

Blueprint Node for Face Tracking: iFacialMocap

This node saves the BlendShape list and Bone Rotation list only while iFacialMocap tracking is active.
Use this if you want to preserve the last tracked facial expression when tracking stops.

The Float toggle node should be adjusted to suit your needs.

# Node

- PerfectSyncStay

# Inputs

Connect the outputs from the “Get iFacialMocap Receiver Data” node to the corresponding inputs:

- Tracking – tracking
- BoneRotateList – List of bone rotations
- BlendShapeList – List of BlendShapes

# Outputs
Connect these outputs to the “Condition is False” input of the following nodes:

- BlendShape List node
- Rotation List node