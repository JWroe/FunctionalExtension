using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FunctionalExtension.Core;
using FunctionalExtension.Extensions;
using FunctionalExtension.Types;
using Shouldly;
using Xunit;
using static FunctionalExtension.Core.F;

namespace FunctionalExtension.Test.Examples
{
    /// <summary>
    ///     Some tests that also show example usage
    /// </summary>
    public class EmployeeTests
    {
        private static readonly Random Random = new Random();
        private const string WithWorkPermitId = nameof(WithWorkPermitId);
        private const string WithExpiredWorkPermitId = nameof(WithExpiredWorkPermitId);
        private const string WithoutWorkPermit = nameof(WithoutWorkPermit);

        [Theory]
        [InlineData(WithWorkPermitId, true)]
        [InlineData(WithoutWorkPermit)]
        [InlineData(WithExpiredWorkPermitId)]
        [InlineData("wrong id")]
        public void EmployeeFGetWorkPermit(string id, bool expected = false) =>
            Employees.ToImmutableDictionary(employee => employee.Id, employee => employee)
                     .GetWorkPermit(id)
                     .Match(some => true, none => false)
                     .ShouldBe(expected, $"{id} {(expected ? "should" : "should not")} have had a work permit, but one was {(!expected ? "found" : "not found'")}");

        private static readonly IImmutableList<Employee> Employees = ImmutableList<Employee>
                                                                     .Empty
                                                                     .Add(new Employee(WithWorkPermitId, Random.NextDateTime(), new WorkPermit(Random.NextString(), DateTime.MaxValue)))
                                                                     .Add(new Employee(WithExpiredWorkPermitId, Random.NextDateTime(), new WorkPermit(Random.NextString(), DateTime.MinValue)))
                                                                     .Add(new Employee(WithoutWorkPermit, Random.NextDateTime()));

        [Fact]
        public void EmployeeFYearsAtCompany() =>
            List((Started: DateTime.MinValue, Left: DateTime.MinValue.AddYears(5).AddDays(182.5).ReturnOption()),
                 (DateTime.UtcNow, None()))
                .Map(tuple => new Employee(Random.NextString(), tuple.Started, tuple.Left))
                .AverageYearsWorkedAtTheCompany()
                .ShouldBe(5.501369863013698d);
    }

    public static class EmployeeF
    {
        public static Option<WorkPermit> GetWorkPermit(this IDictionary<string, Employee> people, string employeeId) =>
            people.Lookup(employeeId)
                  .FlatMap(emp => emp.WorkPermit)
                  .Filter(permit => permit.Expiry > DateTime.UtcNow);

        public static double AverageYearsWorkedAtTheCompany(this IEnumerable<Employee> employees)
            => employees
               .FlatMap(e => e.LeftOn.Map(leftOn => YearsBetween(e.JoinedOn, leftOn)))
               .Average();

        private static double YearsBetween(DateTime start, DateTime end) => (end - start).Days / 365d;
    }

    public class Employee
    {
        public Employee(string id, DateTime joinedOn)
            : this(id,
                   joinedOn,
                   None(),
                   None())
        {
        }

        public Employee(string id, DateTime joinedOn, Option<WorkPermit> workPermit)
            : this(id,
                   joinedOn,
                   workPermit,
                   None())
        {
        }

        public Employee(string id, DateTime joinedOn, Option<DateTime> leftOn)
            : this(id,
                   joinedOn,
                   None(),
                   leftOn)
        {
        }

        public Employee(string id, DateTime joinedOn, WorkPermit workPermit, DateTime leftOn)
            : this(id,
                   joinedOn,
                   workPermit.ReturnOption(),
                   leftOn.ReturnOption())
        {
        }

        private Employee(string id, DateTime joinedOn, Option<WorkPermit> workPermit, Option<DateTime> leftOn)
        {
            Id = id;
            WorkPermit = workPermit;
            JoinedOn = joinedOn;
            LeftOn = leftOn;
        }

        public string Id { get; }
        public Option<WorkPermit> WorkPermit { get; }
        public DateTime JoinedOn { get; }
        public Option<DateTime> LeftOn { get; }
    }

    public class WorkPermit : IEquatable<WorkPermit>
    {
        public string Number { get; }
        public DateTime Expiry { get; }

        public WorkPermit(string number, DateTime expiry)
        {
            Number = number;
            Expiry = expiry;
        }

        public bool Equals(WorkPermit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Number == other.Number && Expiry.Equals(other.Expiry);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((WorkPermit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Number.GetHashCode() * 397) ^ Expiry.GetHashCode();
            }
        }
    }
}