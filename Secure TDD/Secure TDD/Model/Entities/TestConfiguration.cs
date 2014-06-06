using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Secure_TDD.Model.Entities
{
    [XmlRoot("SecureTDDEngineRule")]
    public class TestConfiguration
    {
        public static string ConfigFilesDirectory { get; set; }

        public TestConfiguration()
        {

        }

        public string FilePath { get; set; }

        [XmlArray("AffectedParameters")]
        [XmlArrayItem("RuleAffectedParameters")]
        public List<TestParameter> TestParameters { get; set; }

        [XmlArray("Conditions")]
        [XmlArrayItem("RuleCondition")]
        public List<RuleCondition> Conditions { get; set; }

        [XmlElement]
        public TestCode TestCode { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }

    public class TestParameter
    {
        [XmlAttribute("Type")]
        public ParameterType ParameterType { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }

    public class RuleCondition
    {
        public RuleCondition()
        {

        }

        [XmlAttribute]
        public Rule Key { get; set; }

        [XmlAttribute]
        public bool Value { get; set; }
    }

    [Flags]
    public enum Rule
    {
        [XmlEnum]
        PresentatinoLayer,
        [XmlEnum]
        ContainsUserInput,
        [XmlEnum]
        ContainsUserOutput
    }

    public enum ParameterType
    {
        [XmlEnum]
        Class,
        [XmlEnum]
        ConstractorParam,
        [XmlEnum]
        Method,
        [XmlEnum]
        FirstParameters,
        [XmlEnum]
        SecondParameters,
        [XmlEnum]
        TestingParameterIndex
    }

    public class TestCode
    {
        [XmlElement]
        public string Code { get; set; }
    }
}
