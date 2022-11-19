[TOC]

# MGS.NetClientHub

## Summary
- Client connect to remote server to request data.
- Hub to manage all clients.

## Platform

- Windows.
- Android.

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

## Design

- Waiting Clients, enqueue the idle clients.
- Working Clients, list the working clients.
- Result Cacher
  - Cache the result from client.
  - Get result for client if the request angs is match.
- Client Cacher
  - Cache the  client by key(from request args).
  - Get client for share if the request angs is match.
- Resolver
  - Check client retry by error types.
  - Record the retry times of client.

- Tick Update Thread
  - Dequeue waiting client to start to work.
  - Check working client status, resolve retry  when error.
  - Set result of client to cacher when client is done.

------

Copyright © 2022 Mogoson.	mogoson@outlook.com