# `ComposeMa2ID`命令指南

### 转译由musicID指定的Ma2谱面

## 基本语法

    MaichartConverter ComposeMa2ID --参数

## 示例用法

    MaichartConverter ComposeMa2ID -p "/Users/Neskol/MaiAnalysis/A000" -i 363 -d 3 -f Ma2_104

## 可用参数

### 必要参数:

> 除非另有所指，`XXXXXX`在此指代谱面ID

- `-p | --path <目录>`: `A000`目录的路径。需要该文件夹有如下结构：`A000/music/musicXXXXXX/Music.xml`
- `-i | --id <数字>`: 谱面的ID。 如果给定数字不满6位，程序会自动在前补齐0。
- `-d | --difficulty <数字0-4>`: 给定谱面的难度。 0-4 指代 Basic 到 Re:Master.

### 可选参数:

> 如果`-o | --output`参数没有提供，程序将会在终端输出结果

- `-f | --format <选项>`: 强制将谱面编写为该格式。可用格式: `Simai, SimaiFes, Ma2, Ma2_104`.
- `-r | --rotate <选项>`:
  强制按要求旋转谱面。可用选项：
  `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <整数>`: 将所有谱面按指定间隔向前/向后平移。
- `-o | --output <目录>`: 输出文件夹路径。如果该路径不存在，程序会尝试在该位置创建新文件夹。请确保该路径可写。
