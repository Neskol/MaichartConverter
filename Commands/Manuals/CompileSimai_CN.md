# `ComposeSimai`命令指南

### 转译指定Simai谱面到指定格式

#### _注意: 此命令只转换含`&inote_X=`的完整simai谱面。_

## 基础用法

    MaichartConverter ComposeSimai --参数

## 示例用法

    MaichartConverter ComposeSimai -p "/Users/Neskol/MaiAnalysis/Output_NoBGA/maimai/363_OSHAMASCRAMBLE/maidata.txt" -f Ma2_104

## 可用参数

### 必要参数:

> 除非另有所指，`XXXXXX`在此指代谱面ID

- `-p | --path <目录>`: 需要转换的Simai文件路径

### Optional Params:

> 如果`-o | --output`参数没有提供，程序将会在终端输出结果

- `-d | --difficulty <数字 1-7>`: 指定的难度。 1-6 指代 `Easy` 到 `Re:Master`。 如果没有指定难度，程序将会转译第一个谱面。 7指代`Original`。
- `-f | --format <选项>`: 强制将谱面编写为该格式。可用格式: `Simai, SimaiFes, Ma2, Ma2_104`.
- `-r | --rotate <选项>`: 强制按要求旋转谱面。可用选项： `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <整数>`: 将所有谱面按指定间隔向前/向后平移。
- `-o | --output <目录>`: 输出文件夹路径。如果该路径不存在，程序会尝试在该位置创建新文件夹。请确保该路径可写。
