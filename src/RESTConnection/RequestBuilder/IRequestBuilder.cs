﻿namespace DataDownloader.Connection.RESTConnection.RequestBuilder;

public interface IRequestBuilder
{
    HttpRequestMessage BuildRequest(HttpMethod method, string endpoint, object content = null, params Parameter[] parameters);
}