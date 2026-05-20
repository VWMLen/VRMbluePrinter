# 設計書一覧: VRMローダー機能

## View 層
### クラス: vVrmLoader (vVrmLoader.cs)
| メソッド名 | 戻り値 | 概要 |
| :--- | :--- | :--- |
| rotateCamera | `void` |  |
| sizeCamera | `void` |  |
| postion | `void` |  |
| cameraModelReset | `void` |  |
| setCameraMove | `void` |  |
| getClip | `HumanPoseClip` |  |
| getVrmGameObject | `resultInfo` |  |
| getModelInfo | `resultInfo` |  |

---

## Controller 層
### クラス: cVrmLoader (cVrmLoader.cs)
| メソッド名 | 戻り値 | 概要 |
| :--- | :--- | :--- |
| setVrmGameObject | `resultInfo` |  |
| getVrmGameObject | `resultInfo` |  |
| cameraModelReset | `resultInfo` |  |
| getModelInfo | `resultInfo` |  |
| setCameraMove | `void` |  |

---

## Service 層
### クラス: sVrmLoader (sVrmLoader.cs)
| メソッド名 | 戻り値 | 概要 |
| :--- | :--- | :--- |
| LoadVRMModel | `void` |  |
| LoadBytesAsync | `Task` |  |
| GetVrmMaterialGenerator | `IMaterialDescriptorGenerator` |  |
| setVrmGameObject | `resultInfo` |  |
| getVrmGameObject | `resultInfo` |  |
| setModelInfo | `resultInfo` |  |
| getModelInfo | `resultInfo` |  |
| setModelInfoWindows | `resultInfo` |  |
| cameraModelReset | `resultInfo` |  |
| modelViewReset | `GameObject` |  |
| setCameraMove | `void` |  |

---


