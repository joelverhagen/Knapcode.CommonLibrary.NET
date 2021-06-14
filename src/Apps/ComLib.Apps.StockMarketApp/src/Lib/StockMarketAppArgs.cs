﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommonLibrary;


namespace CommonLibrary.StockMarketApp
{
    /// <summary>
    /// Object that should recieve the command line arguments.
    /// </summary>
    public class StockMarketAppArgs
    {
        /// <summary>
        /// What environment to run this in.
        /// </summary>
        /// <remarks>This can also be inherited.
        /// e.g. prod,qa,dev. ( Prod inherits from Qa inherits from Dev ).</remarks>
        [Arg("env", "Environment to run in", typeof(string), true, "dev", "dev", "dev | qa | prod | prod,qa,dev")]
        public string Envrionment { get; set; }


        /// <summary>
        /// This is NOT required because if config is empty, the config
        /// file will be defaulted to {environment}.config.
        /// e.g. dev.config qa.config where "dev" and "qa" is the environment name above.
        /// </summary>
        /// <remarks>This can be multiple configs. And inherited.
        /// e.g. prod.config,qa.config,dev.config</remarks>
        [Arg("config", "Config file for environment", typeof(string), true, @"config\prod.config", "dev.config", "dev.config | qa.config | prod.config")]
        public string Config { get; set; }


        /// <summary>
        /// This is NOT required because if logfile is empty, the logfile
        /// file will be defaulted to {environment}.log.
        /// e.g. dev.log qa.log where "dev" and "qa" is the environment name above.
        /// </summary>
        [Arg("log", "Log file to write to", typeof(string), false, "{env}.log", "app.log", "app.log | myapp.log")]
        public string LogFile { get; set; }
        

        /// <summary>
        /// The business date to get stock market data for.
        /// </summary>
        [Arg("date", "The business date", typeof(int), true, "${today}", "${today}", "${today} | 05/12/2009", true, false, false)]
        public DateTime BusinessDate { get; set; }


        /// <summary>
        /// Whether or not to store the stock market data into internal business systems.
        /// This effectively is useful for testing purposes.
        /// </summary>
        [Arg("dryrun", "Flag to store", typeof(bool), false, false, "false", "true | false")]
        public bool DryRun { get; set; }


        /// <summary>
        /// Indicates where to get the market data from.
        /// Either "Bloomberg" or "Reuters"
        /// </summary>
        [Arg("source", "Data source to get market data from.", typeof(string), false, "Bloomberg", "Bloomberg", "Bloomberg | Reuters")] 
        public string DataSource { get; set; }


        /// <summary>
        /// How many symbols to process each time. e.g. 10 symbols per batch.
        /// </summary>
        [Arg(0, "Number of symbols to process at one time.", typeof(int), true, 10, "10", "10 | 20 | 30 etc.")]
        public string BatchSize { get; set; }
    }
}
