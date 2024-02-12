# Manual for `ComposeMa2ID` command

### Compile assigned Ma2 chart indexed by ID to assigned format

## Basic usage

    MaichartConverter ComposeMa2ID --params

## Example usage

    MaichartConverter ComposeMa2ID -p "/Users/Neskol/MaiAnalysis/A000" -i 363 -d 3 -f Ma2_104

## Available Params

### Required Params:

> Unless otherwise specified, `XXXXXX` here stands for musicID.

- `-p | --path <path>`: Path to `A000` Folder. Expects file structure of `A000/music/musicXXXXXX/Music.xml`
- `-i | --id <int>`: Index of the track. Program will compensate 0s at the front.
- `-d | --difficulty <0-4>`: Difficulty to be specified. 0-4 for Basic to Re:Master.

### Optional Params:

> Program will print result in the terminal if `-o | --output` parameter is not provided.

- `-f | --format <string>`: Forces program to compile carts in given format (within composed `maidata`.txt). Available
  format: `Simai, SimaiFes, Ma2, Ma2_104`. Note: Festival features requires `SimaiFes` or `Ma2_104` parameter.
- `-r | --rotate <string>`: Forces program to rotate all charts in given method. Available
  rotations: `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <int>`: Shifts notes in all charts front or back by given ticks.
- `-o | --output <path>`: Path to output folder. Program will try to write to this path and create new folder when
  necessary so make sure you have write permission of the given folder.