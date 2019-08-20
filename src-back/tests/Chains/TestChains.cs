using Blc.ChainBuilder;
using Blc.Interfaces;
using System;
using Xunit;

namespace Chains
{
    public static class TestFailureProcessTrace
    {
        public static string TraceMessage = "";
    }

    public class TestChains
    {
        [Fact]
        public void TestSimpleChain()
        {
            var result = new BP().Run("request");
            Assert.Equal("request -> Step1Result -> Step2Result -> Step3Result", result);
        }

        [Fact]
        public void TestOnFailureChain()
        {
            var result = new BP2().Run("request");
            Assert.Equal("request -> Step1Result -> Step2Result -> Step3Result -> Step5Result", result);
        }

        [Fact]
        public void TestFailure()
        {
            TestFailureProcessTrace.TraceMessage = "";

            Assert.Throws<TestException>(()=> new BP3().Run("request"));

            Assert.Equal("/Step1Done/Step2Done/Step3Done/Step4Fail/Step3Cancel/Step2Cancel/Step1Cancel", TestFailureProcessTrace.TraceMessage);
        }

        [Fact]
        public void TestFailureOnFailure()
        {
            TestFailureProcessTrace.TraceMessage = "";

            Assert.Throws<TestException>(() => new BP4().Run("request"));

            Assert.Equal("/Step1Done/Step2Done/Step3Done/Step4Fail/Step5Done/Step6Fail/Step5Cancel/Step3Cancel/Step2Cancel/Step1Cancel", TestFailureProcessTrace.TraceMessage);
        }

        [Fact]
        public void TestSwitchers()
        {
            var result = new BP5().Run("request");
            Assert.Equal("request -> Step1Result -> StepSwitcherResult:NoSwitch -> Step2_NoSwitchResult", result);

            var result1 = new BP5().Run("request1");
            Assert.Equal("request1 -> Step1Result -> StepSwitcherResult:Switch1 -> Step2_Switch1Result", result1);

            var result2 = new BP5().Run("request2");
            Assert.Equal("request2 -> Step1Result -> StepSwitcherResult:Switch2 -> Step2_Switch2Result", result2);
        }


        public class Step1 : IBusinessProcessStep<string>
        {
            string _request;

            public Step1(string request)
            {
                _request = request;
            }

            public  void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step1Cancel";
            }

            public  string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step1Done";
                return $"{_request} -> Step1Result";
            }
        }


        public class Step2 : IBusinessProcessStep<string>
        {
            string _request;

            public Step2(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2Done";
                return $"{_request} -> Step2Result";
            }
        }

        public class Step3 : IBusinessProcessStep<string>
        {
            string _request;

            public Step3(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step3Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step3Done";
                return $"{_request} -> Step3Result";
            }
        }

        public class TestException : Exception
        {
            public TestException(string msg) : base(msg) { }
        }

        public class Step4 : IBusinessProcessStep<string>
        {
            string _request;

            public Step4(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step4Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step4Fail";
                throw new TestException("Step4 fail");
            }
        }

        public class Step5 : IBusinessProcessStep<string>
        {
            string _request;

            public Step5(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step5Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step5Done";
                return $"{_request} -> Step5Result";
            }
        }

        public class Step6 : IBusinessProcessStep<string>
        {
            string _request;

            public Step6(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step6Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step6Fail";
                throw new TestException("Step6 fail");
            }
        }

        public enum Switcher
        {
            NoSwitch = 0,
            Switch1,
            Switch2
        }

        public class SwitcherModel
        {
            public string Request { get; set; }
            public Switcher SwitchResult { get; set; }
        }

        public class StepSwitcher : IBusinessProcessStep<SwitcherModel>
        {
            string _request;

            public StepSwitcher(string request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/StepSwitcherCancel";
            }

            public SwitcherModel Run()
            {
                TestFailureProcessTrace.TraceMessage += "/StepSwitcherDone";
                var result = Switcher.NoSwitch;

                if (_request.StartsWith("request1"))
                {
                    result = Switcher.Switch1;
                }
                else if (_request.StartsWith("request2"))
                {
                    result = Switcher.Switch2;
                }

                return new SwitcherModel()
                {
                    Request = $"{_request} -> StepSwitcherResult:{result.ToString("G")}",
                    SwitchResult = result
                };
            }
        }

        public class Step2_Switch1 : IBusinessProcessStep<string>
        {
            SwitcherModel _request;

            public Step2_Switch1(SwitcherModel request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_Switch1Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_Switch1Done";
                return $"{_request.Request} -> Step2_Switch1Result";
            }
        }

        public class Step2_Switch2 : IBusinessProcessStep<string>
        {
            SwitcherModel _request;

            public Step2_Switch2(SwitcherModel request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_Switch2Cancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_Switch2Done";
                return $"{_request.Request} -> Step2_Switch2Result";
            }
        }

        public class Step2_NoSwitch : IBusinessProcessStep<string>
        {
            SwitcherModel _request;

            public Step2_NoSwitch(SwitcherModel request)
            {
                _request = request;
            }

            public void Cancel()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_NoSwitchCancel";
            }

            public string Run()
            {
                TestFailureProcessTrace.TraceMessage += "/Step2_NoSwitchDone";
                return $"{_request.Request} -> Step2_NoSwitchResult";
            }
        }

        public class BP : IBusinessProcess<string, string>
        {
            public string Run(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1,string>(request)
                    .Then<Step2,string>()
                    .Then<Step3,string>()
                    .Run();
                return t;
            }
        }

        public class BP2 : IBusinessProcess<string, string>
        {
            public string Run(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request)
                    .Then<Step2, string>()
                    .Then<Step3, string>()
                    .Then<Step4, string>()
                        .OnException<TestException, Step5>(step3Result => BusinessLogicChain<string>.New<Step5, string>(step3Result))
                    .Run();
                return t;
            }
        }

        public class BP3 : IBusinessProcess<string, string>
        {
            public string Run(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request) // cancel
                    .Then<Step2, string>() // cancel
                    .Then<Step3, string>() // cancel
                    .Then<Step4, string>() // fail
                    .Then<Step5, string>() // not execute
                    .Run();
                return t;
            }
        }

        public class BP4 : IBusinessProcess<string, string>
        {
            public string Run(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request) // cancel
                    .Then<Step2, string>() // cancel
                    .Then<Step3, string>() // cancel
                    .Then<Step4, string>()
                        .OnException<TestException, Step6>(step3Result =>
                            BusinessLogicChain<string>
                            .New<Step5, string>(step3Result) // cancel
                            .Then<Step6, string>() // fail
                        )
                    .Run();
                return t;
            }
        }

        public class BP5 : IBusinessProcess<string, string>
        {
            public string Run(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request)
                    .Then<StepSwitcher, SwitcherModel>()
                        .OnStepResult(sm => sm.SwitchResult == Switcher.Switch1, sm => BusinessLogicChain<string>.New<Step2_Switch1, string>(sm))
                        .OnStepResult(sm => sm.SwitchResult == Switcher.Switch2, sm => BusinessLogicChain<string>.New<Step2_Switch2, string>(sm))
                    .Then<Step2_NoSwitch, string>()// no call
                    .Run();
                return t;
            }
        }


    }
}
