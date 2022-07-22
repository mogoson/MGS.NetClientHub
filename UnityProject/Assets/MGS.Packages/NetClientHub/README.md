[TOC]

# MGS.NetClientHub

## Summary
- Client connect to remote server to request data.
- Hub to manage all clients.

## Platform

- Windows

## Environment

- Unity 5.0 or above.
- .Net Framework 3.5 or above.

## Demand

- Put request data from remote server.
- Post request data from remote server.
- Download file from remote server.
- Concentrative manage the clients and control concurrency.
- Retry times when error happened, example TimeoutException and WebException.
- Cache the client to share if the same url request.
- Cache the clinet result to share if the same url request.

## Usage

- Use the global instance of API.

```C#
var clinet  = NetClientHubAPI.Handler.Put(url, timeout);
while(!clinet.IsDone)
{
    //Show infos. example clinet.Speed, clinet.Progress...
    //If need abort the request, use clinet.Close();
}
//Show Request is done.
```

- Construct a instance of  API.

```C#
//Just need request
var client = new NetPutClient(url);//NetPostClient/NetFileClient

//Just need manage clients.
var hub = new NetClientHub();
hub.Put(url);//Post/Download

//Need manage clients and cache client and result.
var hub = new NetCacheClient();
hub.Put(url);//Post/Download
```

## Demo

- Demos in the path "MGS.Packages/NetClientHub/Demo/" provide reference to you.

## Source

- https://github.com/mogoson/MGS.NetClientHub


------

Copyright © 2022 Mogoson.	mogoson@outlook.com