# Manual for `ComposeMa2` command

### Compile assigned Ma2 chart to assigned format

## Basic usage

    MaichartConverter ComposeMa2 --params

## Example usage

    MaichartConverter ComposeMa2 -p "/Users/Neskol/MaiAnalysis/A000/music/music000363/000363_03.ma2" -f Ma2_104

### Required Params:

> Unless otherwise specified, `XXXXXX` here stands for musicID.

- `-p | --path <path>`: Path to .ma2 file to be converted.

### Optional Params:

> Program will print result in the terminal if `-o | --output` parameter is not provided.

- `-f | --format <string>`: Forces program to compile carts in given format (within composed `maidata`.txt). Available
  format: `Simai, SimaiFes, Ma2, Ma2_104`. Note: Festival features requires `SimaiFes` or `Ma2_104` parameter.
- `-r | --rotate <string>`: Forces program to rotate all charts in given method. Available
  rotations: `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <int>`: Shifts notes in all charts front or back by given ticks.
- `-o | --output <path>`: Path to output folder. Program will try to write to this path and create new folder when
  necessary so make sure you have write permission of the given folder.