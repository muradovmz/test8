using System.Collections.Generic;
using System.Threading.Tasks;
using ConfIT;
using ConfIT.Extension;
using ConfIT.Util;
using Newtonsoft.Json.Linq;
using {{cookiecutter.ProjectName}}.ComponentTests.SetUp;
using Xunit;
using Xunit.Abstractions;

namespace {{cookiecutter.ProjectName}}.ComponentTests
{
    public class {{cookiecutter.ProjectName}}ComponentTests : BaseTest, IClassFixture<TestSuiteFixture>
    {
        public {{cookiecutter.ProjectName}}ComponentTests(TestSuiteFixture fixture, ITestOutputHelper output)
            : base(
                fixture.TestHttpClient,
                fixture.SuiteConfig,
                fixture.TestProcessFactory,
                new TestOutputLogger(output),
                fixture.Filter)
        {
        }

        /// <summary>
        /// Entry point for testApi. We can run testApi in two ways
        /// TO run by files, we can add
        /// --> [MemberData(nameof(GetTestCases), "bank.json")]
        /// --> [MemberData(nameof(GetTestCases), "{{cookiecutter.ProjectName}}.json")]
        ///  TO run tests by folder, we can add
        /// --> [MemberData(nameof(GetTestCasesForFolder))]
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="test"></param>
        /// <param name="mocks"></param>
        [Theory]
        [MemberData(nameof(GetTestCasesForFolder), "TestCase")]
        public async Task ExecuteTest(string testName, JToken test) =>
            await Execute(testName, test.ToTestCase(null, null));


        /// <summary>
        /// Use this to read tests from a single file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IEnumerable<object[]> GetTestCases(string fileName) =>
            TestReader.GetTestsForAFile("TestCase", fileName);

        /// <summary>
        /// Use this to read tests from a folder 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> GetTestCasesForFolder(string folder) =>
            TestReader.GetTestsForAFolder(folder);
    }
}