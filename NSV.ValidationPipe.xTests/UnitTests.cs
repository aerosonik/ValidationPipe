using NSV.ExecutionPipe.Models;
using System.Threading.Tasks;
using Xunit;
using System.Linq; 

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
            Assert.False(result.IsAnyFailed());
            Assert.All(result, 
                x => Assert.True(x.Success == ExecutionResult.Successful ||
                                 x.Success == ExecutionResult.Initial));
        }

        [Fact]
        public async Task Test_Failed()
        {
            var model = TestData.GetInvalidModel();
            var pipe = new TestValidationPipe();
            var result = await pipe.ExecuteAsync(model);
            Assert.False(result.IsAllValid());
            Assert.True(result.IsAnyFailed());
            Assert.Contains(result,
                x => x.ErrorMessage == "ModelType is Unknown");
            Assert.Contains(result, 
                x => x.ErrorMessage == "Should be Guid");
            Assert.Contains(result, 
                x => x.ErrorMessage == "Value is Money and must Currency == RUB");
        }
    }
}
