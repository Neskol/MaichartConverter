# `ReverseMa2FromSimaiDatabase`命令指南

### 将给定Simai谱面文件夹中的各项数据复制到MaiAnalysis文件夹方便本程序处理

## 基本语法
> 仅支持使用本程序生成的Simai谱面文件夹 或遵循本程序命名方式的文件夹

    MaichartConverter ReverseMa2FromSimaiDatabase --参数

## 示例用法

    MaichartConverter ReverseMa2FromSimaiDatabase -p "/Users/Neskol/MaiAnalysis/Output_NoBGA“

## 可用参数

### 可选参数:

> - 如果直接在素材文件夹运行本程序则不需要提供 `path`参数，但是文件夹命名规则必须与如下所述规则一致。
> - 如果希望程序直接在当前目录创建`MaiAnalysis`文件夹，则不需要提供`output`参数.
> - 除非另有所指，`XXXXXX`在此指代谱面ID

- `-p | --path <目录>`: 输入文件夹。需要谱面文件夹命名方式为`XXX_TrackName...`.
- `-o | --output <目录>`: 输出文件夹。
