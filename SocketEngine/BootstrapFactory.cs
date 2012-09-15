﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine.Configuration;
using SuperSocket.SocketBase.Config;
using System.Configuration;

namespace SuperSocket.SocketEngine
{
    /// <summary>
    /// Bootstrap Factory
    /// </summary>
    public static class BootstrapFactory
    {
        /// <summary>
        /// Creates the bootstrap.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap(IConfigurationSource config)
        {
            if (config.Isolation == IsolationMode.AppDomain)
                return new AppDomainBootstrap(config);
            else
                return new DefaultBootstrap(config);
        }

        /// <summary>
        /// Creates the bootstrap from app configuration's socketServer section.
        /// </summary>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap()
        {
            var configSection = ConfigurationManager.GetSection("superSocket");

            if (configSection == null)//to keep compatible with old version
                configSection = ConfigurationManager.GetSection("socketServer");

            return CreateBootstrap(configSection as IConfigurationSource);
        }

        /// <summary>
        /// Creates the bootstrap.
        /// </summary>
        /// <param name="configSectionName">Name of the config section.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrap(string configSectionName)
        {
            var configSource = ConfigurationManager.GetSection(configSectionName) as IConfigurationSource;

            if (configSource == null)
                throw new ArgumentException("Invalid section name.");

            return CreateBootstrap(configSource);
        }

        /// <summary>
        /// Creates the bootstrap from configuration file.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <returns></returns>
        public static IBootstrap CreateBootstrapFromConfigFile(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;

            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var configSource = config.GetSection("socketServer") as IConfigurationSource;

            return CreateBootstrap(configSource);
        }
    }
}
