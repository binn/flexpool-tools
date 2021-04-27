using CommandLine;

namespace FlexSettings
{
    [Verb("now", isDefault: true, HelpText = "Run via CLI arguments")]
    public class CommandLineOptions
    {
        [Option("address", Required = true, HelpText = "Ethereum wallet address")]
        public string Address { get; set; }

        [Option("ip", Required = true, HelpText = "IP address of majority of your workers")]
        public string IP { get; set; }

        [Option('i', "interval", Required = false, Default = 5000, HelpText = "Interval (in ms) where settings are pushed to Flexpool")]
        public int Interval { get; set; }

        [Option("payout", Required = false, Default = 0.5, HelpText = "Payout limit")]
        public double Payout { get; set; }

        [Option("gas", Required = false, Default = 85, HelpText = "Gas price limit")]
        public int Gas { get; set; }

        [Option('d', "debug", Required = false, Default = false, HelpText = "Enable debug logging (on exceptions)")]
        public bool Debug { get; set; }

     //   [Option('e', "email", Required = false, HelpText = "Notifications email")]
     //   public string Email { get; set; }
    }

    [Verb("conf", isDefault: false, HelpText = "Run via config file parameters")]
    public class ConfigCommandLineOptions
    {
        [Option("file", Required = true, Default = "flex.conf", HelpText = "Configuration file of your choice")]
        public string ConfigurationFileLocation { get; set; }

        [Option('d', "debug", Required = false, Default = false, HelpText = "Enable debug logging (on exceptions)")]
        public bool Debug { get; set; }
    }
}
