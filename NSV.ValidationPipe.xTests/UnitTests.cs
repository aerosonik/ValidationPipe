using System;
using System.Collections.Generic;
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
    }
}
