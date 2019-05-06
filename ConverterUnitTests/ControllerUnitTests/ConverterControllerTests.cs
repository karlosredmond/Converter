using Converter.Common;
using Converter.Controllers;
using Xunit;

namespace ConverterUnitTests.ControllerTests
{
    public class ConverterControllerTests
    {
        ConverterController _sut = new ConverterController(new Utility(new MapPeriod()));

        [Fact]
        public void CheckPreMatch_OK()
        {
            var sutResult = _sut.Get("[PM] 0:00.000");
            Assert.Equal("00:00 - PRE_MATCH", sutResult);
        }

        [Fact]
        public void CheckFirstHalf_OK()
        {
            var sutResult = _sut.Get("[H1] 0:15.025");
            Assert.Equal("00:15 - FIRST_HALF", sutResult);
        }

        [Fact]
        public void CheckFirstHalfRoundUp_OK()
        {
            var sutResult = _sut.Get("[H1] 3:07.513");
            Assert.Equal("03:08 - FIRST_HALF", sutResult);
        }

        [Fact]
        public void CheckFirstHalfOverTimeNoRoundUp_OK()
        {
            var sutResult = _sut.Get("[H1] 45:00.001");
            Assert.Equal("45:00 +00:00 - FIRST_HALF", sutResult);
        }

        [Fact]
        public void CheckFirstHalfOverTimeWithRoundUp_OK()
        {
            var sutResult = _sut.Get("[H1] 46:15.752");
            Assert.Equal("45:00 +01:16 - FIRST_HALF", sutResult);
        }

        [Fact]
        public void CheckHalfTime_OK()
        {
            var sutResult = _sut.Get("[HT] 45:00.000");
            Assert.Equal("45:00 - HALF_TIME", sutResult);
        }

        [Fact]
        public void CheckSecondHalfRoundUp_OK()
        {
            var sutResult = _sut.Get("[H2] 45:00.500");
            Assert.Equal("45:01 - SECOND_HALF", sutResult);
        }

        [Fact]
        public void CheckSecondHalfOvertimeRoundUp_OK()
        {
            var sutResult = _sut.Get("[H2] 90:00.908");
            Assert.Equal("90:00 +00:01 - SECOND_HALF", sutResult);
        }

        [Fact]
        public void CheckFullTimeNoOvertime_OK()
        {
            var sutResult = _sut.Get("[FT] 90:00.000");
            Assert.Equal("90:00 +00:00 - FULL_TIME", sutResult);
        }

        [Fact]
        public void CheckTimeFormat_INVALID()
        {
            var sutResult = _sut.Get("90:00");
            Assert.Equal("INVALID", sutResult);
        }

        [Fact]
        public void CheckPeriodFormat_INVALID()
        {
            var sutResult = _sut.Get("[H3] 90:00.000 ");
            Assert.Equal("INVALID", sutResult);
        }

        [Fact]
        public void CheckNegativeTimeFormat_INVALID()
        {
            var sutResult = _sut.Get("[PM] -1000.000");
            Assert.Equal("INVALID", sutResult);
        }

        [Fact]
        public void CheckStringFormat_INVALID()
        {
            var sutResult = _sut.Get("FOO");
            Assert.Equal("INVALID", sutResult);
        }
    }
}
