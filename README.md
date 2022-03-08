# MaichartConverter

A simple script to convert .ma2 to maidata

You can either use existing Program.cs to perform a pre-defined full database convert or do your customized convert without presenting mp3 and background.

[Usage: using existing Program.cs]
The existing Program.cs asks for 4 inputs: A000 location, mp3 location, background location and output location.
Present all 4 parameter or define in Program.cs to specify locations and wait for program to convert all charts.

{Specification of input}
Requires: A000 Location
  A directory that has structure of A000\music\music0xxxxx\, etc. Music.xml must be present for program to read in information. If chart specified in Music.xml is not present, the program will raise exceptions.
Requires: Music Location
  A folder contains mp3 with naming scheme of music0xxxxx.mp3. The id followed by music in file name matches chart id in A000 location, which is also defined in Music.xml. If music specified in Music.xml is not present, the program will raise exceptions.
Requires: Image Location
  A folder contains pictures with naming scheme of UI_Jacket_0xxxxx.png. ID followed by _ in file name matches chart id in A000 location, which is also defined in Music.xml. If image specified in Music.xml is not present, the program will raise exceptions.
Requires: Output Location
  A folder used for output.

{Specification of output}
Ensures: Output Simai Folders
  A folder that contains all charts converted. It follows the scheme of GENRE\CHART_NAME\, which contains corresponding maidata.txt, bg.png, and track.mp3. The GENRE and CHART_NAME is specified in music.xml.
  maidata.txt will include all difficulties from Basic-Re:Master with appropriate Title, Note Designer, BPM, and follows 3Simai DX chart grammar.
  track.mp3 will be corresponding music in music folder specified.
  bg.png will be corresponding png in image folder specified.
