# `CompileDatabase`命令指南

### 转译整个含Ma2文件的目录

## 基础语法

    MaichartConverter CompileDatabase --参数

## 示例用法

    MaichartConverter CompileDatabase -p "/Users/Neskol/MaiAnalysis/A000/" -c "/Users/Neskol/MaiAnalysis/Image/Texture2D/" -v "/Users/Neskol/MaiAnalysis/DXBGA_HEVC/" -m "/Users/Neskol/MaiAnalysis/Sound/" -o "/Users/Neskol/MaiAnalysis/Output_HEVC_By_Cab/" -g 2 -d

## 可用参数

### 必要参数

> 除非另有所指，`XXXXXX`在此指代谱面ID

- `-p | --path <path>`: Path to `A000` Folder. Expects file structure of `A000/music/musicXXXXXX/Music.xml`
- `-o | --output <path>`: Path to output folder. Program will try to write to this path and create new folder when
  necessary so make sure you have write permission of the given folder.

### Optional Params:

- `-m | --music <path>`: Path to MP3 files. Expected naming scheme is `musicXXXXXX.mp3`.
- `-c | --cover <path>`: Path to track pictures. Expected naming scheme is `UI_Jacket_XXXXXX.png`.
- `-v | --video <path>`: Path to background videos. Expected naming scheme is `musicXXXXXX.mp4`
- `-g | --genre <number 0-6> `: Preferred categorizing scheme.
  `0 = Genre, 1 = Level, 2 = Cabinet, 3 = Composer, 4 = BPM, 5 - SD/DX Chart, 6 = No subfolders`
- `-d | --decimal`: Option to force levels to be displayed in decimals (e.g. `14+ => 14.7`).

### Depreciated/Under development params:

> These parameters are not tested. Feel free to test and report issues!

- `-f | --format <string>`: Forces program to compile carts in given format (within composed `maidata`.txt). Available
  format: `Simai, SimaiFes, Ma2, Ma2_104`.
- `-r | --rotate <string>`: Forces program to rotate all charts in given method. Available
  rotations: `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <int>`: Shifts notes in all charts front or back by given ticks.
