import * as SignalR from "@aspnet/signalr";

const baseUrl = process.env.REACT_APP_API_URL;

export function subscribeTo(socketRoute, eventName, callback) {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl(baseUrl + socketRoute)
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
