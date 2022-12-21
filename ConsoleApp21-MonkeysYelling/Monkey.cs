public class Monkey
{
    public string GetStrValue(IDictionary<string, Monkey> allMonkeys)
    {
        if (this.SafeValue.HasValue)
        {
            return this.SafeValue.Value.ToString();
        }
        else if (this.Name == "humn")
        {
            return "humn";
        }
        else
        {
            return StrOperation(allMonkeys[this.Op1].GetStrValue(allMonkeys), allMonkeys[this.Op2].GetStrValue(allMonkeys));
        }
    }

    public long GetValue(IDictionary<string, long> unsafeCache, IDictionary<string, Monkey> allMonkeys)
    {
        if (this.SafeValue.HasValue)
        {
            return SafeValue.Value;
        }

        if (unsafeCache.TryGetValue(this.Name, out long val))
        {
            return val;
        }

        val = Operation(allMonkeys[this.Op1].GetValue(unsafeCache, allMonkeys), allMonkeys[this.Op2].GetValue(unsafeCache, allMonkeys));
        unsafeCache[this.Name] = val;

        return val;
    }

    public long? CalculateSafeValueAndCache(IDictionary<string, Monkey> allMonkeys)
    {
        if (this.SafeValue.HasValue)
        {
            return this.SafeValue;
        }

        if (this.Name == "humn" || this.Name == "root")
        {
            return null;
        }

        long? safe1 = allMonkeys[this.Op1].CalculateSafeValueAndCache(allMonkeys);
        long? safe2 = allMonkeys[this.Op2].CalculateSafeValueAndCache(allMonkeys);

        if (safe1.HasValue && safe2.HasValue)
        {
            this.SafeValue = Operation(safe1.Value, safe2.Value);

            return this.SafeValue;
        }

        return null;
    }

    public long? SafeValue { get; set; }

    public string Name { get; set; }

    public string Op1 { get; set; }

    public string Op2 { get; set; }

    public Func<long, long, long> Operation { get; set; }

    public Func<string, string, string> StrOperation { get; set; }
}