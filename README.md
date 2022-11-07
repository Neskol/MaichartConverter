# MaichartConverter

## A simple program provide functionality of converting maimai chart between Simai and Ma2.
> The original supporting classes are now separated from this repo. Please see [MaiLib](https://github.com/Neskol/MaiLib) for more information.

### Build
    git clone
    git submodule update --init --recursive
    dotnet build
### Usage
> Type 'MaichartConverter help' for manual

### Parameters notice
- music files should be named musicxxxxxx.mp3 which xxxxxx matches the music id specified in music.xml in each a000 folder, and compensate 0s at the front to 6 digits
- bga files should be named xxxxxx.mp4 which matches the music id specified in music.xml in each a000 folder, and compensate 0s at the front to 6 digits
- image folder should be structured in image/Texture2D/ and the files should start with UI_Jacket_xxxxxx.jpg which xxxxxx matches the music id specified in music.xml in each a000 folder, and compensate 0s at the front to 6 digits
- All of the rules specified above is in convenience for you to directly use after you obtain data from considerable ways
- The difficulty parameter is listed 0-4 as Basic to Re:Master. In MaiLib I specified rules for Easy and Utage but it takes times for me to figure it out, or you could implement on you own referring MaiLib code
- All of the path should end with path separator like "/" or "\". You cannot include quote signs in the path.
- If you have difficulty using the commands, please refer VSCode launch.json where I included several examples
- The whole program was planned to convert from ma2 to simai initially and all other features were developed after that, so there is a HUGE amount of compromises in code design which made it hard to read (but works so far). It would be most kind of you if you would like to help me fixing that

### Disclamer
- Copyrights of the works belong to each individual right holders. This tool is purely used as non-commercial and study purpose. You should find your way for any resource might be used and properly use at your own risk.
- If you would like to use the parser in your project, please refer [MaiLib](https://github.com/Neskol/MaiLib) and hopefully that helps!
