# 变更日志

此文件列出了当前功能分支和
上一个功能版本之间所做的所有更改。它相当于我们
[交互式变更日志](https://godotengine.github.io/godot-interactive-changelog/) 上的列表。

早期功能版本的变更日志可在各自的 Git
分支中找到，并链接到[此文件末尾](#Past-releases)。

## 4.3 - 2024-08-15

- [发布公告](https://godotengine.org/releases/4.3/)
- [迁移指南](https://docs.godotengine.org/en/latest/tutorials/migrating/upgrading_to_godot_4.3.html)
- [交互式变更日志](https://godotengine.github.io/godot-interactive-changelog/#4.3)
- [重大变更](https://github.com/godotengine/godot/pulls?q=is%3Apr+is%3Amerged+label%3A%22breaks+compat%22+milestone%3A4.3)

目录：

- [2D](#2D)
- [3D](#3D)
- [动画](#Animation)
- [Assetlib](#Assetlib)
- [音频](#音频)
- [构建系统](#构建系统)
- [C#](#C)
- [代码样式](#代码样式)
- [核心](#核心)
- [文档](#文档)
- [编辑器](#编辑器)
- [导出](#导出)
- [GDExtension](#GDExtension)
- [GDScript](#GDScript)
- [GUI](#GUI)
- [导入](#导入)
- [输入](#输入)
- [多人游戏](#多人游戏)
- [导航](#导航)
- [网络](#网络)
- [粒子](#粒子)
- [物理](#物理)
- [插件](#插件)
- [移植](#移植)
- [渲染](#渲染)
- [着色器](#着色器)
- [测试](#测试)
- [第三方](#第三方)
- [XR](#XR)

#### 2D

- 添加临时枢轴以旋转多个 2D 节点 ([GH-58375](https://github.com/godotengine/godot/pull/58375))。
- 重新组织一些代码以将 2D 内容分组在一起 ([GH-66744](https://github.com/godotengine/godot/pull/66744))。
- 添加选项以切换 2D 编辑器中位置小工具的可见性，组织现有选项 ([GH-75005](https://github.com/godotengine/godot/pull/75005))。
- 更改了评估某一点处曲线旋转的方式，以匹配 PathFollow2D ([GH-78378](https://github.com/godotengine/godot/pull/78378))。
- 在 Viewport 中组织 2D 音频、摄像头和物理 ([GH-79183](https://github.com/godotengine/godot/pull/79183))。
- 扩展 TextureRegion 编辑器的最小/最大缩放级别 ([GH-79436](https://github.com/godotengine/godot/pull/79436))。
- 为图块源类型添加工具提示 ([GH-79918](https://github.com/godotengine/godot/pull/79918))。
- 添加 Texture2D 和 Texture3D 图标 ([GH-81169](https://github.com/godotengine/godot/pull/81169))。
- 在 Path2D 编辑器中添加一个按钮来清除曲线点 ([GH-81325](https://github.com/godotengine/godot/pull/81325))。
- 改进 UV 编辑器缩放行为 ([GH-83731](https://github.com/godotengine/godot/pull/83731))。
- 修复 UV 编辑器不使用纹理变换 ([GH-84076](https://github.com/godotengine/godot/pull/84076))。
- 修复缓慢的光线投射编辑 ([GH-84164](https://github.com/godotengine/godot/pull/84164))。
- 向 Sprite2DPlugin 转换器预览中添加缩放控件 ([GH-84353](https://github.com/godotengine/godot/pull/84353))。
- 在 2D 视口中拖放时选择新添加的节点 ([GH-84356](https://github.com/godotengine/godot/pull/84356))。
- 修复使用某些图像格式生成地形图标的问题 ([GH-84507](https://github.com/godotengine/godot/pull/84507))。
- 将图块转换处理缓存移至 TileData ([GH-84660](https://github.com/godotengine/godot/pull/84660))。
- 向 Sprite2DPlugin 转换器添加区域矩形和框架支持 ([GH-84754](https://github.com/godotengine/godot/pull/84754))。
- 修复 TileMap 编辑器工具提示中的拼写错误 ([GH-85452](https://github.com/godotengine/godot/pull/85452))。
- 即使 TileMap 不可见，仍保留场景图块 ([GH-85753](https://github.com/godotengine/godot/pull/85753))。
- 将 TileMapLayer 移动到其自己的文件中 ([GH-85791](https://github.com/godotengine/godot/pull/85791))。
- 添加图块属性的描述 ([GH-85868](https://github.com/godotengine/godot/pull/85868))。
- 修复 TileMap 遮挡物 ([GH-85893](https://github.com/godotengine/godot/pull/85893))。
- 将 SkeletonModification2DTwoBoneIK 的后缀从 m 更改为 px ([GH-86056](https://github.com/godotengine/godot/pull/86056))。
- 未选择工具栏模式时重置 TileMap 编辑器 `drag_type` ([GH-86066](https://github.com/godotengine/godot/pull/86066))。
- 修复创建新图集时 `UndoRedo 历史不匹配` ([GH-86387](https://github.com/godotengine/godot/pull/86387))。
- 修复计算图块编辑器缩放级别时可能出现的无限循环 ([GH-86568](https://github.com/godotengine/godot/pull/86568))。
- 调整图块图集帮助标签位置 ([GH-86694](https://github.com/godotengine/godot/pull/86694))。
- 允许 `ui_cancel` 取消选择 2D 中所有编辑模式中的所有节点，匹配 3D 行为 ([GH-86805](https://github.com/godotengine/godot/pull/86805))。
- 修复 `TileMap` 象限画布项目位置不是本地的问题 ([GH-86847](https://github.com/godotengine/godot/pull/86847))。
- 修复全局历史记录注册导致的 2D 视口纹理丢失问题 ([GH-86933](https://github.com/godotengine/godot/pull/86933))。
-
