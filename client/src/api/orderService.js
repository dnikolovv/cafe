import * as apiClient from "./apiClient";
const uuidv4 = require("uuid/v4");

const baseUrl = "/order/";

export function getOrders() {
  return apiClient.get(baseUrl);
}

export function confirmToGoOrder(pricePaid, orderId) {
  const url = baseUrl + "confirm";
  return apiClient.put(url, { orderId, pricePaid });
}

export function issueToGoOrder(itemNumbers) {
  return apiClient.post(baseUrl, { id: uuidv4(), itemNumbers });
}
