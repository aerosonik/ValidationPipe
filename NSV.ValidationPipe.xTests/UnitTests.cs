using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSV.ValidationPipe.xTests
{
    public class UnitTests
    {
        [Fact]
        public async Task Test_Sucsess()
        {
            var model = TestData.GetValidModel();
            var pipe = new TestValidationPipe();
            var result = await pipe.ExecuteAsync(model);
            Assert.True(result.IsValid);
            Assert.All(result.SubResults.Value, 
                x => Assert.True(x.IsValid));
        }

        [Fact]
        public async Task Test_Failed()
        {
            var model = TestData.GetInvalidModel();
            var pipe = new TestValidationPipe();
            var result = await pipe.ExecuteAsync(model);
            Assert.False(result.IsValid);
            //Assert.Contains(result.SubResults.Value, 
            //    x => x.ErrorMessage == "ModelType is Unknown");
            Assert.Contains(result.SubResults.Value, 
                x => x.ErrorMessage == "Should be Guid");
            Assert.Contains(result.SubResults.Value, 
                x => x.ErrorMessage == "Value is Money and must Currency == RUB");
        }
    }
}
