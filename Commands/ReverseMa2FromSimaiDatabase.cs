using ManyConsole;
using MaiLib;

namespace MaichartConverter;

public class ReverseMa2FromSimaiDatabase : ConsoleCommand
{
    /// <summary>
    /// Return when command successfully executed
    /// </summary>
    private const int Success = 0;
    /// <summary>
    /// Return when command failed to execute
    /// </summary>
    private const int Failed = 2;

    public string SourceLocation { get; private set; }
    public string? Destination { get; private set; }
    public bool OverwriteDestination { get; private set; }

    public ReverseMa2FromSimaiDatabase()
    {
        SourceLocation = ".";
        OverwriteDestination = false;
        IsCommand("ReverseMa2FromSimaiDatabase",
            "Reverse Simai Database from given folder to MaiAnalysis folder for compilation");
        HasOption("p|path=", "Load Simai files from this location", path => SourceLocation = path ?? "");
        HasOption("o|output=", "Write extracted files into this directory. New folder will be created if not provided",
            output => Destination = output);
        HasOption("r|replace:", "Replace files at destination if already exist. By default is false.",
            _ => OverwriteDestination = true);
    }

    public override int Run(string[] remainingArguments)
    {
        try
        {
            if (Destination is null) Destination = $"{SourceLocation}\\MaiAnalysis";
            string[] mainFolderInfo = Directory.GetDirectories(SourceLocation, "", SearchOption.AllDirectories);

            // Check if main directory exists
            if (!Directory.Exists(Destination))
            {
                // Console.WriteLine("Source folder: {0}",SourceLocation);
                Console.WriteLine("Destination folder is not found, creating one at: {0}",Destination);
                Directory.CreateDirectory(Destination);
            }
            // Check sub directories of destination
            string soundLocation = $"{Destination}/Sound";
            string imageLocation = $"{Destination}/Image/Texture2D";
            string bgaLocation = $"{Destination}/DXBGA";
            if (!Directory.Exists(soundLocation))
            {
                Console.WriteLine("Sound folder is not found, creating one at: {0}", soundLocation);
                Directory.CreateDirectory(soundLocation);
            }
            if (!Directory.Exists(imageLocation))
            {
                Console.WriteLine("Image folder is not found, creating one at: {0}", imageLocation);
                Directory.CreateDirectory(imageLocation);
            }
            if (!Directory.Exists(bgaLocation))
            {
                Console.WriteLine("BGA folder is not found, creating one at: {0}", bgaLocation);
                Directory.CreateDirectory(bgaLocation);
            }
            foreach (string path in mainFolderInfo)
            {
                // Console.WriteLine(path);
                Console.WriteLine(Path.GetFileName(path));
                string idCandidate = Path.GetFileName(path).Split('_')[0];
                bool isChartFolder = int.TryParse(idCandidate, out int id);
                // Console.WriteLine("ID of path: {0}", idCandidate);
                if (isChartFolder)
                {
                    if (File.Exists($"{path}/bg.png"))
                    {
                        string oldPath = $"{path}/bg.png";
                        string newPath = $"{imageLocation}/UI_Jacket_{Program.CompensateZero(id.ToString())}.png";
                        Console.WriteLine("Located image at: {0}", oldPath);
                        if (File.Exists(newPath) && !OverwriteDestination) Console.WriteLine("File already located at {0} and skipped because not overwriting", newPath);
                        else File.Copy(oldPath, newPath, OverwriteDestination);
                        Console.WriteLine("Copied image to: {0}", newPath);
                    }
                    if (File.Exists($"{path}/pv.mp4"))
                    {
                        string oldPath = $"{path}/pv.mp4";
                        string newPath = $"{bgaLocation}/{Program.CompensateZero(id.ToString())}.mp4";
                        Console.WriteLine("Located PV at: {0}", oldPath);
                        if (File.Exists(newPath) && !OverwriteDestination) Console.WriteLine("File already located at {0} and skipped because not overwriting", newPath);
                        else File.Copy(oldPath, newPath, OverwriteDestination);
                        Console.WriteLine("Copied PV to: {0}", newPath);
                    }
                    if (File.Exists($"{path}/mv.mp4")) {
                        string oldPath = $"{path}/mv.mp4";
                        string newPath = $"{bgaLocation}/{Program.CompensateZero(id.ToString())}.mp4";
                        Console.WriteLine("Located MV at: {0}", oldPath);
                        if (File.Exists(newPath) && !OverwriteDestination) Console.WriteLine("File already located at {0} and skipped because not overwriting", newPath);
                        else File.Copy(oldPath, newPath, OverwriteDestination);
                        Console.WriteLine("Copied MV to: {0}", newPath);
                    }
                    if (File.Exists($"{path}/track.mp3")) {
                        string oldPath = $"{path}/track.mp3";
                        string newPath = $"{soundLocation}/music{Program.CompensateZero(id.ToString())}.mp3";
                        Console.WriteLine("Located track at: {0}", oldPath);
                        if (File.Exists(newPath) && !OverwriteDestination) Console.WriteLine("File already located at {0} and skipped because not overwriting", newPath);
                        else File.Copy(oldPath, newPath, OverwriteDestination);
                        Console.WriteLine("Copied track to: {0}", newPath);
                    }
                }
                else Console.WriteLine("{0} is not a chart folder",path);
                Console.WriteLine();
            }
            return Success;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Program cannot proceed becasue of following error returned: \n{0}", ex.GetType());
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            Console.ReadKey();
            return Failed;
        }
    }
}
