# Manual for `ComposeSimai` command

### Compile assigned simai chart to assigned format

#### _NOTE: This command converts complete Simai file （at least start with `&inote_X=`）._

## Basic usage

    MaichartConverter ComposeSimai --params

## Example usage

    MaichartConverter ComposeSimai -p "/Users/Neskol/MaiAnalysis/Output_NoBGA/maimai/363_OSHAMASCRAMBLE/maidata.txt" -f Ma2_104

### Required Params:

> Unless otherwise specified, `XXXXXX` here stands for musicID.

- `-p | --path <path>`: Path to Simai file to be converted.

### Optional Params:

> Program will print result in the terminal if `-o | --output` parameter is not provided.

- `-d | --difficulty <number 1-7>`: Difficulty to be specified. 1-6 for `Easy` to `Re:Master`. Program will find for the
  first chart to compose if not specified. 7 for `Original` only if file contains.
- `-f | --format <string>`: Forces program to compile carts in given format (within composed `maidata`.txt). Available
  format: `Simai, SimaiFes, Ma2, Ma2_104`. Note: Festival features requires `SimaiFes` or `Ma2_104` parameter.
- `-r | --rotate <string>`: Forces program to rotate all charts in given method. Available
  rotations: `Clockwise90, Clockwise180, CounterClockwise90, CounterClockwise180, UpsideDown, LeftToRight`.
- `-s | --shift <int>`: Shifts notes in all charts front or back by given ticks.
- `-o | --output <path>`: Path to output folder. Program will try to write to this path and create new folder when
  necessary so make sure you have write permission of the given folder.