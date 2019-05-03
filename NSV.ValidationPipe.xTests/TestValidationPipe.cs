using System;
using System.Collections.Generic;
using System.Text;
using NSV.ValidationPipe;

namespace NSV.ValidationPipe.xTests
{
    public class TestValidationPipe : PipeValidator<ComplexTestModel>
    {
        public TestValidationPipe()
        {
            For(x => x.Id)
                .Path($"{nameof(ComplexTestModel)}.{nameof(ComplexTestModel.Id)}")
                .NotEmpty().WithMessage("Empty string")
                .IsGuid().WithMessage("Should be Guid")
                .Equal(TestConst.Guid_1)
                    .WithMessage("Should be equal to [f27a5b7f-0b7c-4e79-aff5-bdb0d34f3a9f]")
                .StartWith("f27a5b7f").WithMessage("Should start with [f27a5b7f-0b7c]")
                .Add()
            .ForCollection(x => x.Models)
                .AsParallel()
                .Path($"{nameof(ComplexTestModel)}.{nameof(ComplexTestModel.Models)}")
                .Set(new TestSubValidationPipe())
                .Add()
            .For(x => x.Models)
                .Path($"{nameof(ComplexTestModel)}.{nameof(ComplexTestModel.Models)}")
                .Must(x => x.Count > 1).WithMessage("Count less then 1")
                .Add();
        }
    }

    public class TestSubValidationPipe : PipeValidator<SimpleTestModel>
    {
        public TestSubValidationPipe()
        {
            For(x => x.ModelType)
                .Must(x => x != TestModelType.Unknown)
                .WithMessage("ModelType is Unknown")
                .Add()
            .If(x => x.TestValue is QuantityTestField)
                .For(x => x.TestValue as QuantityTestField).Must(x => x.Count > 1).WithMessage("");


        }
    }
}
