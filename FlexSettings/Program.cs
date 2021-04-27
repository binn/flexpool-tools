using CommandLine;
using FlexSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Flexpool.Settings
{
    public class Program
    {
        private static bool _debug = false;
        private static readonly HttpClient _client = new();
        private const string _endpoint = "https://api.flexpool.io/v2/miner/payoutSettings";

        public static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CommandLineOptions, ConfigCommandLineOptions>(args);
            return result.MapResult(
                (CommandLineOptions opts) => TryRun(a => CommandLineInput(opts)),
                (ConfigCommandLineOptions opts) => TryRun(a => ConfigurationFileInput(opts)),
                errs => 1);
        }

        public static int Run(FlexOptions opts)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Testing settings once before running agent... (interval: every " + (opts.Interval / 1000) + "s)");
            Console.ResetColor();

            var res = SendRequest(opts);
            if (!res.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("request reject: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write((int)res.StatusCode);
                Console.ResetColor();

                Console.WriteLine(" - " + res.Content.ReadAsStringAsync().Result);

                throw new SettingsPushException();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine((int)res.StatusCode + " OK");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("request successful...starting agent");
                Console.ResetColor();
                Console.WriteLine(".\n.\n.");
            }

            int attempts = 0;
            while (true)
            {
                Thread.Sleep(opts.Interval);

                var response = SendRequest(opts);
                if (!response.IsSuccessStatusCode)
                {
                    attempts++;
                    if (attempts >= 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("stopping agent: ");
                        Console.ResetColor();
                        Console.WriteLine("failed to push settings to flexpool");

                        throw new Exception("server rejected request mid-agent");
                    }
                }
                else attempts = 0;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[log] pushed static settings @ " + DateTime.Now.ToString());
                Console.ResetColor();
                Console.Write(" (");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write((int)response.StatusCode + " OK");
                Console.ResetColor();
                Console.WriteLine(")");
            }
        }

        public static HttpResponseMessage SendRequest(FlexOptions opts)
        {
            string url = _endpoint + "?" + opts.ToString();
            return _client.PutAsync(url, null).Result;
        }

        public static int CommandLineInput(CommandLineOptions options)
        {
            _debug = options.Debug;
            var opts = new FlexOptions()
            {
                IP = options.IP,
                Address = options.Address,
                Interval = options.Interval,
                GasLimit = (long)options.Gas,
                PayoutLimit = (long)(options.Payout * 10e17)
            };

            return TryRun(a => Run(opts));
        }

        public static int ConfigurationFileInput(ConfigCommandLineOptions options)
        {
            _debug = options.Debug;
            if (!File.Exists(options.ConfigurationFileLocation))
                throw new FileNotFoundException("Configuration file '" + options.ConfigurationFileLocation + "'does not exist!");

            Dictionary<string, string> conf = File.ReadAllLines(options.ConfigurationFileLocation)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Where(x => !x.StartsWith("#"))
                .Select(x => x.Trim())
                .ToDictionary(
                    x => x.Split('=')[0],
                    y => string.Join('=', y.Split('=').Skip(1))
                 );

            if (!double.TryParse(conf["payout_eth_limit"], out double payout))
                throw new ArgumentException("invalid payout limit provided");

            if (!long.TryParse(conf["payout_gas_limit"], out long gas))
                throw new ArgumentException("invalid gas limit provided");

            if (!int.TryParse(conf["push_interval"], out int interval))
                throw new ArgumentException("invalid interval provided");

            var opts = new FlexOptions()
            {
                PayoutLimit = (long)(payout * 10e17),
                IP = conf["worker_ip_address"],
                Address = conf["eth_address"],
                Interval = interval,
                GasLimit = gas
            };

            return TryRun(a => Run(opts));
        }

        public static int TryRun(Func<object, int> a)
        {
            try
            {
                return a(null);
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[ERR] ");
                Console.ResetColor();

                Console.WriteLine(_debug ? exception.ToString() : exception.Message);
                return 1;
            }
        }
    }
}
