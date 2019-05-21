using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSV.ValidationPipe;
using NSV.ValidationPipe.Extensions;

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
                    .WithMessage($"Should be equal to [{TestConst.Guid_1}]")
                .StartWith("f27a5b7f")
                    .WithMessage("Should start with [f27a5b7f-0b7c]")
                .Add()
            .ForCollection(x => x.Models)
                //.AsParallel()
                .Path($"{nameof(ComplexTestModel)}.{nameof(ComplexTestModel.Models)}")
                .Set(new TestSubValidationPipe())
                .Add()
            .For(x => x.Models)
                .Path($"{nameof(ComplexTestModel)}.{nameof(ComplexTestModel.Models)}")
                .Must(x => x.Count > 1)
                    .WithMessage("Count less then 1")
                .Add();
        }
    }

    public class TestSubValidationPipe : PipeValidator<SimpleTestModel>
    {
        public TestSubValidationPipe()
        {
            For(x => x.ModelType)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.ModelType)}")
                .Must(x => x != TestModelType.Unknown)
                .WithMessage("ModelType is Unknown")
                .Add()
            .If(x => x.TestValue is QuantityTestField)
                .For(x => x.TestValue as QuantityTestField)
                    .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.TestValue)}")
                    .Must(x => x.Count > 1)
                        .WithMessage("Value is Quantity and must Count > 1")
                    .Add()
            .EndIf()
            .If(x => x.TestValue is PeriodTestField)
                .For(x => x.TestValue as PeriodTestField)
                    .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.TestValue)}")
                    .Must(x => x.From < DateTime.Now)
                        .WithMessage("Value is Period and must From < Now")
                    .Must(x => x.To > DateTime.Now)
                        .WithMessage("Value is Period and must To > Now")
                    .Add()
            .EndIf()
            .If(x => x.TestValue is DateTestField)
                .For(x => x.TestValue as DateTestField)
                    .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.TestValue)}")
                    .Must(x => x.Date.Day == DateTime.Now.Day)
                        .WithMessage("Value is Date and must Date.Day == Now.Day")
                    .Add()
            .EndIf()
            .If(x => x.TestValue is MoneyTestField)
                .For(x => x.TestValue as MoneyTestField)
                    .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.TestValue)}")
                    .Must(x => x.Amount > 0)
                        .WithMessage("Value is Money and must Amount > 0")
                    .Must(x => x.Currency == Currency.RUB)
                        .WithMessage("Value is Money and must Currency == RUB")
                    .Add()
            .EndIf()
            .For(x => x.ComplexFields)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.ComplexFields)}")
                .NotEmpty("ComplexFields is Empty")
                .Add()
            .ForCollection(x => x.ComplexFields)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.ComplexFields)}")
                .Must(x => x.System == TestConst.Guid_2)
                    .WithMessage($"Not All systems equal [{TestConst.Guid_2}]")
                .Set(new TestComplexFieldValidationPipe())
                .Add()
            .For(x => x.Created)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.Created)}")
                .Less(DateTime.Now)
                    .WithMessage("Invalid Created DateTime")
                .Between(DateTime.Now.AddDays(-5), DateTime.Now)
                    .WithMessage("Invalid period of Created DateTime")
                .Add()
            .For(x => x.Duration)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.Duration)}")
                .Must(x => x > TimeSpan.MinValue)
                    .WithMessage("Duration is too short")
                .Add()
            .For(x => x.Text)
                 .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.Text)}")
                 .StartWith("AAA").WithMessage("Should Start with [AAA]")
                 .EndWith("zzzz").WithMessage("Shouls end with [zzzz]")
                 .Contains("ccc").WithMessage("Should contains [ccc]")
                 .Add()
            .For(x => x.Binary)
                .Path($"{nameof(SimpleTestModel)}.{nameof(SimpleTestModel.Binary)}")
                .NotEmpty("Binary Array should'n be Empty")
                .Any(x => x > 1, "At least one item should be greater than 1")
                .Contains((byte)100, "At least one item should be 100")
                .When(x=> x.Binary != null && x.Binary.Any())
                .Add();
        }
    }

    public class TestComplexFieldValidationPipe : PipeValidator<ComplexTestField>
    {
        public TestComplexFieldValidationPipe()
        {
            For(x => x.Subfields)
                .NotEmpty()
                    .WithMessage("Subfield list is empty")
                .Any(y => y.Version != null && y.Version > 5)
                    .WithMessage("Any subfield with Version greater then 5 Not found")
                .Add()
            .ForCollection(x => x.Subfields)
                .Path($"{nameof(ComplexTestField)}.{nameof(ComplexTestField.Subfields)}")
                .Set(new ComplexTestSubFieldValidator())
                .When(x=> x.Subfields != null && x.Subfields.Any())
                .Add();
        }
    }

    public class ComplexTestSubFieldValidator : IValidator<ComplexTestSubField>
    {
        public ValidateResultWrapper Validate(ComplexTestSubField field)
        {
            var result = ValidateResult.Default;
            if (string.IsNullOrWhiteSpace(field.Code))
                result =  ValidateResult.DefaultFailed
                    .SetErrorMessage("Code is Empty");

            if(field.Version != null && field.Version == 0)
                result = ValidateResult.DefaultFailed
                    .SetErrorMessage("Version is 0");

            if(string.IsNullOrWhiteSpace(field.System))
                result = ValidateResult.DefaultFailed
                    .SetErrorMessage("System is Empty");

            if (field.System != TestConst.Guid_3)
                result = ValidateResult.DefaultFailed
                    .SetErrorMessage($"System not equal [{TestConst.Guid_3}]");

            return ValidateResultWrapper.Create(result);
        }
    }
}
