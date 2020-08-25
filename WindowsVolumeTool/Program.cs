using System;
using System.Globalization;

namespace AudioTool
{
    class Program
    {
        private static readonly IFormatProvider US_FORMAT = new CultureInfo("en-US", false);

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                PrintHelpPage();
                return;
            }

            switch (args[0].Trim())
            {
                case "help":
                    PrintHelpPage();
                    break;

                case "version":
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    Version version = assembly.GetName().Version;

                    Console.WriteLine("audio-tool v" + version.Major + "." + version.Minor);
                    break;

                case "list":
                    ListAllAudioSessions();
                    break;

                case "get":
                    if (AssertNumberOfArguments(args, 2))
                    {
                        Console.WriteLine(GetVolume(args[1]).ToString(US_FORMAT));
                    }
                    break;

                case "add":
                    if (AssertNumberOfArguments(args, 3))
                    {
                        float? valueToAdd = ValidatePercentageInput(args[1], true);
                        if (valueToAdd.HasValue)
                        {
                            AddVolume(args[2], valueToAdd.Value);
                        }
                    }
                    break;

                case "set":
                    if (AssertNumberOfArguments(args, 3))
                    {
                        float? valueToSet = ValidatePercentageInput(args[1]);
                        if (valueToSet.HasValue)
                        {
                            SetVolume(args[2], valueToSet.Value);
                        }
                    }
                    break;

                case "mute":
                    if (AssertNumberOfArguments(args, 2))
                    {
                        Mute(args[1]);
                    }
                    break;

                case "unmute":
                    if (AssertNumberOfArguments(args, 2))
                    {
                        Unmute(args[1]);
                    }
                    break;

                case "toggle":
                    if (AssertNumberOfArguments(args, 2))
                    {
                        ToggleMute(args[1]);
                    }
                    break;

                default:
                    Console.WriteLine("Error: Unknown command \"" + args[0].Trim() + "\"");
                    break;
            }

            return;
        }

        private static void PrintHelpPage()
        {
            string name = System.AppDomain.CurrentDomain.FriendlyName;

            Console.WriteLine("Usage: " + name + " <command> [options]\n\n" +
                "  " + name + " list                            List available audio sessions\n" +
                "  " + name + " help                            Print this help page\n" +
                "  " + name + " set <percentage> <name>         Set volume of the session to the given percentage\n" +
                "  " + name + " add [+|-]<percentage> <name>    Add the given percentage to the volume of the session\n" +
                "  " + name + " mute <name>                     Mute the session\n" +
                "  " + name + " unmute <name>                   Unmute the session\n" +
                "  " + name + " toggle <name>                   Mute/unmute the session\n\n" +
                "  Percentage values have to be given as a number between 0.0 (0%) and 1.0 (100%).");
        }

        private static bool AssertNumberOfArguments(string[] args, int required)
        {
            if (args.Length < required)
            {
                if (args.Length > 0)
                {
                    Console.WriteLine("Error: Only " + args.Length + " of " + required + " required arguments for command \"" + args[0] + "\" given.");
                }
                else
                {
                    Console.WriteLine("Error: Only " + args.Length + " of " + required + " required arguments given.");
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private static float? ValidatePercentageInput(string input, bool canBeNegative = false)
        {
            try
            {
                float value = Convert.ToSingle(input, US_FORMAT);

                if (canBeNegative && value < -1.0f || value > 1.0f)
                {
                    Console.WriteLine("Value is not the range from -1.0 to 1.0.");
                    return null;
                }
                else if (!canBeNegative && value < 0.0f || value > 1.0f)
                {
                    Console.WriteLine("Value is not the range from 0.0 to 1.0.");
                    return null;
                }

                return value;
            }
            catch (FormatException)
            {
                Console.WriteLine("Value is not a valid floating point number.");
                return null;
            }
        }

        private static AudioSession GetSession(string name)
        {
            foreach (AudioSession session in AudioUtilities.GetAllSessions())
            {
                if (session.Process != null && session.Process.ProcessName == name)
                {
                    return session;
                }
            }

            Console.WriteLine("No such app");
            return null;
        }

        private static void Mute(string name)
        {
            AudioSession session = GetSession(name);
            session.Mute = true;
        }

        private static void Unmute(string name)
        {
            AudioSession session = GetSession(name);
            session.Mute = false;
        }

        private static void ToggleMute(string name)
        {
            AudioSession session = GetSession(name);
            session.Mute = !session.Mute;
        }

        private static void ListAllAudioSessions()
        {
            foreach (AudioSession session in AudioUtilities.GetAllSessions())
            {
                if (session.Process != null)
                {
                    Console.WriteLine(session.Process.ProcessName);
                }
            }
        }

        private static float GetVolume(string name)
        {
            AudioSession session = GetSession(name);
            return session.Volume;
        }

        private static void SetVolume(string name, float amount)
        {
            AudioSession session = GetSession(name);
            session.Volume = amount;
        }

        private static void AddVolume(string name, float amount)
        {
            AudioSession session = GetSession(name);

            float newVolume = session.Volume + amount;

            if (newVolume > 1) newVolume = 1;
            if (newVolume < 0) newVolume = 0;

            session.Volume = newVolume;
        }
    }
}