import * as SignalR from "@aspnet/signalr";

const baseUrl = process.env.REACT_APP_API_URL;

export function subscribeTo(socketRoute, eventCallbackPairs) {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl(baseUrl + socketRoute, {
      accessTokenFactory: () => localStorage.getItem("access_token")
    })
    .configureLogging(SignalR.LogLevel.Error)
    .build();

  return connection
    .start()
    .then(() => {
      eventCallbackPairs.forEach(pair => {
        connection.on(pair.eventName, pair.callback);
      });
    })
    .catch(error => {
      throw error;
    });
}
