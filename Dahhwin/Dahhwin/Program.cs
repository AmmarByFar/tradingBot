using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Dahhwin.Models;

namespace Dahhwin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help();
                return;
            }

            if(args.Length > 1)
            {
                Console.Write("Dahhwin can only accept one argument. Use 'help' to see available commands.");
            }
  

            if (args[0] == "--help")
            {
                Help();
                return;
            }

            if (args[0] == "--start")
            {
                StartBots();
                return;
            }

            if (args[0] == "--status")
            {
                Console.Write("'--status' command not yet implemented.\n");
                return;
            }

        }

        private static void StartBots()
        {
            //Gather prelimiary data ranges to generate the bots with

            Console.WriteLine("How many instances of hummingbot would you like to run? ");
            int numberOfInstances = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

            Console.WriteLine("Enter a range for randomizing bid value:");
            Console.WriteLine("Lower bid limit: ");
            float lowerLimitBid = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
            Console.WriteLine("Upper bid limit: ");
            float upperLimitBid = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

            Console.WriteLine("Enter a range for randomizing ask value.");
            Console.WriteLine("Lower ask limit: ");
            float lowerLimitAsk = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
            Console.WriteLine("Upper ask limit: ");
            float upperLimitAsk = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

            Console.WriteLine("Enter a range for randomizing OrderRefreshTime value.");
            Console.WriteLine("Lower OrderRefreshTime limit: ");
            int lowerOrderRefreshTime = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
            Console.WriteLine("Upper OrderRefreshTime limit: ");
            int upperOrderRefreshTime = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

            Console.WriteLine("Enter size of order in BTC: ");
            float btcOrderSize = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

            Console.WriteLine("Creating docker instances...");

            for (int i = 0; i < numberOfInstances; i++)
            {
                //Generate random values to feed to bot creator
                float bid = GenerateFloatValue(lowerLimitBid, upperLimitBid);
                float ask = GenerateFloatValue(lowerLimitAsk, upperLimitAsk);
                int timeToRefresh = new Random().Next(lowerOrderRefreshTime, upperOrderRefreshTime);

                string instanceName = "hummingbotInstance" + i;

                Console.Write("Instance Name: " + instanceName +
                            "\nBid Value: " + bid +
                            "\nAsk Value: " + ask +
                            "\nRefresh time: " + timeToRefresh);

                ConfigureBot(instanceName, bid, ask, timeToRefresh, btcOrderSize);
            }
        }

        public static void ConfigureBot(string instanceName, float bid, float ask, int timeToRefresh, float orderSize)
        {
            //Create Directories
            ExecuteCommand("mkdir ../tradingBot/instances/" + instanceName);
            ExecuteCommand("mkdir ../tradingBot/instances/" + instanceName + "/hummingbot_conf");
            ExecuteCommand("mkdir ../tradingBot/instances/" + instanceName + "/hummingbot_logs");
            ExecuteCommand("mkdir ../tradingBot/instances/" + instanceName + "/hummingbot_data");
            ExecuteCommand("mkdir ../tradingBot/instances/" + instanceName + "/hummingbot_scripts");
            ExecuteCommand("mkdir ../instances/" + instanceName + "/hummingbot_certs");

            //Copy neccessary .yml and .json files from existing hummingbot installation
            //to /hummingbot_conf folder
            ExecuteCommand("cp defaultConfFiles/* ../instances/" + instanceName + "/hummingbot_conf");

            //Create .yml configuration file and place it in config 
            StrategyConfig conf = new StrategyConfig
            {
                bid_spread = bid.ToString(),
                ask_spread = ask.ToString(),
                order_amount = orderSize.ToString(),
                order_refresh_time = timeToRefresh.ToString()
            };

            using (TextWriter tw = new StreamWriter("../instances/" + instanceName + "/hummingbot_conf/conf_generated_strategy.yml"))
            {
                foreach (var property in conf.GetType().GetProperties())
                {
                    tw.WriteLine(property.Name + ": " + property.GetValue(conf));
                }
            }

            //Create run command inputing all variables required for autostart
            ExecuteCommand("docker run -d --name " + instanceName + " --network host " +
                "--mount \"type=bind,source=/home/ammar/dahhwin/instances/" + instanceName + "/hummingbot_files/hummingbot_conf,destination=/conf/\" " +
                "--mount \"type=bind,source=/home/ammar/dahhwin/instances/" + instanceName + "/hummingbot_logs,destination=/logs/\" " +
                "--mount \"type=bind,source=/home/ammar/dahhwin/instances/" + instanceName + "/hummingbot_data,destination=/data/\" " +
                "--mount \"type=bind,source=/home/ammar/dahhwin/instances/" + instanceName + "/hummingbot_scripts,destination=/scripts/\" " +
                "--mount \"type=bind,source=/home/ammar/dahhwin/instances/" + instanceName + "/hummingbot_certs,destination=/certs/\" " +
                "--env HTTP_PROXY=\"http://192.168.1.12:3128\" " +
                "--env HTTPS_PROXY=\"https://192.168.1.12:3128\" " + 
                "-e STRATEGY=\"pure_market_making\" " +
                "-e CONFIG_FILE_NAME=\"/home/ammar/dahhwin/instances/hummingbot_files/hummingbot_conf/conf_generated_strategy.yml\" " +
                "-e CONFIG_PASSWORD=\"ama4Life199@\" " +
                "coinalpha/hummingbot:latest");
        }

        public static void Help()
        {
            Console.WriteLine("\nUsage: dotnet Dahhwin.dll [command]\n" + 
                "\nCommands:" + 
                "\n\t--help\t\tDisplays this help message." +
                "\n\t--start\t\tStart the application." +
                "\n\t--status\t\tPrint the current status of all running bots." +
                "\n\t--stop\t\tStop all instances of running docker containers.");
        }

        public static string ExecuteCommand(string command)
        {
            //Sanitize command and escape quotes
            string sanitizedCommand = command.Replace("\"", "\\\"");
            //Console.WriteLine(sanitizedCommand);
            string result = "";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = "-c \" " + sanitizedCommand + " \"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                result += process.StandardOutput.ReadToEnd();
                result += process.StandardError.ReadToEnd();

                process.WaitForExit();
            }

            Console.WriteLine(result);

            return result;
        }


        //Method I got from stackoverflow cause im dumb and lazy
        //it genereates float value between 2 floats. 
        public static float GenerateFloatValue(float min, float max)
        {
            if (min >= max)
            {
                throw new Exception("min value is greater than max value");
            }
            Random randomizer = new Random();
            float randomFloat = (float)randomizer.NextDouble();
            return min + randomFloat * max - randomFloat * min;
        }
    }
}
