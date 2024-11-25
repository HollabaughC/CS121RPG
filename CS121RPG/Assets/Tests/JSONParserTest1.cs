#if UNITY_TEST_RUNNER
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JSONParserTest1
{
    private GameObject parser;
    private JSONQuestionParser JQP;
    [SetUp]
    public void Setup() {
        parser = new GameObject();
        JQP = parser.AddComponent<JSONQuestionParser>();
    }
    [TearDown]
    public void Cleanup() {
        Object.Destroy(parser);
    }
    // A Test behaves as an ordinary method
    [Test]
    public void JSONParserTest1SimplePasses()
    {
                //Check to see if it is grabbing all of the questions
        Assert.AreEqual(5, JQP.data.unit[0].question.Count);
        //Check if it can grab the answers correctly
        Assert.AreEqual("echo 'Hello, World!'", JQP.data.unit[0].question[0].answers[0]);
        //Check if it can grab other answers than the first
        Assert.AreEqual(".pt", JQP.data.unit[0].question[3].answers[2]);
        //Check if it cna grab the correct answer
        Assert.AreEqual(1, JQP.data.unit[0].question[1].correct);
        //Check if it can grab the hint
        Assert.AreEqual("The language does not require compilation before execution.", JQP.data.unit[0].question[2].hint);
        //Checking the same information but with different Units.
        Assert.AreEqual(5, JQP.data.unit[2].question.Count);
        Assert.AreEqual("<class 'str'>", JQP.data.unit[1].question[2].answers[3]);
        Assert.AreEqual("int", JQP.data.unit[3].question[3].answers[0]);
        Assert.AreEqual(3, JQP.data.unit[1].question[1].correct);
        Assert.AreEqual("Use the single equals sign for assignment.", JQP.data.unit[3].question[2].hint);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator JSONParserTest1WithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
#endif