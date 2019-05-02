import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/waiter/";

export function getWaiters() {
  return apiClient.get(baseUrl);
}

export function hireWaiter(waiter) {
  const url = baseUrl + "hire";
  return apiClient.post(url, waiter);
}

export function assignTable(assignment) {
  debugger;
  const url = baseUrl + "table/assign";
  return apiClient.post(url, assignment);
}
