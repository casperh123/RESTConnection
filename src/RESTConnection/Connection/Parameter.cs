namespace RESTConnection.Connection;

public class Parameter(string key, string value)
{
    public string Key { get; } = key;
    public string Value { get; } = value;

    public override string ToString()
    {
        if (Key == "" || Value == "")
        {
            return "";
        }
        
        return  Key + "=" + Value;
    }
}