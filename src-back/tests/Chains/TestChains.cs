using Blc.ChainBuilder;
using Blc.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Chains
{
    public class ProcessTrace
    {
        public static string TraceMessage = "";
    }

    public class DisposeTrace
    {
        public static string TraceMessage = "";
    }

    public class TestChains
    {
        [Fact]
        public async Task TestSimpleChain()
        {
            DisposeTrace.TraceMessage = "";

            var result = await new BP().RunAsync("request");

            Assert.Equal("request -> Step1Result -> Step2Result -> Step3Result", result);
            Assert.Equal("/Step1Disposed/Step3Disposed", DisposeTrace.TraceMessage);
        }

        [Fact]
        public async Task TestOnFailureChain()
        {
            var result = await new BP2().RunAsync("request");
            Assert.Equal("request -> Step1Result -> Step2Result -> Step3Result -> Step5Result", result);
        }

        [Fact]
        public async Task TestFailure()
        {
            ProcessTrace.TraceMessage = "";

            await Assert.ThrowsAsync<TestException>(()=> new BP3().RunAsync("request"));

            Assert.Equal("/Step1Done/Step2Done/Step3Done/Step4Fail/Step3Cancel/Step2Cancel/Step1Cancel", ProcessTrace.TraceMessage);
        }

        [Fact]
        public async Task TestFailureOnFailure()
        {
            ProcessTrace.TraceMessage = "";

            await Assert.ThrowsAsync<TestException>(() => new BP4().RunAsync("request"));

            Assert.Equal("/Step1Done/Step2Done/Step3Done/Step4Fail/Step5Done/Step6Fail/Step3Cancel/Step2Cancel/Step1Cancel", ProcessTrace.TraceMessage);
        }

        [Fact]
        public async Task TestSwitchers()
        {
            var result = await new BP5().RunAsync("request");
            Assert.Equal("request -> Step1Result -> StepSwitcherResult:NoSwitch -> Step2_NoSwitchResult", result);

            var result1 = await new BP5().RunAsync("request1");
            Assert.Equal("request1 -> Step1Result -> StepSwitcherResult:Switch1 -> Step2_Switch1Result", result1);

            var result2 = await new BP5().RunAsync("request2");
            Assert.Equal("request2 -> Step1Result -> StepSwitcherResult:Switch2 -> Step2_Switch2Result", result2);
        }

        #region == Test Data ==

        public class Step1 : IBusinessProcessStep<string>,  IBusinessProcessStepCancelable, IDisposable
        {
            string _request;

            public Step1(string request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step1Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step1Done";
                return await Task.FromResult($"{_request} -> Step1Result");
            }

            public void Dispose()
            {
                DisposeTrace.TraceMessage += "/Step1Disposed";
            }
        }


        public class Step2 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            string _request;

            public Step2(string request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step2Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step2Done";
                return await Task.FromResult($"{_request} -> Step2Result");
            }
        }

        public class Step3 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable, IDisposable
        {
            string _request;

            public Step3(string request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step3Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step3Done";
                return await Task.FromResult($"{_request} -> Step3Result");
            }

            public void Dispose()
            {
                DisposeTrace.TraceMessage += "/Step3Disposed";
            }
        }

        public class TestException : Exception
        {
            public TestException(string msg) : base(msg) { }
        }

        public class Step4 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            string _request;

            public Step4(string request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step4Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step4Fail";
                await Task.CompletedTask;
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

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step5Done";
                return await Task.FromResult($"{_request} -> Step5Result");
            }
        }

        public class Step6 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            string _request;

            public Step6(string request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step6Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step6Fail";
                await Task.CompletedTask;
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

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/StepSwitcherCancel";
                await Task.CompletedTask;
            }

            public async Task<SwitcherModel> RunAsync()
            {
                ProcessTrace.TraceMessage += "/StepSwitcherDone";
                var result = Switcher.NoSwitch;

                if (_request.StartsWith("request1"))
                {
                    result = Switcher.Switch1;
                }
                else if (_request.StartsWith("request2"))
                {
                    result = Switcher.Switch2;
                }

                return await Task.FromResult(new SwitcherModel()
                {
                    Request = $"{_request} -> StepSwitcherResult:{result.ToString("G")}",
                    SwitchResult = result
                });
            }
        }

        public class Step2_Switch1 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            SwitcherModel _request;

            public Step2_Switch1(SwitcherModel request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_Switch1Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_Switch1Done";
                return await Task.FromResult($"{_request.Request} -> Step2_Switch1Result");
            }
        }

        public class Step2_Switch2 : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            SwitcherModel _request;

            public Step2_Switch2(SwitcherModel request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_Switch2Cancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_Switch2Done";
                return await Task.FromResult($"{_request.Request} -> Step2_Switch2Result");
            }
        }

        public class Step2_NoSwitch : IBusinessProcessStep<string>, IBusinessProcessStepCancelable
        {
            SwitcherModel _request;

            public Step2_NoSwitch(SwitcherModel request)
            {
                _request = request;
            }

            public async Task CancelAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_NoSwitchCancel";
                await Task.CompletedTask;
            }

            public async Task<string> RunAsync()
            {
                ProcessTrace.TraceMessage += "/Step2_NoSwitchDone";
                return await Task.FromResult($"{_request.Request} -> Step2_NoSwitchResult");
            }
        }

        public class BP : IBusinessProcess<string, string>
        {
            public async Task<string> RunAsync(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request)
                    .Then<Step2, string>()
                    .Then<Step3, string>()
                    .RunAsync();
                return await t;
            }
        }

        public class BP2 : IBusinessProcess<string, string>
        {
            public async Task<string> RunAsync(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request)
                    .Then<Step2, string>()
                    .Then<Step3, string>()
                    .Then<Step4, string>()
                        .OnException<TestException, string, Step5>(step3Result => BusinessLogicChain<string>.New<Step5, string>(step3Result))
                    .RunAsync();
                return await t;
            }
        }

        public class BP3 : IBusinessProcess<string, string>
        {
            public async Task<string> RunAsync(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request) // cancel
                    .Then<Step2, string>() // cancel
                    .Then<Step3, string>() // cancel
                    .Then<Step4, string>() // fail
                    .Then<Step5, string>() // not execute
                    .RunAsync();
                return await t;
            }
        }

        public class BP4 : IBusinessProcess<string, string>
        {
            public async Task<string> RunAsync(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request) // cancel
                    .Then<Step2, string>() // cancel
                    .Then<Step3, string>() // cancel
                    .Then<Step4, string>() // fail
                        .OnException<TestException, string, Step6>(step3Result =>
                            BusinessLogicChain<string>
                            .New<Step5, string>(step3Result) // cancel
                            .Then<Step6, string>() // fail
                        )
                    .RunAsync();
                return await t;
            }
        }

        public class BP5 : IBusinessProcess<string, string>
        {
            public async Task<string> RunAsync(string request)
            {
                var t = BusinessLogicChain<string>
                    .New<Step1, string>(request)
                    .Then<StepSwitcher, SwitcherModel>()
                        .OnStepResult(sm => sm.SwitchResult == Switcher.Switch1, sm => BusinessLogicChain<string>.New<Step2_Switch1, string>(sm))
                        .OnStepResult(sm => sm.SwitchResult == Switcher.Switch2, sm => BusinessLogicChain<string>.New<Step2_Switch2, string>(sm))
                    .Then<Step2_NoSwitch, string>()// no call
                    .RunAsync();
                return await t;
            }
        } 
        #endregion

    }
}
