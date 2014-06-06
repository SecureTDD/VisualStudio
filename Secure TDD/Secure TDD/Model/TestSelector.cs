using Secure_TDD.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Secure_TDD.Model
{
    public class TestSelector
    {
        # region Data Members

        List<string> _configFilePaths;
        List<TestConfiguration> _testConfigurations;

        # endregion Data Members

        # region Singleton

        private static TestSelector _instance;
        public static TestSelector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TestSelector();
                }

                return _instance;
            }
        }

        # endregion Singleton

        # region Ctor

        private TestSelector()
        {
            _configFilePaths = new List<string>();
            _testConfigurations = new List<TestConfiguration>();

            if (Directory.Exists(TestConfiguration.ConfigFilesDirectory))
            {
                var configFilePaths = Directory.GetFiles(TestConfiguration.ConfigFilesDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                _configFilePaths.AddRange(configFilePaths);
            }

            // Load all the configuration tests.
            LoadConfigFiles();
        }

        # endregion Ctor

        # region Methods

        /// <summary>
        /// Gets list of configuration tests that matches to the given rule.
        /// </summary>
        /// <param name="p_role"></param>
        /// <returns>List of configuration tests</returns>
        public List<TestConfiguration> GetTestConfigurations(Rule p_role)
        {
            List<TestConfiguration> configurations = new List<TestConfiguration>();
            var matchingConfigs = _testConfigurations.Where(config => config.Conditions.Any(condition => p_role.HasFlag(condition.Key) && condition.Value));
            if (matchingConfigs != null)
            {
                configurations = matchingConfigs.ToList();
            }

            return configurations;
        }

        /// <summary>
        /// Loads to a property, all the known configuration tests.
        /// </summary>
        private void LoadConfigFiles()
        {
            if (_configFilePaths.Any())
            {
                foreach (var path in _configFilePaths)
                {
                    var config = DeserializeFromXML(path);
                    if (config != null)
                    {
                        _testConfigurations.Add(config);
                    }
                }
            }
        }

        /// <summary>
        /// Deserialize a TestConfiguration object from an XML file.
        /// </summary>
        /// <param name="p_path"></param>
        /// <returns>Deserialized TestConfiguration object</returns>
        static TestConfiguration DeserializeFromXML(string p_path)
        {
            TestConfiguration configuration = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(TestConfiguration));

            if (File.Exists(p_path))
            {
                using (TextReader textReader = new StreamReader(p_path))
                {
                    configuration = (TestConfiguration)deserializer.Deserialize(textReader);
                }
            }
            else
            {
                Console.WriteLine("The path {0} does not exists.", p_path);
            }

            return configuration;
        }

        # endregion Methods
    }
}
