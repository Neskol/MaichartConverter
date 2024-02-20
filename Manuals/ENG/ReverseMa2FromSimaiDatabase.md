# Manual for `ReverseMa2FromSimaiDatabase` command

### Reverse Simai Database from given folder to MaiAnalysis folder for compilation

## Basic usage

    MaichartConverter ReverseMa2FromSimaiDatabase --params

## Example usage

    MaichartConverter ReverseMa2FromSimaiDatabase -p "/Users/Neskol/MaiAnalysis/Output_NoBGA“

## Available Params

### Optional Params:

> - If you run this program directly from the folder which contains required assets, you do not have to provide `path`
    param, but the file structure must be the same with specifications below.
> - If you want this program to create a `MaiAnalysis` folder in the current folder, you do not have to provide `output`
    param.
> - Unless otherwise specified, `XXXXXX` here stands for musicID.

- `-p | --path <path>`: Path to input folder. Expects chart folder to be named as `XXX_TrackName...`.
- `-o | --output <path>`: Path to output folder.
