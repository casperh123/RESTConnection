namespace DataDownloader.Connection.RESTConnection;

public class Parameter(string parameterName, string value)
{
    public string ParameterName { get; } = parameterName;
    public string Value { get; } = value;

    public override string ToString()
    {
        if (ParameterName == "" || Value == "")
        {
            return "";
        }
        
        return  ParameterName + "=" + Value;
    }
}