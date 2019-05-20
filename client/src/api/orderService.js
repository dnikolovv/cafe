import * as apiClient from "./apiClient";
import * as webSocketClient from "./webSocketClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const uuidv4 = require("uuid/v4");

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/order/");

export function getOrders() {
  return apiClient.get(baseUrl);
}

export function completeToGoOrder(orderId) {
  const url = joinUrlWithRoute(baseUrl, "complete");
  return apiClient.put(url, { orderId });
}

export function confirmToGoOrder(pricePaid, orderId) {
  const url = joinUrlWithRoute(baseUrl, "confirm");
  return apiClient.put(url, { orderId, pricePaid });
}

export function issueToGoOrder(itemNumbers) {
  return apiClient.post(baseUrl, { id: uuidv4(), itemNumbers });
}

export function onOrderConfirmed(callback) {
  return webSocketClient.subscribeTo("confirmedOrders", [
    {
      eventName: "OrderConfirmed",
      callback: ({ order }) => {
        callback(order);
      }
    }
  ]);
}
