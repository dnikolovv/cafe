import * as apiClient from "./apiClient";
import * as webSocketClient from "./webSocketClient";

const uuidv4 = require("uuid/v4");

const baseUrl = apiClient.BASE_URL + "/order/";

export function getOrders() {
  return apiClient.get(baseUrl);
}

export function completeToGoOrder(orderId) {
  const url = baseUrl + "complete";
  return apiClient.put(url, { orderId });
}

export function confirmToGoOrder(pricePaid, orderId) {
  const url = baseUrl + "confirm";
  return apiClient.put(url, { orderId, pricePaid });
}

export function issueToGoOrder(itemNumbers) {
  return apiClient.post(baseUrl, { id: uuidv4(), itemNumbers });
}

export function onOrderConfirmed(callback) {
  return webSocketClient.subscribeTo(
    "confirmedOrders",
    "OrderConfirmed",
    ({ order }) => {
      callback(order);
    }
  );
}
