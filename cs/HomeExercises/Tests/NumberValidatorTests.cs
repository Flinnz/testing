using System;
using System.Collections;
using System.Configuration;
using NUnit.Framework;

namespace HomeExercises.Tests
{
	public class NumberValidatorTests
	{	
		private static IEnumerable ConstructorFailTestCases
		{
			get
			{
				yield return new TestCaseData(-3, 2, true)
					.SetName("WhenPrecisionIsNegative");
				yield return new TestCaseData(0, 2, true)
					.SetName("WhenPrecisionIsZero");
				yield return new TestCaseData(3, -2, true)
					.SetName("WhenScaleIsNegative");
				yield return new TestCaseData(1, 2, true)
					.SetName("WhenScaleIsLargerThanPrecision");
				yield return new TestCaseData(2, 2, true)
					.SetName("WhenScaleIsSameAsPrecision");
			}
		}
		
		[Test]
		[TestCaseSource(nameof(ConstructorFailTestCases))]
		public void Constructor_ShouldFail(int precision, int scale, bool onlyPositive)
		{
			Assert.Throws<ArgumentException>(() => new NumberValidator(precision, scale, onlyPositive));
		}

		private static IEnumerable ScaleTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.0")
					.Returns(true)
					.SetName("HasZeroScalePart");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.4")
					.Returns(true)
					.SetName("HasNonZeroScalePart");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0.42")
					.Returns(true)
					.SetName("ScalePartLengthIsTheSame_AsExpected");
			}
		}
		
		[Test] 
		[TestCaseSource(nameof(ScaleTestCases))]
		public bool IsValidNumber_TestScale(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}

		private static IEnumerable PrecisionTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(17, 2, true), "0")
					.Returns(true)
					.SetName("HasNotFractionalPart");
				yield return new TestCaseData(new NumberValidator(2, 1, true), "10")
					.Returns(true)
					.SetName("HasLengthSame_AsPrecision");
				yield return new TestCaseData(new NumberValidator(2, 1, true), "1.1")
					.Returns(true)
					.SetName("IntegerAndFractionPartLengthSame_AsPrecision");
				yield return new TestCaseData(new NumberValidator(4, 2, true), "+1.23")
					.Returns(true)
					.SetName("HasFractionalPartAndPlusSign");
			}
		}
		
		[Test] 
		[TestCaseSource(nameof(PrecisionTestCases))]
		public bool IsValidNumber_TestPrecision(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}

		private static IEnumerable NegativeSignTestCases
		{
			get
			{
				yield return new TestCaseData(new NumberValidator(4, 2), "-1.23")
					.Returns(true)
					.SetName("CanBeNegativeWithoutOnlyPositiveNumberValidatorArgument");
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-1.23")
					.Returns(true)
					.SetName("HasFractionalPart");
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-1")
					.Returns(true)
					.SetName("HasNotFractionalPart");
				yield return new TestCaseData(new NumberValidator(4, 2, false), "-001")
					.Returns(true)
					.SetName("HasZerosBetweenSignAndNonZeroNumber");
			}
		}
		
		
		[Test] 
		[TestCaseSource(nameof(NegativeSignTestCases))]
		public bool IsValidNumber_TestNegativeNumbers(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}
		
		private static IEnumerable IncorrectValidatorInputTestCases
		{
			get
			{				
				yield return new TestCaseData(new NumberValidator(4), "1.0")
					.Returns(false)
					.SetName("WhenNumberValidatorWithOneArgumentAndHasScale");
				yield return new TestCaseData(new NumberValidator(4, 2, true), "-1.23")
					.Returns(false)
					.SetName("WhenOnlyPositiveAndSignIsMinus");
				yield return new TestCaseData(new NumberValidator(3, 2, false), "-1.23")
					.Returns(false)
					.SetName("WhenPrecisionIsSameAsExpectedButStringValidatorArgumentIsSigned");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "a.1d")
					.Returns(false)
					.SetName("WhenStringValidatorArgumentHasNotOnlyNumbers");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "1.230")
					.Returns(false)
					.SetName("WhenScaleIsLargerThanExpected");
				yield return new TestCaseData(new NumberValidator(17, 2, true), null)
					.Returns(false)
					.SetName("WhenNullStringValidatorArgument");
				yield return new TestCaseData(new NumberValidator(17, 2, true), "")
					.Returns(false)
					.SetName("WhenEmptyStringValidatorArgument");
				yield return new TestCaseData(new NumberValidator(17, 2, true), ".")
					.Returns(false)
					.SetName("WhenSingleDotStringValidatorArgument");
				yield return new TestCaseData(new NumberValidator(17, 2, true), ".12")
					.Returns(false)
					.SetName("WhenDotWithoutIntegerPartStringValidatorArgument");
				yield return new TestCaseData(new NumberValidator(17, 2, false), "-+1.12")
					.Returns(false)
					.SetName("WhenTwoSign");
			}
		}
		
		[Test]
		[TestCaseSource(nameof(IncorrectValidatorInputTestCases))]
		public bool IsValidNumber_TestIncorrectValidatorInputs(NumberValidator validator, string representation)
		{
			return validator.IsValidNumber(representation);
		}
	}
}