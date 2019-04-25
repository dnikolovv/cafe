import * as SignalR from "@aspnet/signalr";

const baseUrl = process.env.REACT_APP_API_URL;

export function subscribeTo(socketRoute, eventName, callback) {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl(baseUrl + socketRoute, {
      accessTokenFactory: () => localStorage.getItem("access_token")
    })
    .configureLogging(SignalR.LogLevel.Error)
    .build();

  return connection
    .start()
    .then(() => {
      connection.on(eventName, callback);
    })
    .catch(error => {
      throw error;
    });
}
