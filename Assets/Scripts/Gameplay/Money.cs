using System;
using TriInspector;

[Serializable]
[InlineProperty]
[DeclareHorizontalGroup("Group")]
public struct Money : IEquatable<Money>, IComparable<Money>, IComparable
{
    public static readonly Money Zero = default(Money);

    [Group("Group")]
    [LabelText("\a$")]
    [LabelWidth(18)]
    public int Whole;
    [Group("Group")]
    [LabelText("\a.")]
    [LabelWidth(14)]
    public int Fractional;

    public const int MaxFractional = 100;

    public Money(int whole, int fractional = 0)
    {
        Whole = whole;
        Fractional = fractional;
    }

    public override bool Equals(object obj)
    {
        return obj is Money other && Equals(other);
    }

    public bool Equals(Money other)
    {
        return Whole == other.Whole && Fractional == other.Fractional;
    }

    public int CompareTo(object obj)
    {
        if (obj is not Money other)
        {
            throw new ArgumentException(nameof(obj));
        }

        return CompareTo(other);
    }

    public int CompareTo(Money other)
    {
        int wholeCompare = Whole.CompareTo(other.Whole);
        return wholeCompare != 0 ? wholeCompare : Fractional.CompareTo(other.Fractional);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Whole.GetHashCode(), Fractional.GetHashCode());
    }

    public override string ToString()
    {
        return $"${Whole:D1}.{Fractional:D2}";
    }

    public static bool operator ==(Money lhs, Money rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Money lhs, Money rhs)
    {
        return !lhs.Equals(rhs);
    }

    public static bool operator <(Money lhs, Money rhs)
    {
        return lhs.Whole != rhs.Whole ? lhs.Whole < rhs.Whole : lhs.Fractional < rhs.Fractional;
    }

    public static bool operator >(Money lhs, Money rhs)
    {
        return lhs.Whole != rhs.Whole ? lhs.Whole > rhs.Whole : lhs.Fractional > rhs.Fractional;
    }

    public static bool operator <=(Money lhs, Money rhs)
    {
        return lhs.Whole != rhs.Whole ? lhs.Whole < rhs.Whole : lhs.Fractional <= rhs.Fractional;
    }

    public static bool operator >=(Money lhs, Money rhs)
    {
        return lhs.Whole != rhs.Whole ? lhs.Whole > rhs.Whole : lhs.Fractional >= rhs.Fractional;
    }

    public static Money operator +(Money lhs, Money rhs)
    {
        int whole = lhs.Whole + rhs.Whole;
        int fractional = lhs.Fractional + rhs.Fractional;
        if (fractional > MaxFractional)
        {
            whole += 1;
            fractional -= MaxFractional;
        }

        return new Money(whole, fractional);
    }

    public static Money operator -(Money lhs, Money rhs)
    {
        int whole = lhs.Whole - rhs.Whole;
        int fractional = lhs.Fractional - rhs.Fractional;
        if (fractional < 0)
        {
            whole -= 1;
            fractional += MaxFractional;
        }

        return new Money(whole, fractional);
    }
}
