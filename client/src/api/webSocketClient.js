import * as SignalR from "@aspnet/signalr";

const baseUrl = process.env.REACT_APP_API_URL;

const openConnections = [];

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
      openConnections.push(connection);

      eventCallbackPairs.forEach(pair => {
        connection.on(pair.eventName, pair.callback);
      });
    })
    .catch(error => {
      throw error;
    });
}

export function closeAllOpenConnections() {
  openConnections.forEach(connection => {
    connection.stop();
  });
}
